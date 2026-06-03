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
    public partial class frmMenuPrincipal : Form
    {
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
    }
}
