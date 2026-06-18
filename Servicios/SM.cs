using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class SM
    {
        private static SM _instancia;
        private Usuario _usuarioActual;

        private Idioma _idiomaInicial;
        private Idioma _idiomaActual;

        private List<IObservadorIdioma> _observadores;
        private List<Rol> _rolesUsuario;

        private SM()
        {
            _observadores = new List<IObservadorIdioma>();
            _rolesUsuario = new List<Rol>();
        }

        public static SM Instancia
        {
            get
            {
                if (_instancia == null) { _instancia = new SM(); }
                return _instancia;
            }
        }

        #region "Sesion de usuario"

        public Usuario UsuarioActual { get { return _usuarioActual; } }

        public bool HaySesionActiva() { return _usuarioActual != null; }

        public void IniciarSesion(Usuario usuario) => _usuarioActual = usuario;

        /// <summary>
        /// Limpia el estado de la sesion (usuario + idioma).
        /// </summary>
        public void CerrarSesion()
        {
            _usuarioActual = null;
            _idiomaInicial = null;
            _idiomaActual = null;
            _rolesUsuario = new List<Rol>();
        }

        #endregion

        #region "Idioma y Observer"

        public Idioma IdiomaActual { get { return _idiomaActual; } }
        public Idioma IdiomaInicial { get { return _idiomaInicial; } }

       
        public void EstablecerIdiomaInicial(Idioma idioma)
        {
            if (idioma == null) throw new Exception("Debe indicar un idioma valido.");

            _idiomaInicial = idioma;
            _idiomaActual = idioma;

            Traductor.Instancia.CargarIdioma(idioma.Codigo);
        }

       
        public void CambiarIdioma(Idioma idioma)
        {
            if (idioma == null) throw new Exception("Debe indicar un idioma valido.");

            _idiomaActual = idioma;

            Traductor.Instancia.CargarIdioma(idioma.Codigo);

            NotificarCambioIdioma();
        }

        
        public bool RequierePersistirIdioma()
        {
            if (_idiomaActual == null || _idiomaInicial == null) return false;
            return _idiomaActual.IdIdioma != _idiomaInicial.IdIdioma;
        }

        public void Suscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            if (!_observadores.Contains(observador)) _observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            _observadores.Remove(observador);
        }

        #endregion

        #region "Roles y permisos del usuario logueado"

        public List<Rol> RolesUsuario { get { return _rolesUsuario; } }

       
        public void EstablecerRolesUsuario(List<Rol> roles)
        {
            _rolesUsuario = roles ?? new List<Rol>();
        }

      
        public bool TienePermiso(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return false;
            if (_rolesUsuario == null) return false;

            foreach (Rol rol in _rolesUsuario)
            {
                if (rol == null) continue;
                if (rol.TienePermiso(codigo)) return true;
            }

            return false;
        }

        #endregion

        #region "Notificacion de cambio de idioma"

        private void NotificarCambioIdioma()
        {
            var copia = _observadores.ToList();

            foreach (var observador in copia)
            {
                try
                {
                    observador.ActualizarIdioma();
                }
                catch
                {
                    // Si un form falla al traducirse, no debe romper a los demas.
                }
            }
        }

        #endregion
    }
}
