using System;
using System.Collections.Generic;

namespace Servicios
{
    /// Composite "raiz" del modelo RBAC. Contiene PermisoSimples y Familias.
    /// Misma forma estructural que Familia pero conceptualmente es lo que se asigna a un Usuario.
    public class Rol : Componente
    {
        public int IdRol { get; set; }
        public List<Componente> Hijos { get; private set; }

        public Rol()
        {
            Hijos = new List<Componente>();
        }

        // Agrega un componente hijo al rol.
        public void Agregar(Componente componente)
        {
            if (componente == null) return;
            if (!Hijos.Contains(componente)) Hijos.Add(componente);
        }

        // Quita un componente hijo del rol.
        public void Quitar(Componente componente)
        {
            if (componente == null) return;
            Hijos.Remove(componente);
        }

 
        public void LimpiarHijos()
        {
            Hijos.Clear();
        }

        // Obtiene todos los permisos de este rol y sus hijos.
        public override List<int> ObtenerPermisos()
        {
            List<int> resultado = new List<int>();

            foreach (Componente hijo in Hijos)
            {
                resultado.AddRange(hijo.ObtenerPermisos());
            }

            return resultado;
        }

        // Verifica si el componente actual o alguno de sus hijos tiene el permiso especificado.
        public override bool TienePermiso(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return false;

            foreach (Componente hijo in Hijos)
            {
                if (hijo.TienePermiso(codigo)) return true;
            }

            return false;
        }

        // Verifica si el componente actual contiene al otro componente en su jerarquía.
        public override bool Contiene(Componente otro)
        {
            if (otro == null) return false;
            if (this == otro) return true;

            foreach (Componente hijo in Hijos)
            {
                if (hijo == otro) return true;
                if (hijo.Contiene(otro)) return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "[Rol] " + Nombre;
        }
    }
}
