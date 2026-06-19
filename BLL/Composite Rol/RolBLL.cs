using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class RolBLL
    {
        private readonly RolDAL _rolDAL = new RolDAL();
        private readonly BitacoraEventoBLL _bitacoraEventoBLL = new BitacoraEventoBLL();

        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        // --- Lectura ---

        public List<Rol> ListarTodos(bool incluirInactivos = false) => _rolDAL.ListarTodos(incluirInactivos);

        public Rol ObtenerPorId(int idRol) => _rolDAL.ObtenerPorId(idRol);

        public List<Rol> ListarRolesDeUsuario(int idUsuario) => _rolDAL.ListarRolesDeUsuario(idUsuario);

        // --- Escritura ---

        public void Crear(Rol rol)
        {
            if (rol == null) throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            rol.Nombre = (rol.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(rol.Nombre, 0);
            ValidarComposicion(rol.Hijos);
            // El Rol no contiene Roles, no hay ciclos posibles.

            rol.IdRol = _rolDAL.InsertarConComposicion(rol);
            RegistrarBitacora("Crear Rol", "El administrador creó el Rol: " + rol.Nombre);
        }

        public void Modificar(Rol rol)
        {
            if (rol == null || rol.IdRol <= 0)
                throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            rol.Nombre = (rol.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(rol.Nombre, rol.IdRol);
            ValidarComposicion(rol.Hijos);

            _rolDAL.ModificarConComposicion(rol);
            RegistrarBitacora("Modificar Rol", "El administrador modificó el Rol: " + rol.Nombre);
        }

        public void Desactivar(int idRol)
        {
            Rol r = _rolDAL.ObtenerPorId(idRol)
                ?? throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            _rolDAL.Desactivar(idRol);
            RegistrarBitacora("Desactivar Rol", "El administrador desactivó el Rol: " + r.Nombre);
        }

        // --- Validaciones ---

        private void ValidarNombreYUnicidad(string nombre, int idExcluir)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception(T("Errores.RBAC.NombreObligatorio"));

            if (_rolDAL.ExistePorNombre(nombre, idExcluir))
                throw new Exception(T("Errores.RBAC.NombreRolYaExiste"));
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

        // Devuelve todos los códigos de PermisoSimple alcanzables desde un componente.
        private static IEnumerable<string> CodigosDe(Componente c)
        {
            if (c is PermisoSimple ps) return new[] { ps.Codigo };
            if (c is Familia f && f.Activo) return f.Hijos.SelectMany(CodigosDe);
            if (c is Rol r && r.Activo) return r.Hijos.SelectMany(CodigosDe);
            return Enumerable.Empty<string>();
        }

        private void RegistrarBitacora(string accion, string descripcion)
        {
            Usuario u = SM.Instancia.UsuarioActual;
            if (u == null) return;
            _bitacoraEventoBLL.Registrar(u.IdUsuario, u.NombreUsuario,
                "Administrador", accion, "Alta", "Exitoso", descripcion);
        }
    }
}
