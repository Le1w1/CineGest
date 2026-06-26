using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;

namespace DAL
{
    public class PermisoSimpleDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public PermisoSimpleDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        // ListarTodos: Obtiene todos los permisos simples de la base de datos.
        public List<PermisoSimple> ListarTodos()
        {
            List<PermisoSimple> permisos = new List<PermisoSimple>();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT IdPermisoSimple, Codigo, Nombre
                                 FROM PermisoSimple
                                 ORDER BY Codigo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permisos.Add(MapearPermisoSimple(reader));
                        }
                    }
                }
            }

            return permisos;
        }


        // ObtenerPorId: Obtiene un permiso simple por su ID.
        public PermisoSimple ObtenerPorId(int idPermisoSimple)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT IdPermisoSimple, Codigo, Nombre
                                 FROM PermisoSimple
                                 WHERE IdPermisoSimple = @Id";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Id", SqlDbType.Int).Value = idPermisoSimple;
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read()) return MapearPermisoSimple(reader);
                    }
                }
            }

            return null;
        }


        // MapearPermisoSimple: Mapea un registro de la base de datos a un objeto PermisoSimple.
        private PermisoSimple MapearPermisoSimple(SqlDataReader reader)
        {
            return new PermisoSimple
            {
                IdPermisoSimple = Convert.ToInt32(reader["IdPermisoSimple"]),
                Codigo = reader["Codigo"].ToString(),
                Nombre = reader["Nombre"].ToString()
            };
        }
    }
}
