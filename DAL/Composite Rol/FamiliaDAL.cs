using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;

namespace DAL
{
    public class FamiliaDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public FamiliaDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        #region "LECTURA"

        
        public List<Familia> ListarTodas(bool incluirInactivas = false)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                conexion.Open();

                //Carga todos los PermisoSimples
                Dictionary<int, PermisoSimple> permisos = LeerTodosPermisos(conexion);

                //Carga todas las Familias 
                Dictionary<int, Familia> familias = LeerTodasFamiliasBase(conexion, incluirInactivas);

                ResolverHijasFamilias(conexion, familias);

                ResolverPermisosDeFamilias(conexion, familias, permisos);

                return familias.Values.OrderBy(f => f.Nombre).ToList();
            }
        }

        public Familia ObtenerPorId(int idFamilia)
        {
            return ListarTodas(incluirInactivas: true).FirstOrDefault(f => f.IdFamilia == idFamilia);
        }

        public bool ExistePorNombre(string nombre, int idFamiliaExcluir = 0)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Familia
                                 WHERE Nombre = @Nombre AND IdFamilia <> @Excluir";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
                    comando.Parameters.Add("@Excluir", SqlDbType.Int).Value = idFamiliaExcluir;

                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        #endregion

        #region "ESCRITURA"

        /// Inserta una Familia nueva + toda su composicion atomicamente, si algo falla, rollback completo. Devuelve el IdFamilia generado.
        public int InsertarConComposicion(Familia familia)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        int nuevoId = InsertarFamiliaBase(conexion, transaccion, familia);
                        InsertarRelacionesDeFamilia(conexion, transaccion, nuevoId, familia);

                        transaccion.Commit();
                        return nuevoId;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        /// Reemplaza la composicion de una Familia existente. Borra las relaciones actuales y carga las nuevas, todo en una transaccion.
        public void ModificarConComposicion(Familia familia)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string updateQuery = @"UPDATE Familia
                                               SET Nombre = @Nombre, Activo = @Activo
                                               WHERE IdFamilia = @Id";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conexion, transaccion))
                        {
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = familia.Nombre;
                            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = familia.Activo;
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = familia.IdFamilia;

                            int filas = cmd.ExecuteNonQuery();
                            if (filas == 0) throw new Exception("No se encontró la Familia a modificar.");
                        }

                        EjecutarNonQuery(conexion, transaccion,"DELETE FROM PermisoSimple_Familia WHERE IdFamilia = @Id",("@Id", SqlDbType.Int, familia.IdFamilia));

                        EjecutarNonQuery(conexion, transaccion,"DELETE FROM Familia_Familia WHERE IdFamiliaPadre = @Id",("@Id", SqlDbType.Int, familia.IdFamilia));

                        InsertarRelacionesDeFamilia(conexion, transaccion, familia.IdFamilia, familia);

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Desactivar(int idFamilia)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"UPDATE Familia SET Activo = 0 WHERE IdFamilia = @Id";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Id", SqlDbType.Int).Value = idFamilia;

                    conexion.Open();
                    int filas = comando.ExecuteNonQuery();
                    if (filas == 0) throw new Exception("No se encontró la Familia a desactivar.");
                }
            }
        }

        #endregion

        #region "Metodos Refactorizados"

        private Dictionary<int, PermisoSimple> LeerTodosPermisos(SqlConnection conexion)
        {
            Dictionary<int, PermisoSimple> dict = new Dictionary<int, PermisoSimple>();

            string query = @"SELECT IdPermisoSimple, Codigo, Nombre FROM PermisoSimple";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    PermisoSimple ps = new PermisoSimple
                    {
                        IdPermisoSimple = Convert.ToInt32(reader["IdPermisoSimple"]),
                        Codigo = reader["Codigo"].ToString(),
                        Nombre = reader["Nombre"].ToString()
                    };
                    dict[ps.IdPermisoSimple] = ps;
                }
            }

            return dict;
        }

        private Dictionary<int, Familia> LeerTodasFamiliasBase(SqlConnection conexion, bool incluirInactivas)
        {
            Dictionary<int, Familia> dict = new Dictionary<int, Familia>();

            string query = @"SELECT IdFamilia, Nombre, Activo FROM Familia";
            if (!incluirInactivas) query += " WHERE Activo = 1";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Familia f = new Familia
                    {
                        IdFamilia = Convert.ToInt32(reader["IdFamilia"]),
                        Nombre = reader["Nombre"].ToString(),
                        Activo = Convert.ToBoolean(reader["Activo"])
                    };
                    dict[f.IdFamilia] = f;
                }
            }

            return dict;
        }

        private void ResolverHijasFamilias(SqlConnection conexion, Dictionary<int, Familia> familias)
        {
            string query = @"SELECT IdFamiliaPadre, IdFamiliaHija FROM Familia_Familia";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idPadre = Convert.ToInt32(reader["IdFamiliaPadre"]);
                    int idHija = Convert.ToInt32(reader["IdFamiliaHija"]);

                    if (familias.ContainsKey(idPadre) && familias.ContainsKey(idHija))
                    {
                        familias[idPadre].Agregar(familias[idHija]);
                    }
                }
            }
        }

        private void ResolverPermisosDeFamilias(
            SqlConnection conexion,
            Dictionary<int, Familia> familias,
            Dictionary<int, PermisoSimple> permisos)
        {
            string query = @"SELECT IdFamilia, IdPermisoSimple FROM PermisoSimple_Familia";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idFam = Convert.ToInt32(reader["IdFamilia"]);
                    int idPs = Convert.ToInt32(reader["IdPermisoSimple"]);

                    if (familias.ContainsKey(idFam) && permisos.ContainsKey(idPs))
                    {
                        familias[idFam].Agregar(permisos[idPs]);
                    }
                }
            }
        }

        private int InsertarFamiliaBase(SqlConnection conexion, SqlTransaction transaccion, Familia familia)
        {
            string query = @"INSERT INTO Familia (Nombre, Activo)
                             OUTPUT INSERTED.IdFamilia
                             VALUES (@Nombre, @Activo)";

            using (SqlCommand cmd = new SqlCommand(query, conexion, transaccion))
            {
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = familia.Nombre;
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = familia.Activo;
                return (int)cmd.ExecuteScalar();
            }
        }

        private void InsertarRelacionesDeFamilia(SqlConnection conexion,SqlTransaction transaccion,int idFamilia,Familia familia)
        {
            foreach (Componente hijo in familia.Hijos)
            {
                if (hijo is PermisoSimple ps)
                {
                    string sql = @"INSERT INTO PermisoSimple_Familia (IdFamilia, IdPermisoSimple)
                                   VALUES (@IdFamilia, @IdPs)";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion, transaccion))
                    {
                        cmd.Parameters.Add("@IdFamilia", SqlDbType.Int).Value = idFamilia;
                        cmd.Parameters.Add("@IdPs", SqlDbType.Int).Value = ps.IdPermisoSimple;
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (hijo is Familia famHija)
                {
                    string sql = @"INSERT INTO Familia_Familia (IdFamiliaPadre, IdFamiliaHija)
                                   VALUES (@Padre, @Hija)";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion, transaccion))
                    {
                        cmd.Parameters.Add("@Padre", SqlDbType.Int).Value = idFamilia;
                        cmd.Parameters.Add("@Hija", SqlDbType.Int).Value = famHija.IdFamilia;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void EjecutarNonQuery(SqlConnection conexion,SqlTransaction transaccion,string sql,params (string nombre, SqlDbType tipo, object valor)[] parametros)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conexion, transaccion))
            {
                foreach (var p in parametros)
                {
                    cmd.Parameters.Add(p.nombre, p.tipo).Value = p.valor;
                }
                cmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
