using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL _usuarioDAL;
        private readonly Cripto _cripto;
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;
        public UsuarioBLL()
        {
            _usuarioDAL = new UsuarioDAL();
            _cripto = new Cripto();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
        }

        public Usuario Login(string email, string contraseñia) 
        {
            if (string.IsNullOrWhiteSpace(email))throw new Exception("Debe ingresar Email.");
            if (string.IsNullOrWhiteSpace(contraseñia)) throw new Exception("Debe ingresar Contraseña.");


            email = email.Trim().ToLower();
            Usuario usuario = _usuarioDAL.BuscarPorEmail(email);

            #region "Validar y Registro en caso de fallar"

            if (usuario == null)
            {
                _bitacoraEventoBLL.Registrar(0,email,"Login","Inicio de sesión","Media","Fallido","Intento de inicio de sesión con un email inexistente.");

                throw new Exception("Las credenciales ingresadas son incorrectas.");
            }

            if (!usuario.Activo)
            {
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario,usuario.NombreUsuario,"Login","Inicio de sesión","Media","Fallido","Intento de inicio de sesión con una cuenta inactiva.");
                
                throw new Exception("La cuenta se encuentra inactiva. Contacte al Administrador.");
            }

            if (usuario.Bloqueado)
            {
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario,usuario.NombreUsuario,"Login","Inicio de sesión","Alta","Fallido","Intento de inicio de sesión con una cuenta bloqueada.");

                throw new Exception("La cuenta se encuentra bloqueada. Contacte al Administrador.");
            }

            #endregion


            string hashIngresado = _cripto.ObtenerHashSha256(contraseñia);

            # region "Intentos fallidos y bloquear"     
            
            if (hashIngresado != usuario.PasswordHash)
            {
                _usuarioDAL.IncrementarIntentosFallidos(usuario.IdUsuario);
                usuario.IntentosFallidos++;               
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario,usuario.NombreUsuario,"Login","Inicio de sesión","Alta","Fallido", "El usuario ingresó una contraseña incorrecta.");

                if (usuario.IntentosFallidos >= 3)
                {
                    _usuarioDAL.Bloquear(usuario.IdUsuario);
                    _bitacoraEventoBLL.Registrar(usuario.IdUsuario,usuario.NombreUsuario,"Login","Bloqueo de cuenta","Alta","Exitoso","La cuenta fue bloqueada por superar la cantidad de intentos permitidos.");
                   
                    throw new Exception("La cuenta fue bloqueada por superar la cantidad de intentos permitidos.");
                }

                throw new Exception("Las credenciales ingresadas son incorrectas.");
            }

            #endregion

            _usuarioDAL.ReiniciarIntentosFallidos(usuario.IdUsuario);

            SM.Instancia.IniciarSesion(usuario);
            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario,"Login","Inicio de sesión", "Alta", "Exitoso", "El usuario inició sesión correctamente");
            return usuario;
        }
    

        public void Logout()
        {
            if (!SM.Instancia.HaySesionActiva())
            {
                throw new Exception("No hay una sesión activa para cerrar.");
            }
            Usuario usuario = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Logout", "Cierre de sesión", "Baja", "Exitoso", "El usuario cerró sesión correctamente");
            SM.Instancia.CerrarSesion(); 
        }
    }
}
