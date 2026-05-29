using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraEventoBLL
    {
        private readonly BitacoraEventoDAL _bitacoraEventoDAL;

        public BitacoraEventoBLL()
        {
            _bitacoraEventoDAL = new BitacoraEventoDAL();
        }

        public void Registrar(int idUsuario, string usuario, string modulo, string accion, string criticidad, string resultado, string descripcion)
        {
            BitacoraEvento evento = new BitacoraEvento
            {
                IdUsuario = idUsuario,
                Usuario = usuario,
                FechaHora = DateTime.Now,
                Modulo = modulo,
                Accion = accion,
                Criticidad = criticidad,
                Resultado = resultado,
                Descripcion = descripcion
            };

            _bitacoraEventoDAL.Registrar(evento);
        }
    }
}
