using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class FamiliaBLL
    {
        private readonly FamiliaDAL _familiaDAL;
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;

        public FamiliaBLL()
        {
            _familiaDAL = new FamiliaDAL();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
        }

        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        #region "Lectura"

        public List<Familia> ListarTodas(bool incluirInactivas = false)
        {
            return _familiaDAL.ListarTodas(incluirInactivas);
        }

        public Familia ObtenerPorId(int idFamilia)
        {
            return _familiaDAL.ObtenerPorId(idFamilia);
        }

        #endregion

        #region "Escritura"

        /// Crea una Familia nueva con su composicion completa de forma transaccional. Si alguna validacion falla, no se persiste nada.
        public void Crear(Familia familia)
        {
            if (familia == null) throw new Exception(T("Errores.RBAC.ComposicionVacia"));

            string nombre = (familia.Nombre ?? string.Empty).Trim();
            familia.Nombre = nombre;

            ValidarNombre(nombre);
            ValidarUnicidadNombre(nombre, 0);
            ValidarComposicionNoVacia(familia.Hijos);
            ValidarSinDuplicados(familia.Hijos);
            // No hay ciclos posibles: la Familia aun no existe en BD.

            int nuevoId = _familiaDAL.InsertarConComposicion(familia);
            familia.IdFamilia = nuevoId;

            RegistrarBitacora("Crear Familia", "Exitoso","El administrador creó la Familia: " + nombre);
        }

        /// Reemplaza la composicion completa de una Familia existente.
        public void Modificar(Familia familia)
        {
            if (familia == null || familia.IdFamilia <= 0)
                throw new Exception(T("Errores.RBAC.FamiliaNoEncontrada"));

            string nombre = (familia.Nombre ?? string.Empty).Trim();
            familia.Nombre = nombre;

            ValidarNombre(nombre);
            ValidarUnicidadNombre(nombre, familia.IdFamilia);
            ValidarComposicionNoVacia(familia.Hijos);
            ValidarSinDuplicados(familia.Hijos);
            ValidarSinCiclos(familia);

            _familiaDAL.ModificarConComposicion(familia);

            RegistrarBitacora("Modificar Familia", "Exitoso","El administrador modificó la Familia: " + nombre);
        }

        public void Desactivar(int idFamilia)
        {
            Familia f = _familiaDAL.ObtenerPorId(idFamilia);
            if (f == null) throw new Exception(T("Errores.RBAC.FamiliaNoEncontrada"));

            _familiaDAL.Desactivar(idFamilia);

            RegistrarBitacora("Desactivar Familia", "Exitoso","El administrador desactivó la Familia: " + f.Nombre);
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
            if (_familiaDAL.ExistePorNombre(nombre, idExcluir))
                throw new Exception(T("Errores.RBAC.NombreFamiliaYaExiste"));
        }

        private void ValidarComposicionNoVacia(List<Componente> hijos)
        {
            if (hijos == null || hijos.Count == 0)
                throw new Exception(T("Errores.RBAC.ComposicionVacia"));
        }

        /// Detecta si dos hijos distintos aportan el mismo PermisoSimple(directa o indirectamente). 
        /// Reporta con mensaje especifico indicando que permiso fue el que ocasiono el error y cuales son los dos origenes.
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

        /// Detecta ciclos al modificar una Familia. 
        /// La Familia que se edita no puede contenerse a si misma (directa ni indirectamente).
        private void ValidarSinCiclos(Familia familiaEnEdicion)
        {
            foreach (Componente hijo in familiaEnEdicion.Hijos)
            {
                if (hijo is Familia famHija)
                {
                    if (famHija.IdFamilia == familiaEnEdicion.IdFamilia)
                    {
                        string mensaje = string.Format(T("Errores.RBAC.CicloDirecto"),familiaEnEdicion.Nombre);
                        throw new Exception(mensaje);
                    }

                    if (FamiliaContieneId(famHija, familiaEnEdicion.IdFamilia))
                    {
                        string mensaje = string.Format(T("Errores.RBAC.CicloIndirecto"),famHija.Nombre, familiaEnEdicion.Nombre);
                        throw new Exception(mensaje);
                    }
                }
            }
        }

        #endregion

        #region "Metodos Refactorizados"

        /// Recorre el subarbol de un Componente y devuelve todos los codigos de PermisoSimple que aporta.
        /// Se usa para detectar duplicados al armar la composicion. 
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

        /// Devuelve true si la Familia (o cualquier descendiente) referencia el idFamilia indicado. Sirve para ciclos.
        private bool FamiliaContieneId(Familia f, int idBuscar)
        {
            if (f == null) return false;

            foreach (Componente hijo in f.Hijos)
            {
                if (hijo is Familia famHija)
                {
                    if (famHija.IdFamilia == idBuscar) return true;
                    if (FamiliaContieneId(famHija, idBuscar)) return true;
                }
            }

            return false;
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
