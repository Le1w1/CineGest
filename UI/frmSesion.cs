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
    public partial class frmSesion : Form
    {
        private readonly UsuarioBLL _usuarioBLL;
        public bool SesionCerrada { get; private set; }
        public bool ReLoginExitoso { get; private set; }

        public frmSesion()
        {
            InitializeComponent();
            _usuarioBLL = new UsuarioBLL();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Desea cerrar la sesión actual?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            try
            {
                _usuarioBLL.ReLogin(txtEmail.Text,txtContraseña.Text);

                ReLoginExitoso = true;

                MessageBox.Show("Re-Login realizado correctamente.","Re-Login",MessageBoxButtons.OK,MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Re-Login",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
