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
    public class BitacoraEventoDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public BitacoraEventoDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        public void Registrar(BitacoraEvento evento)
        {
            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
                    INSERT INTO BitacoraEvento(IdUsuario,Usuario,FechaHora,Modulo,Accion,Criticidad,Resultado,Descripcion)
                    VALUES(@IdUsuario,@Usuario,@FechaHora,@Modulo,@Accion,@Criticidad,@Resultado,@Descripcion)";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (evento.IdUsuario <= 0) comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = DBNull.Value;

                    else
                    {
                        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = evento.IdUsuario;
                    }

                    comando.Parameters.Add("@Usuario", SqlDbType.NVarChar, 150).Value = evento.Usuario;
                    comando.Parameters.Add("@FechaHora", SqlDbType.DateTime2).Value = evento.FechaHora;
                    comando.Parameters.Add("@Modulo", SqlDbType.NVarChar, 100).Value = evento.Modulo;
                    comando.Parameters.Add("@Accion", SqlDbType.NVarChar, 150).Value = evento.Accion;
                    comando.Parameters.Add("@Criticidad", SqlDbType.NVarChar, 50).Value = evento.Criticidad;
                    comando.Parameters.Add("@Resultado", SqlDbType.NVarChar, 50).Value = evento.Resultado;
                    comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = evento.Descripcion;

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
        public List<BitacoraEvento> ObtenerEventos(DateTime fechaDesde,
    DateTime fechaHasta,
    string usuario,
    string modulo,
    string accion,
    string criticidad,
    string resultado,
    string descripcion)
        {
            List<BitacoraEvento> eventos = new List<BitacoraEvento>();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string query = @"
            SELECT
                B.IdEvento,
                B.IdUsuario,
                B.Usuario,
                B.FechaHora,
                B.Modulo,
                B.Accion,
                B.Criticidad,
                B.Resultado,
                B.Descripcion,
                ISNULL(U.Nombre, '') AS Nombre,
                ISNULL(U.Apellido, '') AS Apellido
            FROM BitacoraEvento B
            OUTER APPLY
            (
                SELECT TOP 1
                    U.Nombre,
                    U.Apellido
                FROM Usuario U
                WHERE 
                    U.IdUsuario = B.IdUsuario
                    OR U.NombreUsuario = B.Usuario
                ORDER BY 
                    CASE 
                        WHEN U.IdUsuario = B.IdUsuario THEN 0
                        ELSE 1
                    END
            ) U
            WHERE B.FechaHora >= @FechaDesde
            AND B.FechaHora < @FechaHasta";

                using (SqlCommand comando = new SqlCommand())
                {
                    comando.Connection = conexion;

                    comando.Parameters.AddWithValue("@FechaDesde", fechaDesde.Date);
                    comando.Parameters.AddWithValue("@FechaHasta", fechaHasta.Date.AddDays(1));

                    if (!string.IsNullOrWhiteSpace(usuario))
                    {
                        query += " AND B.Usuario LIKE @Usuario";
                        comando.Parameters.AddWithValue("@Usuario", "%" + usuario.Trim() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(modulo) && modulo != "Todos")
                    {
                        query += " AND B.Modulo = @Modulo";
                        comando.Parameters.AddWithValue("@Modulo", modulo);
                    }

                    if (!string.IsNullOrWhiteSpace(accion) && accion != "Todos")
                    {
                        query += " AND B.Accion = @Accion";
                        comando.Parameters.AddWithValue("@Accion", accion);
                    }

                    if (!string.IsNullOrWhiteSpace(criticidad) && criticidad != "Todas")
                    {
                        query += " AND B.Criticidad = @Criticidad";
                        comando.Parameters.AddWithValue("@Criticidad", criticidad);
                    }

                    if (!string.IsNullOrWhiteSpace(resultado) && resultado != "Todos")
                    {
                        query += " AND B.Resultado = @Resultado";
                        comando.Parameters.AddWithValue("@Resultado", resultado);
                    }

                    if (!string.IsNullOrWhiteSpace(descripcion))
                    {
                        query += " AND B.Descripcion LIKE @Descripcion";
                        comando.Parameters.AddWithValue("@Descripcion", "%" + descripcion.Trim() + "%");
                    }

                    query += " ORDER BY B.FechaHora DESC";

                    comando.CommandText = query;

                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eventos.Add(new BitacoraEvento
                            {
                                IdEvento = Convert.ToInt32(reader["IdEvento"]),
                                IdUsuario = reader["IdUsuario"] == DBNull.Value ? 0 : Convert.ToInt32(reader["IdUsuario"]),
                                Usuario = reader["Usuario"].ToString(),
                                FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                                Modulo = reader["Modulo"].ToString(),
                                Accion = reader["Accion"].ToString(),
                                Criticidad = reader["Criticidad"].ToString(),
                                Resultado = reader["Resultado"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString()
                            });
                        }
                    }
                }
            }

            return eventos;
        }
    }
}