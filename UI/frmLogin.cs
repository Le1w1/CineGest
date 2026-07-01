using BLL;
using Servicios;
using Servicios.DigitoVerificador;
namespace UI
{
    public partial class frmLogin : Form, IObservadorIdioma
    {
        private readonly UsuarioBLL _usuarioBLL;
        private bool _esReLogin = false;
        private bool _cargandoCboIdioma = false;
        private readonly RolBLL _rolBLL;

        public frmLogin(bool esReLogin = false)
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();
            _esReLogin = esReLogin;
            _rolBLL = new RolBLL();

            if (_esReLogin)
            {
                btnSalir.Visible = false;
                btnVolver.Visible = true;
                this.CancelButton = btnVolver;
            }
            else
            {
                btnSalir.Visible = true;
                btnVolver.Visible = false;
                this.CancelButton = btnSalir;
            }

            CargarComboIdiomas();
            ActualizarIdioma();

            this.Load += frmLogin_Load;
            this.FormClosed += frmLogin_FormClosed;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmLogin.Title");
            lblTitulo.Text = t.Traducir("frmLogin.LblTitulo");
            lblSubtitulo.Text = t.Traducir("frmLogin.LblSubtitulo");
            lblEmail.Text = t.Traducir("frmLogin.LblEmail");
            lblContrasenia.Text = t.Traducir("frmLogin.LblContrasenia");
            chkMostrarContrasenia.Text = t.Traducir("frmLogin.ChkMostrar");
            btnIngresar.Text = t.Traducir("frmLogin.BtnIngresar");
            btnSalir.Text = t.Traducir("frmLogin.BtnSalir");
            btnVolver.Text = t.Traducir("frmLogin.BtnVolver");

            SincronizarCboIdioma();
        }

        #region "Selector de idioma en el form de Login"

        private void CargarComboIdiomas()
        {
            // Solo carga los items. El SelectedIndex se setea en SincronizarCboIdioma (que es llamado por ActualizarIdioma).
            _cargandoCboIdioma = true;

            cboIdioma.Items.Clear();
            cboIdioma.Items.Add("Español");   // index 0 → ES
            cboIdioma.Items.Add("English");   // index 1 → EN

            _cargandoCboIdioma = false;
        }

        private void SincronizarCboIdioma()
        {
            // Solo tiene sentido si los items ya estan cargados.
            if (cboIdioma.Items.Count == 0) return;

            _cargandoCboIdioma = true;

            string actual = Traductor.Instancia.CodigoIdiomaActual;
            int indiceObjetivo = (actual == "EN") ? 1 : 0;

            // Evitamos disparar el handler si ya estamos en el indice correcto.
            if (cboIdioma.SelectedIndex != indiceObjetivo)
            {
                cboIdioma.SelectedIndex = indiceObjetivo;
            }

            _cargandoCboIdioma = false;
        }

