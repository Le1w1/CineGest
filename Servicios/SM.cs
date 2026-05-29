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

        private SM(){}

        public static SM Instancia
        {
            get
            {
                if (_instancia == null) {_instancia = new SM();}
                return _instancia;
            }
        }

        public Usuario UsuarioActual { get { return _usuarioActual; } }

        public bool HaySesionActiva() { return _usuarioActual != null; }

        public void IniciarSesion(Usuario usuario) =>_usuarioActual = usuario;
       
        public void CerrarSesion() => _usuarioActual = null;
        
    }
}
