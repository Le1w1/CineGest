using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;

namespace DAL
{
    public class RolDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL = new DAO_AccesoDatos();
        private readonly FamiliaDAL _familiaDAL = new FamiliaDAL();


        //Lista todos los Roles con su composicion completa (Familias y Permisos Simples).
        public List<Rol> ListarTodos()
        {
            var familias = _familiaDAL.ListarTodas().ToDictionary(f => f.IdFamilia);

            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();

            var permisos = LeerPermisos(conexion);
            var roles = LeerRoles(conexion);

            AgregarHijos(conexion, "SELECT IdRol, IdFamilia FROM Rol_Familia","IdRol", "IdFamilia", roles, familias);

            AgregarHijos(conexion, "SELECT IdRol, IdPermisoSimple FROM Rol_PermisoSimple","IdRol", "IdPermisoSimple", roles, permisos);

            return roles.Values.OrderBy(r => r.Nombre).ToList();
        }


        /// Devuelve el Rol con su composicion completa (Familias y Permisos Simples).
        public Rol ObtenerPorId(int idRol) =>ListarTodos().FirstOrDefault(r => r.IdRol == idRol);

        

        public bool EstaUsado(int idRol)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Usuario WHERE IdRol = @Id", conexion);
            cmd.Parameters.AddWithValue("@Id", idRol);
            conexion.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public bool ExistePorNombre(string nombre, int idExcluir = 0)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Rol WHERE Nombre = @Nombre AND IdRol <> @Excluir", conexion);
            cmd.Parameters.AddWithValue("@Nombre", nombre);
            cmd.Parameters.AddWithValue("@Excluir", idExcluir);
            conexion.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

       
        /// Devuelve los Roles asignados al usuario. Como el modelo es 1:1(Usuario.IdRol),la lista tendrá siempre 0 o 1 elemento.
        /// Se mantiene la firma con List por compatibilidad con SM.EstablecerRolesUsuario.
        public List<Rol> ListarRolesDeUsuario(int idUsuario)
        {
            int idRol;

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("SELECT IdRol FROM Usuario WHERE IdUsuario = @IdUsuario", conexion))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                conexion.Open();
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) return new List<Rol>();
                idRol = Convert.ToInt32(result);
            }

            Rol rol = ObtenerPorId(idRol);
            return rol == null ? new List<Rol>() : new List<Rol> { rol };
        }


        //Diccionario de Roles con su composicion completa (Familias y Permisos Simples).
        private Dictionary<int, Rol> LeerRoles(SqlConnection conexion)
        {
            var dict = new Dictionary<int, Rol>();
            using SqlCommand cmd = new SqlCommand("SELECT IdRol, Nombre FROM Rol", conexion);
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                int id = (int)r["IdRol"];
                dict[id] = new Rol { IdRol = id, Nombre = r["Nombre"].ToString() };
            }
            return dict;
        }

       
        /// Inserta un Rol con su composicion completa (Familias y Permisos Simples).
        public int InsertarConComposicion(Rol rol)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();
            using SqlTransaction tx = conexion.BeginTransaction();
            try
            {
                int nuevoId = InsertarCascaraRol(conexion, tx, rol);
                InsertarHijosDeRol(conexion, tx, nuevoId, rol.Hijos);
                tx.Commit();
                return nuevoId;
            }
            catch { tx.Rollback(); throw; }
        }


        /// Modifica un Rol con su composicion completa (Familias y Permisos Simples).
        public void ModificarConComposicion(Rol rol)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();
            using SqlTransaction tx = conexion.BeginTransaction();
            try
            {
                Ejecutar(conexion, tx,"UPDATE Rol SET Nombre = @Nombre WHERE IdRol = @Id",("@Nombre", rol.Nombre), ("@Id", rol.IdRol));
                Ejecutar(conexion, tx, "DELETE FROM Rol_Familia WHERE IdRol = @Id", ("@Id", rol.IdRol));
                Ejecutar(conexion, tx, "DELETE FROM Rol_PermisoSimple WHERE IdRol = @Id", ("@Id", rol.IdRol));
                InsertarHijosDeRol(conexion, tx, rol.IdRol, rol.Hijos);

                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }


        /// Elimina FISICAMENTE un Rol con su composicion propia.
        /// El BLL debe haber validado antes que NO este asignado a Usuarios.
        public void Eliminar(int idRol)
        {
            using SqlConnection conexion = _conexionDAL.ObtenerConexion();
            conexion.Open();
            using SqlTransaction tx = conexion.BeginTransaction();
            try
            {
                Ejecutar(conexion, tx, "DELETE FROM Rol_Familia WHERE IdRol = @Id", ("@Id", idRol));
                Ejecutar(conexion, tx, "DELETE FROM Rol_PermisoSimple WHERE IdRol = @Id", ("@Id", idRol));

                using SqlCommand cmd = new SqlCommand("DELETE FROM Rol WHERE IdRol = @Id", conexion, tx);
                cmd.Parameters.AddWithValue("@Id", idRol);
                if (cmd.ExecuteNonQuery() == 0) throw new Exception("No se encontró el Rol a eliminar.");

                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }


        // Diccionario de Permisos Simples para poder agregarlos a los Roles y Familias.
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


        // Agrega a cada Rol su composicion completa (Familias y Permisos Simples) a partir de los diccionarios de Roles y Familias/Permisos.
        private void AgregarHijos<THijo>(SqlConnection conexion, string sql, string colPadre, string colHijo,Dictionary<int, Rol> roles, Dictionary<int, THijo> hijos) where THijo : Componente
        {
            using SqlCommand cmd = new SqlCommand(sql, conexion);
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                int idRol = (int)r[colPadre];
                int idHijo = (int)r[colHijo];
                if (roles.TryGetValue(idRol, out Rol rol) && hijos.TryGetValue(idHijo, out THijo hijo))
                    rol.Agregar(hijo);
            }
        }


        /// Inserta la "cáscara" del Rol (solo el registro en la tabla Rol) y devuelve el Id generado.
        private int InsertarCascaraRol(SqlConnection conexion, SqlTransaction tx, Rol rol)
        {
            using SqlCommand cmd = new SqlCommand("INSERT INTO Rol (Nombre) OUTPUT INSERTED.IdRol VALUES (@Nombre)",conexion, tx);
            cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
            return (int)cmd.ExecuteScalar();
        }


        // Inserta los hijos (Familias y Permisos Simples) de un Rol en las tablas intermedias.
        private void InsertarHijosDeRol(SqlConnection conexion, SqlTransaction tx, int idRol, List<Componente> hijos)
        {
            foreach (Componente hijo in hijos)
            {
                if (hijo is PermisoSimple ps)
                    Ejecutar(conexion, tx,"INSERT INTO Rol_PermisoSimple (IdRol, IdPermisoSimple) VALUES (@Rol, @Ps)",("@Rol", idRol), ("@Ps", ps.IdPermisoSimple));
                else if (hijo is Familia fam)
                    Ejecutar(conexion, tx,"INSERT INTO Rol_Familia (IdRol, IdFamilia) VALUES (@Rol, @Fam)",("@Rol", idRol), ("@Fam", fam.IdFamilia));
            }
        }


        // Metodo auxiliar para ejecutar comandos SQL
        private void Ejecutar(SqlConnection conexion, SqlTransaction tx, string sql, params (string nombre, object valor)[] parametros)
        {
            using SqlCommand cmd = new SqlCommand(sql, conexion, tx);
            foreach (var p in parametros) cmd.Parameters.AddWithValue(p.nombre, p.valor);
            cmd.ExecuteNonQuery();
        }
    }
}
