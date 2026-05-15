namespace UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            string email = txtEmail.Text.Trim();
            string contrasenia = txtContrasenia.Text;

            MessageBox.Show(
                "Validación visual correcta. Falta conectar con BLL.",
                "CineGest",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea salir del sistema?",
                "Salir",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void chkMostrarContrasenia_CheckedChanged(object sender, EventArgs e)
        {
            txtContrasenia.UseSystemPasswordChar = !chkMostrarContrasenia.Checked;
        }
        private bool ValidarCampos()
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
