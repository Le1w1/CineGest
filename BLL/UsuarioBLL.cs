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
        private readonly IdiomaDAL _idiomaDAL;
        private readonly Cripto _cripto;
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;

        public UsuarioBLL()
        {
            _usuarioDAL = new UsuarioDAL();
            _idiomaDAL = new IdiomaDAL();
            _cripto = new Cripto();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
        }

        // sirve para acortar las llamadas a traduccion.
        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        public Usuario Login(string email, string contraseñia)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new Exception(T("Errores.DebeIngresarEmail"));
            if (string.IsNullOrWhiteSpace(contraseñia)) throw new Exception(T("Errores.DebeIngresarContrasenia"));


            email = email.Trim().ToLower();
            Usuario usuario = _usuarioDAL.BuscarPorEmail(email);

            #region "Validar y Registro en caso de fallar"

            if (usuario == null)
            {
                _bitacoraEventoBLL.Registrar(0, email, "Login", "Inicio de sesión", "Media", "Fallido", "Intento de inicio de sesión con un email inexistente.");

                throw new Exception(T("Errores.CredencialesIncorrectas"));
            }

            if (!usuario.Activo)
            {
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Login", "Inicio de sesión", "Media", "Fallido", "Intento de inicio de sesión con una cuenta inactiva.");

                throw new Exception(T("Errores.CuentaInactiva"));
            }

            if (usuario.Bloqueado)
            {
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Login", "Inicio de sesión", "Alta", "Fallido", "Intento de inicio de sesión con una cuenta bloqueada.");

                throw new Exception(T("Errores.CuentaBloqueada"));
            }

            #endregion


            string hashIngresado = _cripto.ObtenerHashSha256(contraseñia);

            #region "Intentos fallidos y bloquear"

            if (hashIngresado != usuario.PasswordHash)
            {
                _usuarioDAL.IncrementarIntentosFallidos(usuario.IdUsuario);
                usuario.IntentosFallidos++;
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Login", "Inicio de sesión", "Alta", "Fallido", "El usuario ingresó una contraseña incorrecta.");

                if (usuario.IntentosFallidos >= 3)
                {
                    _usuarioDAL.Bloquear(usuario.IdUsuario);
                    _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Login", "Bloqueo de cuenta", "Alta", "Exitoso", "La cuenta fue bloqueada por superar la cantidad de intentos permitidos.");

                    throw new Exception(T("Errores.CuentaBloqueadaIntentos"));
                }

                throw new Exception(T("Errores.CredencialesIncorrectas"));
            }

            #endregion

            _usuarioDAL.ReiniciarIntentosFallidos(usuario.IdUsuario);

            SM.Instancia.IniciarSesion(usuario);

            Idioma idioma = _idiomaDAL.ObtenerPorId(usuario.IdIdioma);
            if (idioma == null) idioma = _idiomaDAL.ObtenerPorCodigo("ES");
            if (idioma != null) SM.Instancia.EstablecerIdiomaInicial(idioma);

            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Inicio de sesión", "Alta", "Exitoso", "El usuario inició sesión correctamente");
            return usuario;
        }

        public Usuario ReLogin(string email, string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new Exception(T("Errores.DebeIngresarEmail"));

            if (string.IsNullOrWhiteSpace(contrasenia)) throw new Exception(T("Errores.DebeIngresarContrasenia"));

            email = email.Trim().ToLower();

            Usuario usuario = _usuarioDAL.BuscarPorEmail(email);

            if (usuario == null) throw new Exception(T("Errores.CredencialesIncorrectas"));

            if (!usuario.Activo) throw new Exception(T("Errores.CuentaInactiva"));

            if (usuario.Bloqueado) throw new Exception(T("Errores.CuentaBloqueada"));

            string hashIngresado = _cripto.ObtenerHashSha256(contrasenia);

            if (hashIngresado != usuario.PasswordHash)
            {
                _usuarioDAL.IncrementarIntentosFallidos(usuario.IdUsuario);
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 3)
                {
                    _usuarioDAL.Bloquear(usuario.IdUsuario);
                    throw new Exception(T("Errores.CuentaBloqueadaIntentos"));
                }

                throw new Exception(T("Errores.CredencialesIncorrectas"));
            }

            _usuarioDAL.ReiniciarIntentosFallidos(usuario.IdUsuario);

            if (SM.Instancia.HaySesionActiva())
            {
                Usuario usuarioActual = SM.Instancia.UsuarioActual;
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario, usuarioActual.NombreUsuario, "Usuario", "Re-Login", "Media", "Fallido", "Intento de Re-Login rechazado porque ya existe una sesión activa.");

                throw new Exception(T("Errores.SesionActivaExistente"));
            }

            throw new Exception(T("Errores.NoHaySesionParaReLogin"));
        }

        public void Logout()
        {
            if (!SM.Instancia.HaySesionActiva())
            {
                throw new Exception(T("Errores.NoHaySesionParaCerrar"));
            }

            Usuario usuario = SM.Instancia.UsuarioActual;

            // Persistir idioma SOLO si cambio durante la sesion.
            if (SM.Instancia.RequierePersistirIdioma())
            {
                Idioma idiomaFinal = SM.Instancia.IdiomaActual;

                _usuarioDAL.ActualizarIdioma(usuario.IdUsuario, idiomaFinal.IdIdioma);

                _bitacoraEventoBLL.Registrar(
                    usuario.IdUsuario,
                    usuario.NombreUsuario,
                    "Idioma",
                    "Persistir idioma",
                    "Baja",
                    "Exitoso",
                    "Se persistió el idioma del usuario al cerrar sesión: " + idiomaFinal.Nombre + ".");
            }

            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Cierre de sesión", "Baja", "Exitoso", "El usuario cerró sesión correctamente");
            SM.Instancia.CerrarSesion();
        }

        public bool ActivarDesactivarUsuario(int idUsuario)
        {
            Usuario usuarioSeleccionado = _usuarioDAL.BuscarPorId(idUsuario);

            if (usuarioSeleccionado == null) { throw new Exception(T("Errores.UsuarioSeleccionadoNoEncontrado")); }

            bool nuevoEstado = !usuarioSeleccionado.Activo;

            _usuarioDAL.CambiarEstadoActivo(idUsuario, nuevoEstado);

            Usuario administrador = SM.Instancia.UsuarioActual;

            string accion = nuevoEstado ? "Activar Usuario" : "Desactivar Usuario";

            string descripcion = nuevoEstado ? "El administrador activó el usuario: " + usuarioSeleccionado.NombreUsuario : "El administrador desactivó el usuario: " + usuarioSeleccionado.NombreUsuario;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario, administrador.NombreUsuario, "Administrador", accion, "Alta", "Exitoso", descripcion);
            return nuevoEstado;
        }

        public void DesbloquearUsuario(Usuario usuarioSeleccionado)
        {
            if (usuarioSeleccionado == null) throw new Exception(T("Errores.DebeSeleccionarUsuarioDesbloquear"));
            if (!usuarioSeleccionado.Bloqueado) throw new Exception(T("Errores.UsuarioNoBloqueado"));


            _usuarioDAL.DesbloquearUsuario(usuarioSeleccionado.IdUsuario);

            Usuario administrador = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario, administrador.NombreUsuario, "Administrador", "Desbloquear Usuario", "Alta", "Exitoso", "El administrador desbloqueó el usuario: " + usuarioSeleccionado.NombreUsuario);
        }

        public void CambiarClave(string claveActual, string nuevaClave, string confirmarClave)
        {
            if (!SM.Instancia.HaySesionActiva()) { throw new Exception(T("Errores.NoHaySesionActiva")); }

            if (string.IsNullOrWhiteSpace(claveActual)) { throw new Exception(T("Errores.DebeIngresarClaveActual")); }

            if (string.IsNullOrWhiteSpace(nuevaClave)) { throw new Exception(T("Errores.DebeIngresarNuevaClave")); }

            if (string.IsNullOrWhiteSpace(confirmarClave)) { throw new Exception(T("Errores.DebeConfirmarNuevaClave")); }

            if (nuevaClave != confirmarClave) { throw new Exception(T("Errores.ClavesNoCoinciden")); }

            if (!EsClaveSegura(nuevaClave)) { throw new Exception(T("Errores.ClaveInsegura")); }

            Usuario usuario = SM.Instancia.UsuarioActual;

            string hashClaveActual = _cripto.ObtenerHashSha256(claveActual);

            if (hashClaveActual != usuario.PasswordHash)
            {
                _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Cambio de clave", "Alta", "Fallido", "El usuario ingresó una clave actual incorrecta.");

                throw new Exception(T("Errores.ClaveActualIncorrecta"));
            }

            string hashNuevaClave = _cripto.ObtenerHashSha256(nuevaClave);

            if (hashNuevaClave == usuario.PasswordHash) { throw new Exception(T("Errores.ClaveIgualAnterior")); }

            _usuarioDAL.ActualizarPassword(usuario.IdUsuario, hashNuevaClave);

            usuario.PasswordHash = hashNuevaClave;
            usuario.DebeCambiarClave = false;

            _bitacoraEventoBLL.Registrar(usuario.IdUsuario, usuario.NombreUsuario, "Usuario", "Cambio de clave", "Alta", "Exitoso", "El usuario modificó su contraseña correctamente.");
        }


        public void CrearUsuario(string nombre, string apellido, string dni, string email, bool activo)
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
                throw new Exception(T("Errores.CamposObligatorios"));
            }
            string nombreSinEspacios = GenerarNombreUsuario(nombre, dni);

            #region "Validaciones con REGEX"

            if (!EsNombreOApellidoValido(nombre))
            {
                throw new Exception(T("Errores.NombreInvalido"));
            }

            if (!EsNombreOApellidoValido(apellido))
            {
                throw new Exception(T("Errores.ApellidoInvalido"));
            }

            if (!EsDNIValido(dni))
            {
                throw new Exception(T("Errores.DniInvalido"));
            }

            if (!EsEmailValido(email))
            {
                throw new Exception(T("Errores.EmailInvalido"));
            }

            if (!EsNombreUsuarioValido(nombreSinEspacios))
            {
                throw new Exception(T("Errores.NombreUsuarioInvalido"));
            }

            #endregion

            #region "Validaciones de unicidad"
            if (_usuarioDAL.ExistePorEmail(email))
            {
                throw new Exception(T("Errores.EmailYaRegistrado"));
            }

            if (_usuarioDAL.ExistePorDNI(dni))
            {
                throw new Exception(T("Errores.DniYaRegistrado"));
            }

            if (_usuarioDAL.ExistePorNombreUsuario(nombreSinEspacios))
            {
                throw new Exception(T("Errores.NombreUsuarioYaRegistrado"));
            }
            #endregion

            string contraseniaInicial = apellido + dni;
            string passwordHash = _cripto.ObtenerHashSha256(contraseniaInicial);

            Idioma idiomaPorDefecto = _idiomaDAL.ObtenerPorCodigo("ES");
            if (idiomaPorDefecto == null) throw new Exception(T("Errores.IdiomaPorDefectoNoEncontrado"));

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
                DebeCambiarClave = true,
                IdIdioma = idiomaPorDefecto.IdIdioma
            };

            _usuarioDAL.Insertar(nuevoUsuario);

            Usuario administrador = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario, administrador.NombreUsuario, "Administrador", "Crear Usuario", "Alta", "Exitoso", "El administrador creó el usuario: " + nombreSinEspacios);
        }

        public void ModificarUsuario(int idUsuario, string nombre, string apellido, string dni, string email, string nombreUsuario, bool activo)
        {
            nombre = (nombre ?? string.Empty).Trim();
            apellido = (apellido ?? string.Empty).Trim();
            dni = (dni ?? string.Empty).Trim();
            email = (email ?? string.Empty).Trim();
            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();

            if (idUsuario <= 0) throw new Exception(T("Errores.DebeSeleccionarUsuarioModificar"));

            #region "Validaciones con REGEX y de Unicidad"

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new Exception(T("Errores.CamposObligatorios"));
            }

            if (!EsNombreOApellidoValido(nombre))
            {
                throw new Exception(T("Errores.NombreInvalido"));
            }

            if (!EsNombreOApellidoValido(apellido))
            {
                throw new Exception(T("Errores.ApellidoInvalido"));
            }

            if (!EsDNIValido(dni))
            {
                throw new Exception(T("Errores.DniInvalido"));
            }

            if (!EsEmailValido(email))
            {
                throw new Exception(T("Errores.EmailInvalido"));
            }

            if (!EsNombreUsuarioValido(nombreUsuario))
            {
                throw new Exception(T("Errores.NombreUsuarioInvalido"));
            }

            if (_usuarioDAL.ExisteEmailEnOtroUsuario(email, idUsuario))
            {
                throw new Exception(T("Errores.EmailYaRegistradoOtroUsuario"));
            }

            if (_usuarioDAL.ExisteDNIEnOtroUsuario(dni, idUsuario))
            {
                throw new Exception(T("Errores.DniYaRegistradoOtroUsuario"));
            }

            if (_usuarioDAL.ExisteNombreUsuarioEnOtroUsuario(nombreUsuario, idUsuario))
            {
                throw new Exception(T("Errores.NombreUsuarioYaRegistradoOtroUsuario"));
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

            _bitacoraEventoBLL.Registrar(administrador.IdUsuario, administrador.NombreUsuario, "Administrador", "Modificar Usuario", "Alta", "Exitoso", "El administrador modificó el usuario: " + nombreUsuario);
        }


        #region "Validaciones con REGEX"
        private bool EsClaveSegura(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave)) { return false; }
            string patron = @"^(?=.*[A-Z])(?=.*\d)(?!.*\s).{8,}$";
            return Regex.IsMatch(clave, patron);
        }
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
