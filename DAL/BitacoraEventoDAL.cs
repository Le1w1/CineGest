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
                    
                    else{
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
        public List<BitacoraEvento> ObtenerEventos(DateTime fechaDesde,DateTime fechaHasta,string usuario,string modulo,string criticidad,string resultado)
        {
            List<BitacoraEvento> eventos = new List<BitacoraEvento>();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            {
                string consulta = @"
              SELECT IdEvento,IdUsuario,Usuario,FechaHora,Modulo,Accion,Criticidad,Resultado,Descripcion
              FROM BitacoraEvento
              WHERE FechaHora >= @FechaDesde
              AND FechaHora < @FechaHasta
              AND (@Usuario = '' OR Usuario LIKE '%' + @Usuario + '%')
              AND (@Modulo = 'Todos' OR Modulo = @Modulo)
              AND (@Criticidad = 'Todas' OR Criticidad = @Criticidad)
              AND (@Resultado = 'Todos' OR Resultado = @Resultado)
              ORDER BY FechaHora DESC";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@FechaDesde", SqlDbType.DateTime2).Value = fechaDesde;
                    comando.Parameters.Add("@FechaHasta", SqlDbType.DateTime2).Value = fechaHasta;
                    comando.Parameters.Add("@Usuario", SqlDbType.NVarChar, 150).Value = usuario;
                    comando.Parameters.Add("@Modulo", SqlDbType.NVarChar, 100).Value = modulo;
                    comando.Parameters.Add("@Criticidad", SqlDbType.NVarChar, 50).Value = criticidad;
                    comando.Parameters.Add("@Resultado", SqlDbType.NVarChar, 50).Value = resultado;

                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BitacoraEvento evento = new BitacoraEvento
                            {
                                IdEvento = Convert.ToInt32(reader["IdEvento"]),
                                IdUsuario = reader["IdUsuario"] == DBNull.Value? 0 : Convert.ToInt32(reader["IdUsuario"]),
                                Usuario = reader["Usuario"].ToString(),
                                FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                                Modulo = reader["Modulo"].ToString(),
                                Accion = reader["Accion"].ToString(),
                                Criticidad = reader["Criticidad"].ToString(),
                                Resultado = reader["Resultado"].ToString(),
                                Descripcion = reader["Descripcion"].ToString()
                            };
                            eventos.Add(evento);
                        }
                    }
                }
            }

            return eventos;
        }
    }
}
