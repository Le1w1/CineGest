using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class frmCambiarClave : Form
    {
        private readonly UsuarioBLL _usuarioBLL;

        public frmCambiarClave()
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                _usuarioBLL.CambiarClave(txtClaveActual.Text, txtNuevaClave.Text, txtConfirmarClave.Text);

                MessageBox.Show("Contraseña modificada correctamente.", "CineGest", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;

                MessageBox.Show(ex.Message, "Cambiar Clave", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMostrarClaves_CheckedChanged(object sender, EventArgs e)
        {
            bool ocultar = !chkMostrarClaves.Checked;

            txtClaveActual.UseSystemPasswordChar = ocultar;
            txtNuevaClave.UseSystemPasswordChar = ocultar;
            txtConfirmarClave.UseSystemPasswordChar = ocultar;
        }
        private bool ValidarCampos()
        {
            errorProviderCambiarClave.Clear();
            lblMensaje.Text = string.Empty;

            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtClaveActual.Text))
            {
                errorProviderCambiarClave.SetError(txtClaveActual, "Debe ingresar la clave actual.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNuevaClave.Text))
            {
                errorProviderCambiarClave.SetError(txtNuevaClave, "Debe ingresar la nueva clave.");
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmarClave.Text))
            {
                errorProviderCambiarClave.SetError(txtConfirmarClave, "Debe confirmar la nueva clave.");
                esValido = false;
            }

            if (!string.IsNullOrWhiteSpace(txtNuevaClave.Text) &&
                !string.IsNullOrWhiteSpace(txtConfirmarClave.Text) &&
                txtNuevaClave.Text != txtConfirmarClave.Text)
            {
                errorProviderCambiarClave.SetError(txtConfirmarClave, "La nueva clave y la confirmación no coinciden.");
                esValido = false;
            }

            if (!esValido)
            {
                lblMensaje.Text = "Revise los datos ingresados.";
            }

            return esValido;
        }
        private void LimpiarCampos()
        {
            txtClaveActual.Clear();
            txtNuevaClave.Clear();
            txtConfirmarClave.Clear();

            chkMostrarClaves.Checked = false;

            txtClaveActual.UseSystemPasswordChar = true;
            txtNuevaClave.UseSystemPasswordChar = true;
            txtConfirmarClave.UseSystemPasswordChar = true;

            errorProviderCambiarClave.Clear();
            lblMensaje.Text = string.Empty;

            txtClaveActual.Focus();
        }
    }
}
