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
    }
}
