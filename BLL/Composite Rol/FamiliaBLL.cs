using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class FamiliaBLL
    {
        private readonly FamiliaDAL _familiaDAL = new FamiliaDAL();
        private readonly BitacoraEventoBLL _bitacoraEventoBLL = new BitacoraEventoBLL();

        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        // --- Lectura ---

        public List<Familia> ListarTodas() => _familiaDAL.ListarTodas();

        public Familia ObtenerPorId(int idFamilia) => _familiaDAL.ObtenerPorId(idFamilia);

        // --- Escritura ---

        public void Crear(Familia familia)
        {
            if (familia == null) throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            familia.Nombre = (familia.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(familia.Nombre, 0);
            ValidarComposicion(familia.Hijos);
            // Al crear no hay ciclos posibles: la Familia aun no existe en BD.

            familia.IdFamilia = _familiaDAL.InsertarConComposicion(familia);

            RegistrarBitacora("Crear Familia", "El administrador creó la Familia: " + familia.Nombre);
        }

        public void Modificar(Familia familia)
        {
            if (familia == null || familia.IdFamilia <= 0)
                throw new Exception(T("Errores.RBAC.FamiliaNoEncontrada"));

            familia.Nombre = (familia.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(familia.Nombre, familia.IdFamilia);
            ValidarComposicion(familia.Hijos);
            ValidarSinCiclos(familia);

            _familiaDAL.ModificarConComposicion(familia);

            RegistrarBitacora("Modificar Familia", "El administrador modificó la Familia: " + familia.Nombre);
        }

        public void Eliminar(int idFamilia)
        {
            Familia f = _familiaDAL.ObtenerPorId(idFamilia)
                ?? throw new Exception(T("Errores.RBAC.FamiliaNoEncontrada"));

            if (_familiaDAL.EstaUsada(idFamilia))
                throw new Exception(T("Errores.RBAC.FamiliaEnUso"));

            _familiaDAL.Eliminar(idFamilia);
            RegistrarBitacora("Eliminar Familia", "El administrador eliminó la Familia: " + f.Nombre);
        }

        // --- Validaciones ---

        private void ValidarNombreYUnicidad(string nombre, int idExcluir)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception(T("Errores.RBAC.NombreObligatorio"));

            if (_familiaDAL.ExistePorNombre(nombre, idExcluir))
                throw new Exception(T("Errores.RBAC.NombreFamiliaYaExiste"));
        }

        private void ValidarComposicion(List<Componente> hijos)
        {
            if (hijos == null || hijos.Count == 0)
                throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            // Detecta si dos hijos distintos aportan el mismo permiso (directa o indirectamente).
            var origenes = new Dictionary<string, string>();
            foreach (Componente hijo in hijos)
            {
                string origen = hijo.ToString();
                foreach (string codigo in CodigosDe(hijo))
                {
                    if (origenes.TryGetValue(codigo, out string previo))
                        throw new Exception(string.Format(T("Errores.RBAC.PermisoDuplicado"), codigo, previo, origen));
                    origenes[codigo] = origen;
                }
            }
        }

        private void ValidarSinCiclos(Familia familiaEnEdicion)
        {
            foreach (Familia hija in familiaEnEdicion.Hijos.OfType<Familia>())
            {
                if (hija.IdFamilia == familiaEnEdicion.IdFamilia)
                    throw new Exception(string.Format(T("Errores.RBAC.CicloDirecto"), familiaEnEdicion.Nombre));

                if (ContieneFamiliaConId(hija, familiaEnEdicion.IdFamilia))
                    throw new Exception(string.Format(T("Errores.RBAC.CicloIndirecto"), hija.Nombre, familiaEnEdicion.Nombre));
            }
        }

        // --- Helpers Composite ---

        // Devuelve todos los códigos de PermisoSimple alcanzables desde un componente.
        private static IEnumerable<string> CodigosDe(Componente c)
        {
            if (c is PermisoSimple ps) return new[] { ps.Codigo };
            if (c is Familia f) return f.Hijos.SelectMany(CodigosDe);
            if (c is Rol r) return r.Hijos.SelectMany(CodigosDe);
            return Enumerable.Empty<string>();
        }

        // Recorre el subárbol buscando una Familia con el Id indicado.
        private static bool ContieneFamiliaConId(Familia f, int idBuscar) =>
            f?.Hijos.OfType<Familia>()
                .Any(h => h.IdFamilia == idBuscar || ContieneFamiliaConId(h, idBuscar))
            ?? false;

        private void RegistrarBitacora(string accion, string descripcion)
        {
            Usuario u = SM.Instancia.UsuarioActual;
            if (u == null) return;
            _bitacoraEventoBLL.Registrar(u.IdUsuario, u.NombreUsuario,
                "Administrador", accion, "Alta", "Exitoso", descripcion);
        }
    }
}
