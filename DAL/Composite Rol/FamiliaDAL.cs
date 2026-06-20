using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;

namespace DAL
{
    public class FamiliaDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL = new DAO_AccesoDatos();

        // ===== LECTURA =====

        public List<Familia> ListarTodas()
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();

            // 1) Cargamos todos los Permisos y todas las Familias (cáscaras) en diccionarios
            //    para resolver las referencias rápidamente en memoria.
            var permisos = LeerPermisos(conexion);
            var familias = LeerFamilias(conexion);

            // 2) Resolvemos los hijos: una Familia puede contener otras Familias y/o PermisoSimples.
            AgregarHijosDeRelaciones(conexion,
                "SELECT IdFamiliaPadre, IdFamiliaHija FROM Familia_Familia",
                "IdFamiliaPadre", "IdFamiliaHija",
                familias, familias);

            AgregarHijosDeRelaciones(conexion,
                "SELECT IdFamilia, IdPermisoSimple FROM PermisoSimple_Familia",
                "IdFamilia", "IdPermisoSimple",
                familias, permisos);

            return familias.Values.OrderBy(f => f.Nombre).ToList();
        }

        public Familia ObtenerPorId(int idFamilia) =>
            ListarTodas().FirstOrDefault(f => f.IdFamilia == idFamilia);

        /// <summary>
        /// True si la Familia esta siendo usada por otras Familias (como hija)
        /// o por Roles. NO cuenta sus propias relaciones (composicion propia).
        /// Se usa para decidir si es seguro eliminar fisicamente.
        /// </summary>
        public bool EstaUsada(int idFamilia)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(@"
                SELECT
                    (SELECT COUNT(1) FROM Familia_Familia WHERE IdFamiliaHija = @Id)
                  + (SELECT COUNT(1) FROM Rol_Familia    WHERE IdFamilia    = @Id)
                AS Cnt", conexion);
            cmd.Parameters.AddWithValue("@Id", idFamilia);
            conexion.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public bool ExistePorNombre(string nombre, int idExcluir = 0)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Familia WHERE Nombre = @Nombre AND IdFamilia <> @Excluir", conexion);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Excluir", idExcluir);
            conexion.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // ===== ESCRITURA (transaccional) =====

        public int InsertarConComposicion(Familia familia)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();
            using SqlTransaction tx = conexion.BeginTransaction();
            try
            {
                int nuevoId = InsertarCascaraFamilia(conexion, tx, familia);
                InsertarHijos(conexion, tx, nuevoId, familia.Hijos);
                tx.Commit();
                return nuevoId;
            }
            catch { tx.Rollback(); throw; }
        }

        public void ModificarConComposicion(Familia familia)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();
            using SqlTransaction tx = conexion.BeginTransaction();
            try
            {
                Ejecutar(conexion, tx,
                    "UPDATE Familia SET Nombre = @Nombre WHERE IdFamilia = @Id",
                    ("@Nombre", familia.Nombre), ("@Id", familia.IdFamilia));

                Ejecutar(conexion, tx, "DELETE FROM PermisoSimple_Familia WHERE IdFamilia = @Id", ("@Id", familia.IdFamilia));
                Ejecutar(conexion, tx, "DELETE FROM Familia_Familia WHERE IdFamiliaPadre = @Id", ("@Id", familia.IdFamilia));

                InsertarHijos(conexion, tx, familia.IdFamilia, familia.Hijos);

                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }

        /// <summary>
        /// Elimina FISICAMENTE una Familia con su composicion propia.
        /// El BLL debe haber validado antes que NO este en uso.
        /// Todo en una transaccion: si algo falla, rollback.
        /// </summary>
        public void Eliminar(int idFamilia)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();
            using SqlTransaction tx = conexion.BeginTransaction();
            try
            {
                // Borrar composicion propia
                Ejecutar(conexion, tx, "DELETE FROM PermisoSimple_Familia WHERE IdFamilia = @Id", ("@Id", idFamilia));
                Ejecutar(conexion, tx, "DELETE FROM Familia_Familia WHERE IdFamiliaPadre = @Id", ("@Id", idFamilia));

                // Borrar cascara
                using SqlCommand cmd = new SqlCommand("DELETE FROM Familia WHERE IdFamilia = @Id", conexion, tx);
                cmd.Parameters.AddWithValue("@Id", idFamilia);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception("No se encontró la Familia a eliminar.");

                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }

        // ===== HELPERS PRIVADOS =====

        private Dictionary<int, PermisoSimple> LeerPermisos(SqlConnection conexion)
        {
            var dict = new Dictionary<int, PermisoSimple>();
            using SqlCommand cmd = new SqlCommand("SELECT IdPermisoSimple, Codigo, Nombre FROM PermisoSimple", conexion);
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                int id = (int)r["IdPermisoSimple"];
                dict[id] = new PermisoSimple { IdPermisoSimple = id, Codigo = r["Codigo"].ToString(), Nombre = r["Nombre"].ToString() };
            }
            return dict;
        }

        private Dictionary<int, Familia> LeerFamilias(SqlConnection conexion)
        {
            var dict = new Dictionary<int, Familia>();
            using SqlCommand cmd = new SqlCommand("SELECT IdFamilia, Nombre FROM Familia", conexion);
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                int id = (int)r["IdFamilia"];
                dict[id] = new Familia { IdFamilia = id, Nombre = r["Nombre"].ToString() };
            }
            return dict;
        }

        // Lee una tabla relacional y agrega cada hijo al padre correspondiente.
        // Genérico: sirve tanto para Familia_Familia como para PermisoSimple_Familia.
        private void AgregarHijosDeRelaciones<TPadre, THijo>(
            SqlConnection conexion, string sql, string colPadre, string colHijo,
            Dictionary<int, TPadre> padres, Dictionary<int, THijo> hijos)
            where TPadre : Componente where THijo : Componente
        {
            using SqlCommand cmd = new SqlCommand(sql, conexion);
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                int idPadre = (int)r[colPadre];
                int idHijo = (int)r[colHijo];
                if (padres.TryGetValue(idPadre, out TPadre padre) && hijos.TryGetValue(idHijo, out THijo hijo))
                {
                    if (padre is Familia fam) fam.Agregar(hijo);
                }
            }
        }

        private int InsertarCascaraFamilia(SqlConnection conexion, SqlTransaction tx, Familia familia)
        {
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO Familia (Nombre) OUTPUT INSERTED.IdFamilia VALUES (@Nombre)",
                conexion, tx);
            cmd.Parameters.AddWithValue("@Nombre", familia.Nombre);
            return (int)cmd.ExecuteScalar();
        }

        private void InsertarHijos(SqlConnection conexion, SqlTransaction tx, int idFamilia, List<Componente> hijos)
        {
            foreach (Componente hijo in hijos)
            {
                if (hijo is PermisoSimple ps)
                    Ejecutar(conexion, tx,
                        "INSERT INTO PermisoSimple_Familia (IdFamilia, IdPermisoSimple) VALUES (@Fam, @Ps)",
                        ("@Fam", idFamilia), ("@Ps", ps.IdPermisoSimple));
                else if (hijo is Familia fam)
                    Ejecutar(conexion, tx,
                        "INSERT INTO Familia_Familia (IdFamiliaPadre, IdFamiliaHija) VALUES (@Padre, @Hija)",
                        ("@Padre", idFamilia), ("@Hija", fam.IdFamilia));
            }
        }

        private void Ejecutar(SqlConnection conexion, SqlTransaction tx, string sql, params (string nombre, object valor)[] parametros)
        {
            using SqlCommand cmd = new SqlCommand(sql, conexion, tx);
            foreach (var p in parametros) cmd.Parameters.AddWithValue(p.nombre, p.valor);
            cmd.ExecuteNonQuery();
        }
    }
}
