using System;
using System.Collections.Generic;

namespace Servicios
{
    //clase abstracta que representa un componente en el patrón Composite
    public abstract class Componente 
    {
        public string Nombre { get; set; }

   
        public abstract List<int> ObtenerPermisos();

        public abstract bool TienePermiso(string codigo);

        
        public virtual bool Contiene(Componente otro)
        {
            if (otro == null) return false;
            return this == otro;
        }

        public override string ToString()
        {
            return Nombre ?? string.Empty;
        }
    }
}
