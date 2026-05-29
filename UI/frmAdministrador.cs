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
    public partial class frmAdministrador : Form
    {
        public frmAdministrador()
        {
            InitializeComponent();
            dgvUsuarios.ClearSelection();
            txtNombre.Focus();
        }



        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDNI.Clear();
            txtEmail.Clear();
            txtNombreUsuario.Clear();

            chkActivo.Checked = true;
            chkBloqueado.Checked = false;

            lblMensaje.Text = string.Empty;

            dgvUsuarios.ClearSelection();

            txtNombre.Focus();
        }
        private void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
        }
        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            MostrarMensaje("Campos limpiados. Listo para ingresar un nuevo usuario.");
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Funcionalidad pendiente: Crear Usuario.");
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Funcionalidad pendiente: Modificar Usuario.");

        }

        private void btnDesbloquearUsuario_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Funcionalidad pendiente: Desbloquear Usuario.");

        }

        private void btnActivarDesactivarUsuario_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Funcionalidad pendiente: Activar / Desactivar Usuario.");

        }

        private void btnAuditarBitacora_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Funcionalidad pendiente: Auditar Bitácora.");

        }

        private void btnVolver_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
