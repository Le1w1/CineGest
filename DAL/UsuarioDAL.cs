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
    public class UsuarioDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public UsuarioDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        public Usuario BuscarPorEmail(string email)
        {
            Usuario usuario = null;

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
               string query = @"
                    SELECT IdUsuario, Nombre, Apellido, DNI, Email, NombreUsuario, PasswordHash,
                    Activo, Bloqueado, IntentosFallidos, DebeCambiarClave 
                    FROM Usuario WHERE Email = @Email";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = email;
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = (int)reader["IdUsuario"],
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Email = reader["Email"].ToString(),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Activo = (bool)reader["Activo"],
                                Bloqueado = (bool)reader["Bloqueado"],
                                IntentosFallidos = (int)reader["IntentosFallidos"],
                                DebeCambiarClave = (bool)reader["DebeCambiarClave"]
                            };
                        }
                    }
                }
                return usuario;
            }
        }
        public void IncrementarIntentosFallidos(int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @" UPDATE Usuario SET IntentosFallidos = IntentosFallidos + 1 WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Bloquear(int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"UPDATE Usuario SET Bloqueado = 1 WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ReiniciarIntentosFallidos(int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"UPDATE Usuario SET IntentosFallidos = 0 WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
