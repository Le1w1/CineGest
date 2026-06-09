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
using BLL;

namespace UI
{
    public partial class frmMenuPrincipal : Form
    {
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();
        public frmMenuPrincipal()
        {

            InitializeComponent();
        }

        private void frmMenuPrincipal_Load(object sender, EventArgs e)
        {
            CargarDatosSesion();


        }

        private void CargarDatosSesion()
        {
            if (SM.Instancia.HaySesionActiva())
            {
                Usuario usuario = SM.Instancia.UsuarioActual;

                lblUsuarioSesion.Text = "Usuario: " + usuario.NombreUsuario;
                lblEstadoSesion.Text = "Sesión activa";
            }
            else
            {
                lblUsuarioSesion.Text = "Usuario: Sin sesión";
                lblEstadoSesion.Text = "Sin sesión activa";
            }
        }

        private void mnuSesion_Click(object sender, EventArgs e)
        {

            frmSesion formSesion = new frmSesion();
            formSesion.ShowDialog();

            if (formSesion.ReLoginExitoso)
            {
                CargarDatosSesion();
                return;
            }

            if (formSesion.SesionCerrada)
            {
                this.Close();
                return;
            }

        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdministrador formAdministrador = new frmAdministrador();
            formAdministrador.ShowDialog();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea cerrar la sesión actual?",
                "Cerrar sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    _usuarioBLL.Logout();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CineGest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void reLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin formSesion = new frmLogin(true);
            formSesion.ShowDialog();

        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCambiarClave cambiarClave = new frmCambiarClave();
            cambiarClave.ShowDialog();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdministrador formAdministrador = new frmAdministrador();
            formAdministrador.ShowDialog();
        }

        private void bitacoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAuditarEventos formAuditarEventos = new frmAuditarEventos();
            formAuditarEventos.ShowDialog();
        }
    }
}
