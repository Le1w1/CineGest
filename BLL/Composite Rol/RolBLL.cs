using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class RolBLL
    {
        private readonly RolDAL _rolDAL;
        private readonly UsuarioDAL _usuarioDAL;
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;

        public RolBLL()
        {
            _rolDAL = new RolDAL();
            _usuarioDAL = new UsuarioDAL();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
        }

        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        #region "Lectura"

        public List<Rol> ListarTodos(bool incluirInactivos = false)
        {
            return _rolDAL.ListarTodos(incluirInactivos);
        }

        public Rol ObtenerPorId(int idRol)
        {
            return _rolDAL.ObtenerPorId(idRol);
        }

        public List<Rol> ListarRolesDeUsuario(int idUsuario)
        {
            return _rolDAL.ListarRolesDeUsuario(idUsuario);
        }

        #endregion

        #region "Escritura"

        public void Crear(Rol rol)
        {
            if (rol == null) throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            string nombre = (rol.Nombre ?? string.Empty).Trim();
            rol.Nombre = nombre;

            ValidarNombre(nombre);
            ValidarUnicidadNombre(nombre, 0);
            ValidarComposicionNoVacia(rol.Hijos);
            ValidarSinDuplicados(rol.Hijos);
            // Un Rol NO contiene Roles, asi que no hay ciclos posibles.

            int nuevoId = _rolDAL.InsertarConComposicion(rol);
            rol.IdRol = nuevoId;

            RegistrarBitacora("Crear Rol", "Exitoso","El administrador creó el Rol: " + nombre);
        }

        public void Modificar(Rol rol)
        {
            if (rol == null || rol.IdRol <= 0)
                throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            string nombre = (rol.Nombre ?? string.Empty).Trim();
            rol.Nombre = nombre;

            ValidarNombre(nombre);
            ValidarUnicidadNombre(nombre, rol.IdRol);
            ValidarComposicionNoVacia(rol.Hijos);
            ValidarSinDuplicados(rol.Hijos);

            _rolDAL.ModificarConComposicion(rol);

            RegistrarBitacora("Modificar Rol", "Exitoso","El administrador modificó el Rol: " + nombre);
        }

        public void Desactivar(int idRol)
        {
            Rol r = _rolDAL.ObtenerPorId(idRol);
            if (r == null) throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            _rolDAL.Desactivar(idRol);

            RegistrarBitacora("Desactivar Rol", "Exitoso","El administrador desactivó el Rol: " + r.Nombre);
        }

        #endregion

        #region "Asignacion de Roles a Usuario"

        public void AsignarRolAUsuario(int idUsuario, int idRol)
        {
            Usuario u = _usuarioDAL.BuscarPorId(idUsuario);
            if (u == null) throw new Exception(T("Errores.UsuarioSeleccionadoNoEncontrado"));

            Rol r = _rolDAL.ObtenerPorId(idRol);
            if (r == null) throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            _rolDAL.AsignarRolAUsuario(idUsuario, idRol);

            RegistrarBitacora("Asignar Rol", "Exitoso","El administrador asignó el Rol '" + r.Nombre + "' al usuario: " + u.NombreUsuario);
        }

        public void QuitarRolDeUsuario(int idUsuario, int idRol)
        {
            Usuario u = _usuarioDAL.BuscarPorId(idUsuario);
            if (u == null) throw new Exception(T("Errores.UsuarioSeleccionadoNoEncontrado"));

            Rol r = _rolDAL.ObtenerPorId(idRol);
            if (r == null) throw new Exception(T("Errores.RBAC.RolNoEncontrado"));

            _rolDAL.QuitarRolDeUsuario(idUsuario, idRol);

            RegistrarBitacora("Quitar Rol", "Exitoso","El administrador quitó el Rol '" + r.Nombre + "' al usuario: " + u.NombreUsuario);
        }

        #endregion

        #region "Validaciones"

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception(T("Errores.RBAC.NombreObligatorio"));
        }

        private void ValidarUnicidadNombre(string nombre, int idExcluir)
        {
            if (_rolDAL.ExistePorNombre(nombre, idExcluir))
                throw new Exception(T("Errores.RBAC.NombreRolYaExiste"));
        }

        private void ValidarComposicionNoVacia(List<Componente> hijos)
        {
            if (hijos == null || hijos.Count == 0)
                throw new Exception(T("Errores.RBAC.ComposicionVacia"));
        }

        private void ValidarSinDuplicados(List<Componente> hijos)
        {
            Dictionary<string, string> origenes = new Dictionary<string, string>();

            foreach (Componente hijo in hijos)
            {
                string origenActual = DescribirOrigen(hijo);
                List<string> codigos = ExtraerCodigosEfectivos(hijo);

                foreach (string codigo in codigos)
                {
                    if (origenes.ContainsKey(codigo))
                    {
                        string mensaje = string.Format(T("Errores.RBAC.PermisoDuplicado"),codigo, origenes[codigo], origenActual);
                        throw new Exception(mensaje);
                    }
                    origenes[codigo] = origenActual;
                }
            }
        }

        #endregion

        #region "Metodos Refactorizados"

        private List<string> ExtraerCodigosEfectivos(Componente c)
        {
            List<string> resultado = new List<string>();

            if (c is PermisoSimple ps)
            {
                if (!string.IsNullOrWhiteSpace(ps.Codigo)) resultado.Add(ps.Codigo);
                return resultado;
            }

            if (c is Familia f)
            {
                if (!f.Activo) return resultado;
                foreach (Componente hijo in f.Hijos)
                {
                    resultado.AddRange(ExtraerCodigosEfectivos(hijo));
                }
                return resultado;
            }

            if (c is Rol r)
            {
                if (!r.Activo) return resultado;
                foreach (Componente hijo in r.Hijos)
                {
                    resultado.AddRange(ExtraerCodigosEfectivos(hijo));
                }
                return resultado;
            }

            return resultado;
        }

        private string DescribirOrigen(Componente c)
        {
            if (c is PermisoSimple ps) return "Permiso '" + ps.Codigo + "'";
            if (c is Familia f) return "Familia '" + f.Nombre + "'";
            if (c is Rol r) return "Rol '" + r.Nombre + "'";
            return "componente";
        }

        private void RegistrarBitacora(string accion, string resultado, string descripcion)
        {
            Usuario u = SM.Instancia.UsuarioActual;
            if (u == null) return;

            _bitacoraEventoBLL.Registrar(u.IdUsuario, u.NombreUsuario,
                "Administrador", accion, "Alta", resultado, descripcion);
        }

        #endregion
    }
}
