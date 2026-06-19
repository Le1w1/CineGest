using Microsoft.Data.SqlClient;
using Servicios;
using System.Data;


namespace DAL
{
    public class UsuarioDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        // Lista de columnas reutilizada por todos los SELECT.
        // Si en el futuro se agrega un campo, se cambia aca y en MapearUsuario.
        private const string COLUMNAS_USUARIO ="IdUsuario, Nombre, Apellido, DNI, Email, NombreUsuario, PasswordHash, Activo, Bloqueado, IntentosFallidos, DebeCambiarClave, IdIdioma, IdRol";

        public UsuarioDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        /// Mapea una fila del SqlDataReader a un objeto Usuario.
        /// Centralizado para evitar duplicar el mapeo en cada SELECT.
        private Usuario MapearUsuario(SqlDataReader reader)
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
                DebeCambiarClave = Convert.ToBoolean(reader["DebeCambiarClave"]),
                IdIdioma = Convert.ToInt32(reader["IdIdioma"]),
                IdRol = Convert.ToInt32(reader["IdRol"])
            };
        }

        public Usuario BuscarPorEmail(string email)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = "SELECT " + COLUMNAS_USUARIO + " FROM Usuario WHERE Email = @Email";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = email;
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapearUsuario(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Usuario BuscarPorId(int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = "SELECT " + COLUMNAS_USUARIO + " FROM Usuario WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapearUsuario(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Usuario BuscarPorNombreUsuario(string nombreUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = "SELECT " + COLUMNAS_USUARIO + " FROM Usuario WHERE NombreUsuario = @NombreUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapearUsuario(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Usuario> ListarUsuarios(string filtro)
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = "SELECT " + COLUMNAS_USUARIO + " FROM Usuario";

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
                            usuarios.Add(MapearUsuario(reader));
                        }
                    }
                }
            }

            return usuarios;
        }

        public int Insertar(Usuario usuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
            INSERT INTO Usuario
            (
                Nombre,Apellido,DNI,Email,NombreUsuario,PasswordHash,Activo,Bloqueado,IntentosFallidos,DebeCambiarClave,IdIdioma,IdRol
            )
            OUTPUT INSERTED.IdUsuario 
            VALUES
            (
                @Nombre,@Apellido,@DNI,@Email,@NombreUsuario,@PasswordHash,@Activo,@Bloqueado,@IntentosFallidos,@DebeCambiarClave,@IdIdioma,@IdRol
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
                    comando.Parameters.AddWithValue("@IdIdioma", usuario.IdIdioma);
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    conexion.Open();
                    return (int)comando.ExecuteScalar();
                }
            }
        }

        public void Modificar(Usuario usuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
                UPDATE Usuario SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, Email = @Email, NombreUsuario = @NombreUsuario,Activo = @Activo, Bloqueado = @Bloqueado, IdRol = @IdRol
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
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    conexion.Open();

                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0) throw new Exception("No se encontró el usuario a modificar.");
                }
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

        public void DesbloquearUsuario(int idUsuario)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
                UPDATE Usuario
                SET Bloqueado = 0,IntentosFallidos = 0
                WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0) throw new Exception("No se encontró el usuario a desbloquear.");
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

        public void ActualizarPassword(int idUsuario, string nuevoPasswordHash)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
                                 UPDATE Usuario
                                 SET PasswordHash = @PasswordHash, DebeCambiarClave = 0
                                 WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@PasswordHash", SqlDbType.Char, 64).Value = nuevoPasswordHash;
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        /// Persiste el idioma del usuario. Lo invoca el Logout si SM.RequierePersistirIdioma() == true.
        public void ActualizarIdioma(int idUsuario, int idIdioma)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"UPDATE Usuario SET IdIdioma = @IdIdioma WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.Add("@IdIdioma", SqlDbType.Int).Value = idIdioma;
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas == 0) throw new Exception("No se encontró el usuario para actualizar el idioma.");
                }
            }
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

        #region "Validaciones de existencia en otros usuarios"
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
        #endregion

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
    }
}
