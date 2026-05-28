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
        public List<BitacoraEvento> ObtenerEventos(
    DateTime fechaDesde,
    DateTime fechaHasta,
    string usuario,
    string modulo,
    string criticidad,
    string resultado)
        {
            if (fechaDesde.Date > fechaHasta.Date)
            {
                throw new Exception("La fecha desde no puede ser mayor a la fecha hasta.");
            }

            DateTime fechaDesdeConsulta = fechaDesde.Date;
            DateTime fechaHastaConsulta = fechaHasta.Date.AddDays(1);

            if (usuario == null)
            {
                usuario = string.Empty;
            }

            usuario = usuario.Trim();

            return _bitacoraEventoDAL.ObtenerEventos(
                fechaDesdeConsulta,
                fechaHastaConsulta,
                usuario,
                modulo,
                criticidad,
                resultado);
        }
    }
}
