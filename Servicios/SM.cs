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

        // Idioma con el que el usuario inicio sesion (snapshot al Login)
        private Idioma _idiomaInicial;

        // Idioma vigente en la sesion (puede cambiar N veces sin tocar la BD)
        private Idioma _idiomaActual;

        private List<IObservadorIdioma> _observadores;

        // Roles del usuario logueado. Los chequeos de permisos se delegan
        // al patron Composite (Componente.TienePermiso recorre el arbol).
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


        /// Limpia el estado de la sesion (usuario + idioma).
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

        /// Fija el idioma con el que el usuario inicio sesion.
        /// Se llama UNA SOLA VEZ desde el Login (BLL) despues de IniciarSesion.
        /// Carga el JSON pero NO notifica observadores (todavia no hay forms abiertos).
        public void EstablecerIdiomaInicial(Idioma idioma)
        {
            if (idioma == null) throw new Exception("Debe indicar un idioma valido.");

            _idiomaInicial = idioma;
            _idiomaActual = idioma;

            Traductor.Instancia.CargarIdioma(idioma.Codigo);
        }


        /// Cambia el idioma EN MEMORIA. No persiste en BD. La persistencia ocurre al Logout, comparando con el IdiomaInicial.
        public void CambiarIdioma(Idioma idioma)
        {
            if (idioma == null) throw new Exception("Debe indicar un idioma valido.");

            _idiomaActual = idioma;

            Traductor.Instancia.CargarIdioma(idioma.Codigo);

            NotificarCambioIdioma();
        }


        /// Devuelve true si el idioma actual difiere del inicial. Lo usa el Logout para decidir si hay que escribir BD.
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

        /// Recibe los Roles del usuario logueado tal como vienen del DAL(con su composicion Composite ya armada). 
        /// El SM solo los guarda.
        public void EstablecerRolesUsuario(List<Rol> roles)
        {
            _rolesUsuario = roles ?? new List<Rol>();
        }

        /// Pregunta a cada Rol asignado si contiene el codigo solicitado.
        /// El recorrido recursivo lo hace cada Componente por si mismo(TienePermiso esta declarado abstract en Componente).
        public bool TienePermiso(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return false;
            if (_rolesUsuario == null) return false;

            foreach (Rol rol in _rolesUsuario)
            {
                if (rol != null && rol.TienePermiso(codigo)) return true;
            }

            return false;
        }

        #endregion

        #region "Notificacion de cambio de idioma"

        /// Notifica a todos los observadores que el idioma ha cambiado.
        private void NotificarCambioIdioma()
        {
            // Recorremos una copia para evitar errores si un observador se desuscribe a si mismo dentro de ActualizarIdioma().
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
