using BLL;
using Servicios;
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
    public partial class frmCambiarClave : Form, IObservadorIdioma
    {
        private readonly UsuarioBLL _usuarioBLL;

        public frmCambiarClave()
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();

            // Traducir antes de que el form se cargue.
            ActualizarIdioma();

            this.Load += frmCambiarClave_Load;
            this.FormClosed += frmCambiarClave_FormClosed;
        }

        private void frmCambiarClave_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
        }

        private void frmCambiarClave_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmCambiarClave.Title");
            label1.Text = t.Traducir("frmCambiarClave.LblTitulo");
            label3.Text = t.Traducir("frmCambiarClave.LblClaveActual");
            label4.Text = t.Traducir("frmCambiarClave.LblNuevaClave");
            label5.Text = t.Traducir("frmCambiarClave.LblConfirmarClave");
            chkMostrarClaves.Text = t.Traducir("frmCambiarClave.ChkMostrar");
            btnGuardar.Text = t.Traducir("frmCambiarClave.BtnGuardar");
            btnLimpiar.Text = t.Traducir("frmCambiarClave.BtnLimpiar");
            btnVolver.Text = t.Traducir("frmCambiarClave.BtnVolver");
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var t = Traductor.Instancia;

            try
            {
                _usuarioBLL.CambiarClave(txtClaveActual.Text, txtNuevaClave.Text, txtConfirmarClave.Text);

                MessageBox.Show(t.Traducir("frmCambiarClave.MsgExito"),"CineGest",MessageBoxButtons.OK,MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;

                MessageBox.Show(ex.Message, t.Traducir("frmCambiarClave.LblTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var t = Traductor.Instancia;

            bool esValido = true;

            if (string.IsNullOrWhiteSpace(txtClaveActual.Text))
            {
                errorProviderCambiarClave.SetError(txtClaveActual, t.Traducir("frmCambiarClave.MsgFaltaClaveActual"));
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtNuevaClave.Text))
            {
                errorProviderCambiarClave.SetError(txtNuevaClave, t.Traducir("frmCambiarClave.MsgFaltaNuevaClave"));
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmarClave.Text))
            {
                errorProviderCambiarClave.SetError(txtConfirmarClave, t.Traducir("frmCambiarClave.MsgFaltaConfirmar"));
                esValido = false;
            }

            if (!string.IsNullOrWhiteSpace(txtNuevaClave.Text) &&
                !string.IsNullOrWhiteSpace(txtConfirmarClave.Text) &&
                txtNuevaClave.Text != txtConfirmarClave.Text)
            {
                errorProviderCambiarClave.SetError(txtConfirmarClave, t.Traducir("frmCambiarClave.MsgNoCoinciden"));
                esValido = false;
            }

            if (!esValido)
            {
                lblMensaje.Text = t.Traducir("frmCambiarClave.MsgRevise");
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
