using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario,"Usuario","Inicio de sesión", "Alta", "Exitoso", "El usuario inició sesión correctamente");
            return usuario;
        }

        public Usuario ReLogin(string email, string contrasenia)
        {
            if (!SM.Instancia.HaySesionActiva())throw new Exception("No hay una sesión activa para realizar Re-Login.");

            if (string.IsNullOrWhiteSpace(email)) throw new Exception("Debe ingresar Email.");

            if (string.IsNullOrWhiteSpace(contrasenia))throw new Exception("Debe ingresar Contraseña.");

            Usuario usuarioActual = SM.Instancia.UsuarioActual;

            email = email.Trim().ToLower();

            Usuario usuarioNuevo = _usuarioDAL.BuscarPorEmail(email);

            if (usuarioNuevo == null)
            {
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario, "Usuario", "Cambio de usuario","Media","Fallido","Intento de Re-Login con un email inexistente.");

                throw new Exception("Las credenciales ingresadas son incorrectas.");
            }

            if (!usuarioNuevo.Activo)
            {
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario, "Usuario", "Cambio de usuario","Media","Fallido","Intento de Re-Login con una cuenta inactiva.");

                throw new Exception("La cuenta se encuentra inactiva. Contacte al Administrador.");
            }

            if (usuarioNuevo.Bloqueado)
            {
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario, "Usuario", "Cambio de usuario","Alta","Fallido","Intento de Re-Login con una cuenta bloqueada.");

                throw new Exception("La cuenta se encuentra bloqueada. Contacte al Administrador.");
            }

            string hashIngresado = _cripto.ObtenerHashSha256(contrasenia);

            if (hashIngresado != usuarioNuevo.PasswordHash)
            {
                _usuarioDAL.IncrementarIntentosFallidos(usuarioNuevo.IdUsuario);
                usuarioNuevo.IntentosFallidos++;

                _bitacoraEventoBLL.Registrar(usuarioNuevo.IdUsuario,usuarioNuevo.NombreUsuario, "Usuario", "Cambio de usuario","Alta","Fallido","El usuario ingresó una contraseña incorrecta durante el Re-Login.");

                if (usuarioNuevo.IntentosFallidos >= 3)
                {
                    _usuarioDAL.Bloquear(usuarioNuevo.IdUsuario);

                    _bitacoraEventoBLL.Registrar(usuarioNuevo.IdUsuario,usuarioNuevo.NombreUsuario, "Usuario", "Bloqueo de cuenta","Alta","Exitoso","La cuenta fue bloqueada por superar la cantidad de intentos permitidos durante el Re-Login.");

                    throw new Exception("La cuenta fue bloqueada por superar la cantidad de intentos permitidos.");
                }

                throw new Exception("Las credenciales ingresadas son incorrectas.");
            }

            if (usuarioNuevo.IdUsuario == usuarioActual.IdUsuario)
            {
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario, "Usuario", "Cambio de usuario","Media","Fallido","Intento de Re-Login con el mismo usuario actualmente logueado.");

                throw new Exception("No puede realizar Re-Login con el mismo usuario. Debe ingresar con un usuario diferente.");
            }

            _usuarioDAL.ReiniciarIntentosFallidos(usuarioNuevo.IdUsuario);

            SM.Instancia.IniciarSesion(usuarioNuevo);

            _bitacoraEventoBLL.Registrar(usuarioNuevo.IdUsuario,usuarioNuevo.NombreUsuario, "Usuario", "Cambio de usuario","Alta","Exitoso","Se reemplazó la sesión del usuario " + usuarioActual.NombreUsuario + " por " + usuarioNuevo.NombreUsuario + ".");

            return usuarioNuevo;
        }
        public void Logout()
        {
            if (!SM.Instancia.HaySesionActiva())
            {
                throw new Exception("No hay una sesión activa para cerrar.");
            }
            Usuario usuario = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Cierre de sesión", "Baja", "Exitoso", "El usuario cerró sesión correctamente");
            SM.Instancia.CerrarSesion(); 
        }

        public bool ActivarDesactivarUsuario(int idUsuario)
        {
            
            Usuario usuarioSeleccionado = _usuarioDAL.BuscarPorId(idUsuario);

            if (usuarioSeleccionado == null){throw new Exception("No se encontró el usuario seleccionado.");}

            bool nuevoEstado = !usuarioSeleccionado.Activo;

            _usuarioDAL.CambiarEstadoActivo(idUsuario, nuevoEstado);

            Usuario administrador = SM.Instancia.UsuarioActual;

            string accion = nuevoEstado ? "Activar Usuario" : "Desactivar Usuario";  //Si nuevoEstado es true  → accion = "Activar Usuario"

            string descripcion = nuevoEstado? "El administrador activó el usuario: " + usuarioSeleccionado.NombreUsuario: "El administrador desactivó el usuario: " + usuarioSeleccionado.NombreUsuario;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario,administrador.NombreUsuario,"Administrador",accion,"Alta","Exitoso",descripcion);
            return nuevoEstado;
        }

        public void DesbloquearUsuario(Usuario usuarioSeleccionado)
        {
            if (usuarioSeleccionado == null) throw new Exception("Debe seleccionar un usuario para desbloquear.");
            if (!usuarioSeleccionado.Bloqueado) throw new Exception("El usuario seleccionado no se encuentra bloqueado.");


            _usuarioDAL.DesbloquearUsuario(usuarioSeleccionado.IdUsuario);

            Usuario administrador = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario, administrador.NombreUsuario, "Administrador", "Desbloquear Usuario", "Alta", "Exitoso", "El administrador desbloqueó el usuario: " + usuarioSeleccionado.NombreUsuario);
        }

        public void CambiarClave(string claveActual, string nuevaClave, string confirmarClave)
        {
            if (!SM.Instancia.HaySesionActiva())
            {
                throw new Exception("No hay una sesión activa.");
            }

            if (nuevaClave != confirmarClave)
            {
                throw new Exception("La nueva clave y la confirmación no coinciden.");
            }

            Usuario usuario = SM.Instancia.UsuarioActual;

            string hashClaveActual = _cripto.ObtenerHashSha256(claveActual);

            if (hashClaveActual != usuario.PasswordHash)
            {
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Cambio de clave", "Alta", "Fallido", "El usuario ingresó una clave actual incorrecta.");

                throw new Exception("La clave actual ingresada es incorrecta.");
            }

            string hashNuevaClave = _cripto.ObtenerHashSha256(nuevaClave);

            if (hashNuevaClave == usuario.PasswordHash)
            {
                throw new Exception("La nueva clave no puede ser igual a la clave actual.");
            }

            _usuarioDAL.ActualizarPassword(usuario.IdUsuario, hashNuevaClave);

            usuario.PasswordHash = hashNuevaClave;
            usuario.DebeCambiarClave = false;

            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Cambio de clave", "Alta", "Exitoso", "El usuario modificó su contraseña correctamente.");
        }

        public void CrearUsuario(string nombre,string apellido,string dni,string email,bool activo)
        {
            nombre = (nombre ?? string.Empty).Trim();
            apellido = (apellido ?? string.Empty).Trim();
            dni = (dni ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(dni) ||
                string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Debe completar todos los campos obligatorios.");
            }
            string nombreSinEspacios = GenerarNombreUsuario(nombre, dni);

            #region "Validaciones con REGEX"

            if (!EsNombreOApellidoValido(nombre))
            {
                throw new Exception("El nombre solo puede contener letras y debe tener entre 2 y 50 caracteres.");
            }

            if (!EsNombreOApellidoValido(apellido))
            {
                throw new Exception("El apellido solo puede contener letras y debe tener entre 2 y 50 caracteres.");
            }

            if (!EsDNIValido(dni))
            {
                throw new Exception("Debe ingresar un DNI válido.");
            }

            if (!EsEmailValido(email))
            {
                throw new Exception("El email ingresado no tiene un formato válido.");
            }

            if (!EsNombreUsuarioValido(nombreSinEspacios))
            {
                throw new Exception("El nombre de usuario debe tener entre 4 y 30 caracteres y solo puede contener letras, números, punto, guion o guion bajo.");
            }

            #endregion

            #region "Validaciones de unicidad"
            if (_usuarioDAL.ExistePorEmail(email))
            {
                throw new Exception("El email ya se encuentra registrado.");
            }

            if (_usuarioDAL.ExistePorDNI(dni))
            {
                throw new Exception("El DNI ya se encuentra registrado.");
            }

            if (_usuarioDAL.ExistePorNombreUsuario(nombreSinEspacios))
            {
                throw new Exception("El nombre de usuario ya se encuentra registrado.");
            }
#endregion

            string contraseniaInicial = apellido + dni;


            string passwordHash = _cripto.ObtenerHashSha256(contraseniaInicial);

            Usuario nuevoUsuario = new Usuario
            {
                Nombre = nombre,
                Apellido = apellido,
                DNI = dni,
                Email = email,
                NombreUsuario = nombreSinEspacios,
                PasswordHash = passwordHash,
                Activo = activo,
                Bloqueado = false,
                IntentosFallidos = 0,
                DebeCambiarClave = true
            };

            _usuarioDAL.Insertar(nuevoUsuario);

            Usuario administrador = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario,administrador.NombreUsuario,"Administrador","Crear Usuario","Alta","Exitoso","El administrador creó el usuario: " + nombreSinEspacios);
        }

        public void ModificarUsuario(int idUsuario,string nombre,string apellido,string dni,string email,string nombreUsuario,bool activo)
        {
            nombre = (nombre ?? string.Empty).Trim();
            apellido = (apellido ?? string.Empty).Trim();
            dni = (dni ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim();
            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();

            if (idUsuario <= 0)throw new Exception("Debe seleccionar un usuario para modificar.");
         
            #region "Validaciones con REGEX y de Unicidad"

            if (string.IsNullOrWhiteSpace(nombre) ||string.IsNullOrWhiteSpace(apellido) ||string.IsNullOrWhiteSpace(dni) ||string.IsNullOrWhiteSpace(email) ||string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new Exception("Debe completar todos los campos obligatorios.");
            }

            if (!EsNombreOApellidoValido(nombre))
            {
                throw new Exception("El nombre solo puede contener letras y debe tener entre 2 y 50 caracteres.");
            }

            if (!EsNombreOApellidoValido(apellido))
            {
                throw new Exception("El apellido solo puede contener letras y debe tener entre 2 y 50 caracteres.");
            }

            if (!EsDNIValido(dni))
            {
                throw new Exception("Debe ingresar un DNI válido.");
            }

            if (!EsEmailValido(email))
            {
                throw new Exception("El email ingresado no tiene un formato válido.");
            }

            if (!EsNombreUsuarioValido(nombreUsuario))
            {
                throw new Exception("El nombre de usuario debe tener entre 4 y 30 caracteres y solo puede contener letras, números, punto, guion o guion bajo.");
            }

            if (_usuarioDAL.ExisteEmailEnOtroUsuario(email, idUsuario))
            {
                throw new Exception("El email ya se encuentra registrado por otro usuario.");
            }

            if (_usuarioDAL.ExisteDNIEnOtroUsuario(dni, idUsuario))
            {
                throw new Exception("El DNI ya se encuentra registrado por otro usuario.");
            }

            if (_usuarioDAL.ExisteNombreUsuarioEnOtroUsuario(nombreUsuario, idUsuario))
            {
                throw new Exception("El nombre de usuario ya se encuentra registrado por otro usuario.");
            }
            #endregion

            Usuario usuarioModificado = new Usuario
            {
                IdUsuario = idUsuario,
                Nombre = nombre,
                Apellido = apellido,
                DNI = dni,
                Email = email,
                NombreUsuario = nombreUsuario,
                Activo = activo,
                Bloqueado = false
            };

            _usuarioDAL.Modificar(usuarioModificado);

            Usuario administrador = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario,administrador.NombreUsuario,"Administrador","Modificar Usuario","Alta","Exitoso","El administrador modificó el usuario: " + nombreUsuario);
        }


        #region "Validaciones con REGEX"
        private bool EsNombreOApellidoValido(string valor)
        {
            string patron = @"^[\p{L}\s'-]{2,50}$";
            return Regex.IsMatch(valor, patron);
        }

        private bool EsDNIValido(string dni)
        {
            string patron = @"^\d{7,8}$";
            return Regex.IsMatch(dni, patron);
        }

        private bool EsEmailValido(string email)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patron);
        }

        private string GenerarNombreUsuario(string nombre, string dni)
        {
            string nombreSinEspacios = Regex.Replace(nombre.Trim(), @"\s+", "");
            return nombreSinEspacios + dni;
        }
        private bool EsNombreUsuarioValido(string nombreUsuario)
        {
            string patron = @"^[\p{L}0-9._-]{4,100}$";
            return Regex.IsMatch(nombreUsuario, patron);
        }
        #endregion

        public List<Usuario> ListarUsuarios(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro)) filtro = "ACTIVOS";

            filtro = filtro.Trim().ToUpper();

            return _usuarioDAL.ListarUsuarios(filtro);
        }

        public Usuario BuscarPorNombreUsuario(string nombreUsuario)
        {
            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return null;
            }

            return _usuarioDAL.BuscarPorNombreUsuario(nombreUsuario);
        }
    }
}
