using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class BitacoraEvento
    {
        public int IdEvento { get; set; }

        public int IdUsuario { get; set; }

        public string Usuario { get; set; }

        public DateTime FechaHora { get; set; }

        public string Modulo { get; set; }

        public string Accion { get; set; }

        public string Criticidad { get; set; }

        public string Resultado { get; set; }

        public string Descripcion { get; set; }

        public string Fecha
        {
            get { return FechaHora.ToString("dd/MM/yyyy"); }
        }

        public string Hora
        {
            get { return FechaHora.ToString("HH:mm:ss"); }
        }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
