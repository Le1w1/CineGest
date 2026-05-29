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

        public void Insertar(Usuario usuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
            INSERT INTO Usuario
            (
                Nombre,Apellido,DNI,Email,NombreUsuario,PasswordHash,Activo,Bloqueado,IntentosFallidos,DebeCambiarClave
            )
            VALUES
            (
                @Nombre,@Apellido,@DNI,@Email,@NombreUsuario,@PasswordHash,@Activo,@Bloqueado,@IntentosFallidos,@DebeCambiarClave
            )";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@DNI", usuario.DNI);
                    comando.Parameters.AddWithValue("@Email", usuario.Email);
                    comando.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                    comando.Parameters.AddWithValue("@Activo", usuario.Activo);
                    comando.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);
                    comando.Parameters.AddWithValue("@IntentosFallidos", usuario.IntentosFallidos);
                    comando.Parameters.AddWithValue("@DebeCambiarClave", usuario.DebeCambiarClave);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public Usuario BuscarPorId(int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
            SELECT
                IdUsuario,
                Nombre,
                Apellido,
                DNI,
                Email,
                NombreUsuario,
                PasswordHash,
                Activo,
                Bloqueado,
                IntentosFallidos,
                DebeCambiarClave
            FROM Usuario
            WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Email = reader["Email"].ToString(),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                Bloqueado = Convert.ToBoolean(reader["Bloqueado"]),
                                IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]),
                                DebeCambiarClave = Convert.ToBoolean(reader["DebeCambiarClave"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void CambiarEstadoActivo(int idUsuario, bool activo)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
            UPDATE Usuario
            SET Activo = @Activo
            WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    comando.Parameters.AddWithValue("@Activo", activo);

                    conexion.Open();

                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0)
                    {
                        throw new Exception("No se encontró el usuario a activar o desactivar.");
                    }
                }
            }
        }

        public void Modificar(Usuario usuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
                UPDATE [Usuario] SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, Email = @Email, NombreUsuario = @NombreUsuario,Activo = @Activo, Bloqueado = @Bloqueado 
                WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@DNI", usuario.DNI);
                    comando.Parameters.AddWithValue("@Email", usuario.Email);
                    comando.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    comando.Parameters.AddWithValue("@Activo", usuario.Activo);
                    comando.Parameters.AddWithValue("@Bloqueado", usuario.Bloqueado);

                    conexion.Open();

                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0) throw new Exception("No se encontró el usuario a modificar.");
                    
                }
            }
        }

        public bool ExisteEmailEnOtroUsuario(string email, int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Usuario WHERE Email = @Email AND IdUsuario <> @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Email", email);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        public bool ExisteDNIEnOtroUsuario(string dni, int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Usuario WHERE DNI = @DNI AND IdUsuario <> @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@DNI", dni);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        public bool ExisteNombreUsuarioEnOtroUsuario(string nombreUsuario, int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Usuario WHERE NombreUsuario = @NombreUsuario AND IdUsuario <> @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        #region "Validaciones de existencia"
        public bool ExistePorEmail(string email)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Usuario WHERE Email = @Email";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Email", email);
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }

        }

        public bool ExistePorDNI(string dni)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Usuario WHERE DNI = @DNI";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@DNI", dni);
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        public bool ExistePorNombreUsuario(string nombreUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"SELECT COUNT(1) FROM Usuario WHERE NombreUsuario = @NombreUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    conexion.Open();
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        #endregion

        public List<Usuario> ListarUsuarios(string filtro)
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
                SELECT IdUsuario,Nombre,Apellido,DNI,Email,NombreUsuario,PasswordHash,Activo,Bloqueado,IntentosFallidos,DebeCambiarClave
                FROM Usuario";

                if (filtro == "ACTIVOS")
                {
                    query += " WHERE Activo = 1 AND Bloqueado = 0";
                }
                else if (filtro == "BLOQUEADOS")
                {
                    query += " WHERE Bloqueado = 1";
                }

                query += " ORDER BY Apellido, Nombre";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario usuario = new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Email = reader["Email"].ToString(),
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                Bloqueado = Convert.ToBoolean(reader["Bloqueado"]),
                                IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]),
                                DebeCambiarClave = Convert.ToBoolean(reader["DebeCambiarClave"])
                            };

                            usuarios.Add(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }
    }
}
