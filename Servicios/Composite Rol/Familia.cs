using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicios
{
    /// <summary>
    /// Composite recursivo. Puede contener PermisoSimples y otras Familias.
    /// </summary>
    public class Familia : Componente
    {
        public int IdFamilia { get; set; }
        public List<Componente> Hijos { get; private set; }

        public Familia()
        {
            Hijos = new List<Componente>();
        }

        /// <summary>
        /// Agrega un Componente como hijo. NO valida ciclos ni duplicados:
        /// esa logica vive en la BLL para mantener la entidad limpia.
        /// </summary>
        public void Agregar(Componente componente)
        {
            if (componente == null) return;
            if (!Hijos.Contains(componente)) Hijos.Add(componente);
        }

        public void Quitar(Componente componente)
        {
            if (componente == null) return;
            Hijos.Remove(componente);
        }

        public void LimpiarHijos()
        {
            Hijos.Clear();
        }

        public override List<int> ObtenerPermisos()
        {
            // NO deduplicamos a proposito: la BLL valida duplicados con
            // mensaje especifico ("el permiso X ya esta incluido via Familia Y").
            // Si el caller solo quiere el set efectivo, hace .Distinct() afuera.
            List<int> resultado = new List<int>();

            foreach (Componente hijo in Hijos)
            {
                resultado.AddRange(hijo.ObtenerPermisos());
            }

            return resultado;
        }

        public override bool TienePermiso(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return false;

            foreach (Componente hijo in Hijos)
            {
                if (hijo.TienePermiso(codigo)) return true;
            }

            return false;
        }

        /// <summary>
        /// Determina si esta Familia contiene (directa o indirectamente) al
        /// componente indicado. Recorre todo el subarbol.
        /// Lo usa la BLL para detectar ciclos antes de agregar un hijo.
        /// </summary>
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
            return "[Familia] " + Nombre;
        }
    }
}
