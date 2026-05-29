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
    public partial class frmAdministrador : Form
    {
        private readonly UsuarioBLL _usuarioBLL;
        private Usuario _usuarioSeleccionado;
        public frmAdministrador()
        {
            InitializeComponent();
            dgvUsuarios.ClearSelection();
            txtNombre.Focus();
            _usuarioBLL = new UsuarioBLL();
            ConfigurarGrillaUsuarios();
            CargarUsuarios();

        }

        #region "Configuracion del DataGridView"
        private void CargarUsuarios()
        {
            try
            {
                string filtro = ObtenerFiltroSeleccionado();

                var usuarios = _usuarioBLL.ListarUsuarios(filtro);

                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = usuarios;

                lblCantidadUsuarios.Text = "Número de Usuarios: " + usuarios.Count;
                lblMensaje.Text = string.Empty;

                dgvUsuarios.ClearSelection();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;

                MessageBox.Show(
                    ex.Message,
                    "Cargar usuarios",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        private string ObtenerFiltroSeleccionado()
        {
            if (rbActivos.Checked)
            {
                return "ACTIVOS";
            }

            if (rbBloqueados.Checked)
            {
                return "BLOQUEADOS";
            }

            return "TODOS";
        }
        private void ConfigurarGrillaUsuarios()
        {
            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "IdUsuario",
                Name = "colIdUsuario",
                Visible = false
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "DNI",
                DataPropertyName = "DNI",
                Name = "colDNI"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Apellido",
                DataPropertyName = "Apellido",
                Name = "colApellido"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                Name = "colNombre"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Name = "colEmail"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Usuario",
                DataPropertyName = "NombreUsuario",
                Name = "colNombreUsuario"
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Activo",
                DataPropertyName = "Activo",
                Name = "colActivo"
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Bloqueado",
                DataPropertyName = "Bloqueado",
                Name = "colBloqueado"
            });
        }

        private void rbFiltros_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CargarUsuarios();
            }
        }
        #endregion
   
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            Usuario usuario = dgvUsuarios.CurrentRow.DataBoundItem as Usuario;

            if (usuario == null) return;

            _usuarioSeleccionado = usuario;

            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtDNI.Text = usuario.DNI;
            txtEmail.Text = usuario.Email;
            txtNombreUsuario.Text = usuario.NombreUsuario;

            chkActivo.Checked = usuario.Activo;
            chkBloqueado.Checked = usuario.Bloqueado;
            btnActivarDesactivarUsuario.Text = usuario.Activo
    ? "Desactivar Usuario"
    : "Activar Usuario";

            lblMensaje.Text = "Usuario seleccionado: " + usuario.NombreUsuario;

        }
        

        private void LimpiarCampos()
        {
            _usuarioSeleccionado = null;

            txtNombre.Clear();
            txtApellido.Clear();
            txtDNI.Clear();
            txtEmail.Clear();
            txtNombreUsuario.Clear();

            chkActivo.Checked = true;
            chkBloqueado.Checked = false;
            btnActivarDesactivarUsuario.Text = "Activar / Desactivar Usuario";
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
            try
            {
                _usuarioBLL.CrearUsuario(txtNombre.Text,txtApellido.Text,txtDNI.Text,txtEmail.Text,txtNombreUsuario.Text,chkActivo.Checked,chkBloqueado.Checked);
              
                LimpiarCampos();
                CargarUsuarios();
                lblMensaje.Text = "Usuario creado correctamente.";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message,"Crear Usuario",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                lblMensaje.Text = "Debe seleccionar un usuario para modificar.";
                MessageBox.Show("Debe seleccionar un usuario para modificar.","Modificar Usuario",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show("¿Desea modificar el usuario seleccionado?","Modificar Usuario",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (respuesta == DialogResult.No) return; 

            try
            {
                _usuarioBLL.ModificarUsuario(_usuarioSeleccionado.IdUsuario,txtNombre.Text,txtApellido.Text,txtDNI.Text,txtEmail.Text,txtNombreUsuario.Text,chkActivo.Checked,chkBloqueado.Checked);

                CargarUsuarios();

                LimpiarCampos();

                lblMensaje.Text = "Usuario modificado correctamente.";

                MessageBox.Show("Usuario modificado correctamente.","Modificar Usuario",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;

                MessageBox.Show(ex.Message,"Modificar Usuario",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void btnDesbloquearUsuario_Click(object sender, EventArgs e)
        {
            MostrarMensaje("Funcionalidad pendiente: Desbloquear Usuario.");

        }

        private void btnActivarDesactivarUsuario_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                lblMensaje.Text = "Debe seleccionar un usuario para activar o desactivar.";

                MessageBox.Show(
                    "Debe seleccionar un usuario para activar o desactivar.",
                    "Activar / Desactivar Usuario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string accion = _usuarioSeleccionado.Activo ? "desactivar" : "activar";

            DialogResult respuesta = MessageBox.Show(
                "¿Desea " + accion + " el usuario seleccionado?",
                "Activar / Desactivar Usuario",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.No)
            {
                return;
            }

            try
            {
                bool quedoActivo = _usuarioBLL.ActivarDesactivarUsuario(_usuarioSeleccionado.IdUsuario);

                CargarUsuarios();

                LimpiarCampos();

                string mensaje = quedoActivo
                    ? "Usuario activado correctamente."
                    : "Usuario desactivado correctamente.";

                lblMensaje.Text = mensaje;

                MessageBox.Show(
                    mensaje,
                    "Activar / Desactivar Usuario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;

                MessageBox.Show(
                    ex.Message,
                    "Activar / Desactivar Usuario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
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
