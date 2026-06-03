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
        
        public List<BitacoraEvento> ObtenerEventos(DateTime fechaDesde,
    DateTime fechaHasta,
    string usuario,
    string modulo,
    string accion,
    string criticidad,
    string resultado,
    string descripcion)
        {
            if (fechaDesde.Date > fechaHasta.Date)
            {
                throw new Exception("La fecha desde no puede ser mayor a la fecha hasta.");
            }

            usuario = (usuario ?? string.Empty).Trim();
            modulo = (modulo ?? string.Empty).Trim();
            accion = (accion ?? string.Empty).Trim();
            criticidad = (criticidad ?? string.Empty).Trim();
            resultado = (resultado ?? string.Empty).Trim();
            descripcion = (descripcion ?? string.Empty).Trim();

            return _bitacoraEventoDAL.ObtenerEventos(
                fechaDesde,
                fechaHasta,
                usuario,
                modulo,
                accion,
                criticidad,
                resultado,
                descripcion);
        }



    }
}
