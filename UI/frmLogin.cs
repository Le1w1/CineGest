using BLL;
using Servicios;

namespace UI
{
    public partial class frmLogin : Form, IObservadorIdioma
    {
        private readonly UsuarioBLL _usuarioBLL;
        private bool _esReLogin = false;
        private bool _cargandoCboIdioma = false;

        public frmLogin(bool esReLogin = false)
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();
            _esReLogin = esReLogin;

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
            // Solo carga los items. El SelectedIndex se setea en
            // SincronizarCboIdioma (que es llamado por ActualizarIdioma).
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

            // Cambio "ligero" de idioma: solo el Traductor (no toca SM ni BD).
            // No hay sesion activa todavia, asi que no podemos usar IdiomaBLL.
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
                    DialogResult respuesta = MessageBox.Show(
                        t.Traducir("frmLogin.MsgCambioClaveRecomendado"),
                        t.Traducir("frmLogin.TitleCambioClave"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (respuesta == DialogResult.Yes)
                    {
                        frmCambiarClave formCambiarClave = new frmCambiarClave();
                        formCambiarClave.ShowDialog();
                    }
                }
                lblMensaje.Text = string.Empty;
                frmMenuPrincipal menu = new frmMenuPrincipal();

                this.Hide();
                menu.ShowDialog();
                this.Show();

                txtContrasenia.Clear();
                txtEmail.Focus();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmLogin.LblSubtitulo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(
                t.Traducir("frmLogin.MsgConfirmarSalir"),
                t.Traducir("frmLogin.TitleSalir"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

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
