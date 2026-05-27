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
    }
}
