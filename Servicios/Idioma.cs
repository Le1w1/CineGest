using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Idioma
    {
        public int IdIdioma { get; set; }
        public string Codigo { get; set; }   
        public string Nombre { get; set; }   

        public override string ToString()
        {
            return Nombre;
        }
    }
}
