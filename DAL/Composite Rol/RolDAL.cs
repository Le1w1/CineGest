using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;

namespace DAL
{
    public class RolDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;
        private readonly FamiliaDAL _familiaDAL;

        public RolDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
            _familiaDAL = new FamiliaDAL();
        }

        #region "LECTURA"

        /// Devuelve todos los Roles con su composicion completa.
        public List<Rol> ListarTodos(bool incluirInactivos = false)
        {
            
            Dictionary<int, Familia> familias = _familiaDAL.ListarTodas(incluirInactivas: true).ToDictionary(f => f.IdFamilia);

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                conexion.Open();

                //carga todos los PermisoSimples
                Dictionary<int, PermisoSimple> permisos = LeerTodosPermisos(conexion);

                //Carga Roles 
                Dictionary<int, Rol> roles = LeerTodosRolesBase(conexion, incluirInactivos);

                ResolverFamiliasDeRoles(conexion, roles, familias);

                ResolverPermisosDeRoles(conexion, roles, permisos);

                return roles.Values.OrderBy(r => r.Nombre).ToList();
            }
        }

        public Rol ObtenerPorId(int idRol)
        {
            return ListarTodos(incluirInactivos: true).FirstOrDefault(r => r.IdRol == idRol);
        }

        public bool ExistePorNombre(string nombre, int idRolExcluir = 0)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Rol
                                 WHERE Nombre = @Nombre AND IdRol <> @Excluir";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;
                    cmd.Parameters.Add("@Excluir", SqlDbType.Int).Value = idRolExcluir;
                    conexion.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public List<Rol> ListarRolesDeUsuario(int idUsuario)
        {
            List<Rol> todos = ListarTodos(incluirInactivos: false);

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT IdRol FROM Usuario_Rol WHERE IdUsuario = @IdUsuario";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    conexion.Open();

                    HashSet<int> idsAsignados = new HashSet<int>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idsAsignados.Add(Convert.ToInt32(reader["IdRol"]));
                        }
                    }

                    return todos.Where(r => idsAsignados.Contains(r.IdRol)).ToList();
                }
            }
        }

        #endregion

        #region "ESCRITURA"

        public int InsertarConComposicion(Rol rol)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction tx = conexion.BeginTransaction())
                {
                    try
                    {
                        int nuevoId = InsertarRolBase(conexion, tx, rol);
                        InsertarRelacionesDeRol(conexion, tx, nuevoId, rol);

                        tx.Commit();
                        return nuevoId;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void ModificarConComposicion(Rol rol)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction tx = conexion.BeginTransaction())
                {
                    try
                    {
                        string updateQuery = @"UPDATE Rol
                                               SET Nombre = @Nombre, Activo = @Activo
                                               WHERE IdRol = @Id";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conexion, tx))
                        {
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = rol.Nombre;
                            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = rol.Activo;
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = rol.IdRol;

                            int filas = cmd.ExecuteNonQuery();
                            if (filas == 0) throw new Exception("No se encontró el Rol a modificar.");
                        }

                        using (SqlCommand cmd = new SqlCommand(
                            "DELETE FROM Rol_Familia WHERE IdRol = @Id", conexion, tx))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = rol.IdRol;
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(
                            "DELETE FROM Rol_PermisoSimple WHERE IdRol = @Id", conexion, tx))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = rol.IdRol;
                            cmd.ExecuteNonQuery();
                        }

                        InsertarRelacionesDeRol(conexion, tx, rol.IdRol, rol);

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Desactivar(int idRol)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"UPDATE Rol SET Activo = 0 WHERE IdRol = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = idRol;
                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    if (filas == 0) throw new Exception("No se encontró el Rol a desactivar.");
                }
            }
        }

        public void AsignarRolAUsuario(int idUsuario, int idRol)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"IF NOT EXISTS (SELECT 1 FROM Usuario_Rol
                                                WHERE IdUsuario = @IdUsuario AND IdRol = @IdRol)
                                 INSERT INTO Usuario_Rol (IdUsuario, IdRol)
                                 VALUES (@IdUsuario, @IdRol)";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void QuitarRolDeUsuario(int idUsuario, int idRol)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"DELETE FROM Usuario_Rol
                                 WHERE IdUsuario = @IdUsuario AND IdRol = @IdRol";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
                    conexion.Open();
                    cmd.ExecuteNonQuery();
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

        private Dictionary<int, Rol> LeerTodosRolesBase(SqlConnection conexion, bool incluirInactivos)
        {
            Dictionary<int, Rol> dict = new Dictionary<int, Rol>();

            string query = @"SELECT IdRol, Nombre, Activo FROM Rol";
            if (!incluirInactivos) query += " WHERE Activo = 1";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Rol r = new Rol
                    {
                        IdRol = Convert.ToInt32(reader["IdRol"]),
                        Nombre = reader["Nombre"].ToString(),
                        Activo = Convert.ToBoolean(reader["Activo"])
                    };
                    dict[r.IdRol] = r;
                }
            }

            return dict;
        }

        private void ResolverFamiliasDeRoles(SqlConnection conexion,Dictionary<int, Rol> roles,Dictionary<int, Familia> familias)
        {
            string query = @"SELECT IdRol, IdFamilia FROM Rol_Familia";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idRol = Convert.ToInt32(reader["IdRol"]);
                    int idFam = Convert.ToInt32(reader["IdFamilia"]);

                    if (roles.ContainsKey(idRol) && familias.ContainsKey(idFam))
                    {
                        roles[idRol].Agregar(familias[idFam]);
                    }
                }
            }
        }

        private void ResolverPermisosDeRoles(SqlConnection conexion,Dictionary<int, Rol> roles,Dictionary<int, PermisoSimple> permisos)
        {
            string query = @"SELECT IdRol, IdPermisoSimple FROM Rol_PermisoSimple";

            using (SqlCommand cmd = new SqlCommand(query, conexion))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idRol = Convert.ToInt32(reader["IdRol"]);
                    int idPs = Convert.ToInt32(reader["IdPermisoSimple"]);

                    if (roles.ContainsKey(idRol) && permisos.ContainsKey(idPs))
                    {
                        roles[idRol].Agregar(permisos[idPs]);
                    }
                }
            }
        }

        private int InsertarRolBase(SqlConnection conexion, SqlTransaction tx, Rol rol)
        {
            string query = @"INSERT INTO Rol (Nombre, Activo)
                             OUTPUT INSERTED.IdRol
                             VALUES (@Nombre, @Activo)";

            using (SqlCommand cmd = new SqlCommand(query, conexion, tx))
            {
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = rol.Nombre;
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = rol.Activo;
                return (int)cmd.ExecuteScalar();
            }
        }

        private void InsertarRelacionesDeRol(SqlConnection conexion,SqlTransaction tx,int idRol,Rol rol)
        {
            foreach (Componente hijo in rol.Hijos)
            {
                if (hijo is PermisoSimple ps)
                {
                    string sql = @"INSERT INTO Rol_PermisoSimple (IdRol, IdPermisoSimple)
                                   VALUES (@IdRol, @IdPs)";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion, tx))
                    {
                        cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
                        cmd.Parameters.Add("@IdPs", SqlDbType.Int).Value = ps.IdPermisoSimple;
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (hijo is Familia famHija)
                {
                    string sql = @"INSERT INTO Rol_Familia (IdRol, IdFamilia)
                                   VALUES (@IdRol, @IdFam)";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion, tx))
                    {
                        cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
                        cmd.Parameters.Add("@IdFam", SqlDbType.Int).Value = famHija.IdFamilia;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion
    }
}
