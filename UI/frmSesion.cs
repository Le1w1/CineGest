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
    public partial class frmSesion : Form, IObservadorIdioma
    {
        private readonly UsuarioBLL _usuarioBLL;
        public bool SesionCerrada { get; private set; }
        public bool ReLoginExitoso { get; private set; }

        public frmSesion()
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();

            // Traducir YA, antes de que el form se pinte.
            ActualizarIdioma();

            this.Load += frmSesion_Load;
            this.FormClosed += frmSesion_FormClosed;
        }

        private void frmSesion_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
        }

        private void frmSesion_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmSesion.Title");
            lblTitulo.Text = t.Traducir("frmSesion.LblTitulo");
            label1.Text = t.Traducir("frmSesion.LblOpciones");
            label2.Text = t.Traducir("frmSesion.LblEmail");
            label3.Text = t.Traducir("frmSesion.LblContrasenia");
            btnCambiarClave.Text = t.Traducir("frmSesion.BtnCambiarClave");
            btnCerrarSesion.Text = t.Traducir("frmSesion.BtnCerrarSesion");
            btnReLogin.Text = t.Traducir("frmSesion.BtnReLogin");
            btnVolver.Text = t.Traducir("frmSesion.BtnVolver");
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(
                t.Traducir("frmSesion.ConfirmarCerrarSesion"),
                t.Traducir("frmSesion.CerrarSesionTitulo"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    _usuarioBLL.Logout();
                    SesionCerrada = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CineGest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnCambiarClave_Click(object sender, EventArgs e)
        {
            frmCambiarClave formCambiarClave = new frmCambiarClave();
            formCambiarClave.ShowDialog();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReLogin_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            try
            {
                var usuario = _usuarioBLL.ReLogin(txtEmail.Text, txtContraseña.Text);

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
                ReLoginExitoso = true;

                MessageBox.Show(
                    t.Traducir("frmSesion.MsgReLoginExitoso"),
                    t.Traducir("frmSesion.BtnReLogin"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Traducir("frmSesion.BtnReLogin"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