        private void cboIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCboIdioma) return;

            // Cambio de idioma: solo el Traductor (no toca SM ni BD). No hay sesion activa todavia, asi que no podemos usar IdiomaBLL.
            string codigo = (cboIdioma.SelectedIndex == 1) ? "EN" : "ES";

            try
            {
                Traductor.Instancia.CargarIdioma(codigo);
                ActualizarIdioma();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CineGest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposVacios()) return;

            string email = txtEmail.Text.Trim();
            string contrasenia = txtContrasenia.Text;

            var t = Traductor.Instancia;

            try
            {
                if (_esReLogin)
                {
                    _usuarioBLL.ReLogin(email, contrasenia);
                    return;
                }

                var usuario = _usuarioBLL.Login(email, contrasenia);

                if (usuario.DebeCambiarClave)
                {
                    DialogResult respuesta = MessageBox.Show(t.Traducir("frmLogin.MsgCambioClaveRecomendado"), t.Traducir("frmLogin.TitleCambioClave"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (respuesta == DialogResult.Yes)
                    {
                        frmCambiarClave formCambiarClave = new frmCambiarClave();
                        formCambiarClave.ShowDialog();
                    }
                }

                lblMensaje.Text = string.Empty;
                frmMenuPrincipal menu = new frmMenuPrincipal();

                // ELogin no debe recibir notificaciones de cambio de idioma:
                // esta oculto y no es parte de la sesion. Sin esto, los textos del Login terminan en el idioma
                // que el usuario haya elegido durante la sesion.
                SM.Instancia.Desuscribir(this);

                this.Hide();
                menu.ShowDialog();
                this.Show();

                // Despues del Logout, SM.CerrarSesion() ya restauro el Traductor a ES.
                // Re-suscribimos al Login y refrescamos sus textos para que arranque siempre en Español.
                SM.Instancia.Suscribir(this);
                ActualizarIdioma();

                txtContrasenia.Clear();
                txtEmail.Focus();
            }
            catch (IntegridadComprometidaException ex)
            {
                // Integridad rota: modo reparacion acotado. Solo un admin con permisos de respaldo puede entrar.
               
                //   1) Validar credenciales minimas.
                //   2) Cargar los roles del usuario desde una base potencialmente corrupta. Este es el riesgo asumido de la Salida "modo reparacion". 
                //   3) Si es admin: abrir frmRepararIntegridad como modal.
                //   4) Si no es admin: informar y no dejar entrar.

                // NO se toca SM aca. La sesion (si es que se termina iniciando) la maneja frmRepararIntegridad internamente cuando el admin apreta
                // el boton de restaurar, y se descarta con el Application.Restart() al finalizar la reparacion.

                try
                {
                    Usuario adminReparador = _usuarioBLL.ValidarCredencialesMinimo(email, contrasenia);
                    System.Collections.Generic.List<Rol> rolesAdmin =_rolBLL.ListarRolesDeUsuario(adminReparador.IdUsuario);
                    bool esAdmin = false;

                    foreach (Rol rol in rolesAdmin)
                    {
                        if (rol != null && rol.TienePermiso("BAK_GESTIONAR"))
                        {
                            esAdmin = true;
                            break;
                        }
                    }

                    if (!esAdmin)
                    {
                        // El sistema queda inaccesible hasta que entre alguien con permisos.
                        MessageBox.Show(t.Traducir("frmLogin.MsgIntegridadSinPermiso"),t.Traducir("frmLogin.LblSubtitulo"),MessageBoxButtons.OK,MessageBoxIcon.Error);
                        txtContrasenia.Clear();
                        txtEmail.Focus();
                        return;
                    }

                    // Es admin: abrir la pantalla de reparacion como modal.
                    using (var formReparar = new frmRepararIntegridad(adminReparador, rolesAdmin, ex.Inconsistencias))
                    {
                        formReparar.ShowDialog(this);
                    }

                    // Si el admin reparo con exito, Application.Restart() ya se ejecuto y estas lineas no corren.
                    txtContrasenia.Clear();
                    txtEmail.Focus();
                }
                catch (Exception credencialesEx)
                {
                    // Las credenciales que ingreso no son validas.
                    // No damos pistas sobre si la corrupcion es real o si el usuario no existe:
                    // mostramos el mensaje generico del intento.
                    lblMensaje.Text = credencialesEx.Message;
                    MessageBox.Show(credencialesEx.Message,t.Traducir("frmLogin.LblSubtitulo"),MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(t.Traducir("frmLogin.MsgConfirmarSalir"), t.Traducir("frmLogin.TitleSalir"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes) Application.Exit();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMostrarContrasenia_CheckedChanged(object sender, EventArgs e)
        {
            txtContrasenia.UseSystemPasswordChar = !chkMostrarContrasenia.Checked;
        }

        private bool ValidarCamposVacios()
        {
            errorProviderLogin.Clear();
            lblMensaje.Text = string.Empty;
            bool esValido = true;

            var t = Traductor.Instancia;

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProviderLogin.SetError(txtEmail, t.Traducir("frmLogin.MsgFaltaEmail"));
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtContrasenia.Text))
            {
                errorProviderLogin.SetError(txtContrasenia, t.Traducir("frmLogin.MsgFaltaContrasenia"));
                esValido = false;
            }

            if (!esValido)
            {
                lblMensaje.Text = t.Traducir("frmLogin.MsgFaltaEmailContrasenia");
            }

            return esValido;
        }

       
    }
}
