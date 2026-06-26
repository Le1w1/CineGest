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

        //Traductor de mensajes de error
        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        //Lista todas las Familias, sin sus hijos (para mostrar en grilla).
        public List<Familia> ListarTodas() => _familiaDAL.ListarTodas();

   
        //Obtiene una Familia con todos sus hijos (para editar).
        public Familia ObtenerPorId(int idFamilia) => _familiaDAL.ObtenerPorId(idFamilia);


        //Crea una nueva Familia con su composición de hijos, validando que no haya ciclos ni permisos duplicados.
        public void Crear(Familia familia)
        {
            // Defensa en profundidad: la UI ya deshabilita la acción, la BLL valida igual.
            SM.Instancia.RequierePermiso("FAM_GESTIONAR");

            if (familia == null) throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            familia.Nombre = (familia.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(familia.Nombre, 0);
            ValidarComposicion(familia.Hijos);
            familia.IdFamilia = _familiaDAL.InsertarConComposicion(familia);

            RegistrarBitacora("Crear Familia", "El administrador creó la Familia: " + familia.Nombre);
        }


        //Modifica una Familia existente con su composición de hijos, validando que no haya ciclos ni permisos duplicados.
        public void Modificar(Familia familia)
        {
            // Defensa en profundidad: la UI ya deshabilita la acción, la BLL valida igual.
            SM.Instancia.RequierePermiso("FAM_GESTIONAR");

            if (familia == null || familia.IdFamilia <= 0)
                throw new Exception(T("Errores.RBAC.FamiliaNoEncontrada"));

            familia.Nombre = (familia.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(familia.Nombre, familia.IdFamilia);
            ValidarComposicion(familia.Hijos);
            ValidarSinCiclos(familia);

            _familiaDAL.ModificarConComposicion(familia);

            RegistrarBitacora("Modificar Familia", "El administrador modificó la Familia: " + familia.Nombre);
        }


        // Elimina una Familia existente, validando que no esté en uso por ningún Rol ni Usuario.
        public void Eliminar(int idFamilia)
        {
            // Defensa en profundidad: la UI ya deshabilita la acción, la BLL valida igual.
            SM.Instancia.RequierePermiso("FAM_GESTIONAR");

            Familia f = _familiaDAL.ObtenerPorId(idFamilia)?? throw new Exception(T("Errores.RBAC.FamiliaNoEncontrada"));

            if (_familiaDAL.EstaUsada(idFamilia))
                throw new Exception(T("Errores.RBAC.FamiliaEnUso"));

            _familiaDAL.Eliminar(idFamilia);
            RegistrarBitacora("Eliminar Familia", "El administrador eliminó la Familia: " + f.Nombre);
        }


        //Valida que el nombre no esté vacío y que no exista otra Familia con el mismo nombre (excluyendo la propia en caso de edición).
        private void ValidarNombreYUnicidad(string nombre, int idExcluir)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception(T("Errores.RBAC.NombreObligatorio"));

            if (_familiaDAL.ExistePorNombre(nombre, idExcluir))
                throw new Exception(T("Errores.RBAC.NombreFamiliaYaExiste"));
        }


        //Valida que la composición de hijos no esté vacía y que no haya permisos duplicados (directa o indirectamente).
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


        //Valida que la Familia no contenga a sí misma (directa o indirectamente) en su composición de hijos.
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


        // Devuelve todos los códigos de permisos que aporta un Componente (directa o indirectamente).
        private static IEnumerable<string> CodigosDe(Componente c)
        {
            if (c is PermisoSimple ps) return new[] { ps.Codigo };
            if (c is Familia f) return f.Hijos.SelectMany(CodigosDe);
            if (c is Rol r) return r.Hijos.SelectMany(CodigosDe);
            return Enumerable.Empty<string>();
        }


        // Devuelve true si la Familia f contiene (directa o indirectamente) a una Familia con el Id especificado.
        private static bool ContieneFamiliaConId(Familia f, int idBuscar) =>f?.Hijos.OfType<Familia>().Any(h => h.IdFamilia == idBuscar || ContieneFamiliaConId(h, idBuscar))?? false;


        // Registra un evento en la bitácora de auditoría.
        private void RegistrarBitacora(string accion, string descripcion)
        {
            Usuario u = SM.Instancia.UsuarioActual;
            if (u == null) return;
            _bitacoraEventoBLL.Registrar(u.IdUsuario, u.NombreUsuario,"Administrador", accion, "Alta", "Exitoso", descripcion);
        }
    }
}
