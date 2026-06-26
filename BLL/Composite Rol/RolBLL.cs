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

        
        public List<Rol> ListarTodos() => _rolDAL.ListarTodos();

        public Rol ObtenerPorId(int idRol) => _rolDAL.ObtenerPorId(idRol);

        
        public List<Rol> ListarRolesDeUsuario(int idUsuario) => _rolDAL.ListarRolesDeUsuario(idUsuario);


        //Crear Rol
        public void Crear(Rol rol)
        {
            // Defensa en profundidad: la UI ya deshabilita la acción, la BLL valida igual.
            SM.Instancia.RequierePermiso("ROL_GESTIONAR");

            if (rol == null) throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            rol.Nombre = (rol.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(rol.Nombre, 0);
            ValidarComposicion(rol.Hijos);
            rol.IdRol = _rolDAL.InsertarConComposicion(rol);
            RegistrarBitacora("Crear Rol", "El administrador creó el Rol: " + rol.Nombre);
        }

        //Modificar Rol
        public void Modificar(Rol rol)
        {
            // Defensa en profundidad: la UI ya deshabilita la acción, la BLL valida igual.
            SM.Instancia.RequierePermiso("ROL_GESTIONAR");

            if (rol == null || rol.IdRol <= 0)
                throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            rol.Nombre = (rol.Nombre ?? "").Trim();

            ValidarNombreYUnicidad(rol.Nombre, rol.IdRol);
            ValidarComposicion(rol.Hijos);

            _rolDAL.ModificarConComposicion(rol);
            RegistrarBitacora("Modificar Rol", "El administrador modificó el Rol: " + rol.Nombre);
        }

        //Eliminar Rol
        public void Eliminar(int idRol)
        {
            // Defensa en profundidad: la UI ya deshabilita la acción, la BLL valida igual.
            SM.Instancia.RequierePermiso("ROL_GESTIONAR");

            Rol r = _rolDAL.ObtenerPorId(idRol)
                ?? throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            if (_rolDAL.EstaUsado(idRol))
                throw new Exception(T("Errores.RBAC.RolEnUso"));

            _rolDAL.Eliminar(idRol);
            RegistrarBitacora("Eliminar Rol", "El administrador eliminó el Rol: " + r.Nombre);
        }


        private void ValidarNombreYUnicidad(string nombre, int idExcluir)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception(T("Errores.RBAC.NombreObligatorio"));

            if (_rolDAL.ExistePorNombre(nombre, idExcluir))
                throw new Exception(T("Errores.RBAC.NombreRolYaExiste"));
        }


        // Valida que la composición de un rol sea correcta, es decir, que no esté vacía y que no haya permisos duplicados.
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
            if (c is Familia f) return f.Hijos.SelectMany(CodigosDe);
            if (c is Rol r) return r.Hijos.SelectMany(CodigosDe);
            return Enumerable.Empty<string>();
        }


        // Registra un evento en la bitácora de eventos del sistema.
        private void RegistrarBitacora(string accion, string descripcion)
        {
            Usuario u = SM.Instancia.UsuarioActual;
            if (u == null) return;
            _bitacoraEventoBLL.Registrar(u.IdUsuario, u.NombreUsuario,"Administrador", accion, "Alta", "Exitoso", descripcion);
        }
    }
}
