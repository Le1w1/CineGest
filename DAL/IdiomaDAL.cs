using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class IdiomaDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public IdiomaDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        /// Lista todos los idiomas disponibles en la base de datos.
        public List<Idioma> ListarIdiomas()
        {
            List<Idioma> idiomas = new List<Idioma>();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT IdIdioma, Codigo, Nombre FROM Idioma ORDER BY Nombre";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idiomas.Add(new Idioma
                            {
                                IdIdioma = Convert.ToInt32(reader["IdIdioma"]),
                                Codigo = reader["Codigo"].ToString(),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
            }

            return idiomas;
        }

       
        /// Obtiene un idioma por su ID.
        public Idioma ObtenerPorId(int idIdioma)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT IdIdioma, Codigo, Nombre FROM Idioma WHERE IdIdioma = @IdIdioma";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@IdIdioma", SqlDbType.Int).Value = idIdioma;

                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Idioma
                            {
                                IdIdioma = Convert.ToInt32(reader["IdIdioma"]),
                                Codigo = reader["Codigo"].ToString(),
                                Nombre = reader["Nombre"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

      
        /// Obtiene un idioma por su código.
        public Idioma ObtenerPorCodigo(string codigo)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT IdIdioma, Codigo, Nombre FROM Idioma WHERE Codigo = @Codigo";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Codigo", SqlDbType.NVarChar, 10).Value = codigo;

                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Idioma
                            {
                                IdIdioma = Convert.ToInt32(reader["IdIdioma"]),
                                Codigo = reader["Codigo"].ToString(),
                                Nombre = reader["Nombre"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
