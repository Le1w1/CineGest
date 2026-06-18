using System;
using System.Collections.Generic;

namespace Servicios
{
    
    public class PermisoSimple : Componente
    {
        public int IdPermisoSimple { get; set; }
        public string Codigo { get; set; }

        public override List<int> ObtenerPermisos()
        {
            return new List<int> { IdPermisoSimple };
        }

        public override bool TienePermiso(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return false;
            return string.Equals(Codigo, codigo, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return "[Permiso] " + Codigo + " - " + Nombre;
        }
    }
}
