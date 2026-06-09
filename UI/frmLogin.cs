using BLL;
using Servicios;

namespace UI
{
    public partial class frmLogin : Form
    {
        private readonly UsuarioBLL _usuarioBLL;
        private bool _esReLogin = false;
        public frmLogin(bool esReLogin = false)
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();
            _esReLogin = esReLogin;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposVacios()) return;

            string email = txtEmail.Text.Trim();
            string contrasenia = txtContrasenia.Text;

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
                    DialogResult respuesta = MessageBox.Show("Está utilizando una contraseña inicial. Se recomienda cambiarla por seguridad.\n\n¿Desea cambiarla ahora?","Cambio de contraseña recomendado",MessageBoxButtons.YesNo,MessageBoxIcon.Information);

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
                MessageBox.Show(ex.Message,"Inicio de sesión",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        
        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Desea salir del sistema?","Salir",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes) Application.Exit();  
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

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProviderLogin.SetError(txtEmail, "Debe ingresar el Email.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtContrasenia.Text))
            {
                errorProviderLogin.SetError(txtContrasenia, "Debe ingresar la Contraseña.");
                esValido = false;
            }

            if (!esValido)
            {
                lblMensaje.Text = "Debe ingresar Email y Contraseña.";
            }

            return esValido;
        }
    }
}
