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
    public partial class frmGestionDeUsuario : Form, IObservadorIdioma
    {
        private readonly UsuarioBLL _usuarioBLL;
        private readonly RolBLL _rolBLL;
        private Usuario _usuarioSeleccionado;

        public frmGestionDeUsuario()
        {
            InitializeComponent();
            dgvUsuarios.ClearSelection();
            txtNombre.Focus();
            _usuarioBLL = new UsuarioBLL();
            _rolBLL = new RolBLL();
            ConfigurarGrillaUsuarios();
            CargarUsuarios();
            CargarRolesEnCombo();
            ConfigurarModoCreacion();

            // Traducir  antes de que el form se cargue.
            ActualizarIdioma();

            this.Load += frmAdministrador_Load;
            this.FormClosed += frmAdministrador_FormClosed;
        }

        private void frmAdministrador_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
        }

        private void frmAdministrador_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmAdministrador.Title");
            lblTitulo.Text = t.Traducir("frmAdministrador.LblTitulo");

            // Filtros
            gbFiltros.Text = t.Traducir("frmAdministrador.GbFiltros");
            rbActivos.Text = t.Traducir("frmAdministrador.RbActivos");
            rbBloqueados.Text = t.Traducir("frmAdministrador.RbBloqueados");
            rbTodos.Text = t.Traducir("frmAdministrador.RbTodos");

            // Datos del Usuario
            gbDatosUsuario.Text = t.Traducir("frmAdministrador.GbDatosUsuario");
            lblNombre.Text = t.Traducir("frmAdministrador.LblNombre");
            lblApellido.Text = t.Traducir("frmAdministrador.LblApellido");
            lblDNI.Text = t.Traducir("frmAdministrador.LblDNI");
            lblEmail.Text = t.Traducir("frmAdministrador.LblEmail");
            lblNombreUsuario.Text = t.Traducir("frmAdministrador.LblNombreUsuario");
            chkActivo.Text = t.Traducir("frmAdministrador.ChkActivo");
            lblRol.Text = t.Traducir("frmAdministrador.LblRol");

            // Botones
            btnCrearUsuario.Text = t.Traducir("frmAdministrador.BtnCrearUsuario");
            btnModificarUsuario.Text = t.Traducir("frmAdministrador.BtnModificarUsuario");
            btnDesbloquearUsuario.Text = t.Traducir("frmAdministrador.BtnDesbloquearUsuario");
            btnLimpiar.Text = t.Traducir("frmAdministrador.BtnLimpiar");
            btnVolver.Text = t.Traducir("frmAdministrador.BtnVolver");

            // Botón Activar/Desactivar: depende del estado seleccionado
            if (_usuarioSeleccionado == null)
            {
                btnActivarDesactivarUsuario.Text = t.Traducir("frmAdministrador.BtnActivarDesactivar");
            }
            else
            {
                btnActivarDesactivarUsuario.Text = _usuarioSeleccionado.Activo? t.Traducir("frmAdministrador.BtnDesactivar"): t.Traducir("frmAdministrador.BtnActivar");
            }

            // Cabeceras del DataGridView
            if (dgvUsuarios.Columns.Count > 0)
            {
                if (dgvUsuarios.Columns.Contains("colIdUsuario")) dgvUsuarios.Columns["colIdUsuario"].HeaderText = t.Traducir("frmAdministrador.ColID");
                if (dgvUsuarios.Columns.Contains("colDNI")) dgvUsuarios.Columns["colDNI"].HeaderText = t.Traducir("frmAdministrador.ColDNI");
                if (dgvUsuarios.Columns.Contains("colApellido")) dgvUsuarios.Columns["colApellido"].HeaderText = t.Traducir("frmAdministrador.ColApellido");
                if (dgvUsuarios.Columns.Contains("colNombre")) dgvUsuarios.Columns["colNombre"].HeaderText = t.Traducir("frmAdministrador.ColNombre");
                if (dgvUsuarios.Columns.Contains("colEmail")) dgvUsuarios.Columns["colEmail"].HeaderText = t.Traducir("frmAdministrador.ColEmail");
                if (dgvUsuarios.Columns.Contains("colNombreUsuario")) dgvUsuarios.Columns["colNombreUsuario"].HeaderText = t.Traducir("frmAdministrador.ColUsuario");
                if (dgvUsuarios.Columns.Contains("colActivo")) dgvUsuarios.Columns["colActivo"].HeaderText = t.Traducir("frmAdministrador.ColActivo");
                if (dgvUsuarios.Columns.Contains("colBloqueado")) dgvUsuarios.Columns["colBloqueado"].HeaderText = t.Traducir("frmAdministrador.ColBloqueado");
            }

            // Label de cantidad de usuarios: respetar el numero ya cargado
            ActualizarLblCantidad(dgvUsuarios.RowCount);
        }

        private void ActualizarLblCantidad(int cantidad)
        {
            lblCantidadUsuarios.Text = Traductor.Instancia.Traducir("frmAdministrador.LblCantidadUsuarios") + " " + cantidad;
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
                lblNombreUsuario.Visible = false;
                txtNombreUsuario.Visible = false;
                ActualizarLblCantidad(usuarios.Count);
                lblMensaje.Text = string.Empty;

                dgvUsuarios.ClearSelection();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;

                MessageBox.Show(ex.Message, Traductor.Instancia.Traducir("frmAdministrador.LblTitulo"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarModoCreacion()
        {
            _usuarioSeleccionado = null;

            lblNombreUsuario.Visible = false;
            txtNombreUsuario.Visible = false;
            txtNombreUsuario.Clear();

            // El combo de Rol solo aplica para creación de usuarios nuevos.
            lblRol.Visible = true;
            cboRol.Visible = true;
            cboRol.Enabled = true;
            if (cboRol.Items.Count > 0) cboRol.SelectedIndex = 0;

            btnActivarDesactivarUsuario.Text = Traductor.Instancia.Traducir("frmAdministrador.BtnActivarDesactivar");
        }

        /// Carga los Roles activos en el ComboBox. Lo llamo una sola vez al abrir el form. Si el admin crea/desactiva roles desde otro form hay que reabrir Administrador para verlos.
        
        private void CargarRolesEnCombo()
        {
            try
            {
                List<Rol> roles = _rolBLL.ListarTodos();
                cboRol.DataSource = roles;
                cboRol.DisplayMember = "Nombre";
                cboRol.ValueMember = "IdRol";

                if (roles.Count == 0)
                {
                    cboRol.Enabled = false;
                    lblMensaje.Text = "No hay Roles activos disponibles. Cree uno desde 'Gestión de Roles y Familias'.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        private void ConfigurarModoEdicion()
        {
            lblNombreUsuario.Visible = true;
            txtNombreUsuario.Visible = true;

            // En modo edición el combo SI se muestra: el admin puede cambiar el rol.
            lblRol.Visible = true;
            cboRol.Visible = true;
            cboRol.Enabled = cboRol.Items.Count > 0;
        }

        private string ObtenerFiltroSeleccionado()
        {
            if (rbActivos.Checked) return "ACTIVOS";
            if (rbBloqueados.Checked) return "BLOQUEADOS";
            return "TODOS";
        }

        private void ConfigurarGrillaUsuarios()
        {
            var t = Traductor.Instancia;

            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColID"),
                DataPropertyName = "IdUsuario",
                Name = "colIdUsuario",
                Visible = false
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColDNI"),
                DataPropertyName = "DNI",
                Name = "colDNI"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColApellido"),
                DataPropertyName = "Apellido",
                Name = "colApellido"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColNombre"),
                DataPropertyName = "Nombre",
                Name = "colNombre"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColEmail"),
                DataPropertyName = "Email",
                Name = "colEmail"
            });

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColUsuario"),
                DataPropertyName = "NombreUsuario",
                Name = "colNombreUsuario"
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColActivo"),
                DataPropertyName = "Activo",
                Name = "colActivo"
            });

            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = t.Traducir("frmAdministrador.ColBloqueado"),
                DataPropertyName = "Bloqueado",
                Name = "colBloqueado"
            });
        }

        private void rbFiltros_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                CargarUsuarios();
                LimpiarCampos();
            }
        }
        #endregion

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            Usuario usuario = dgvUsuarios.CurrentRow.DataBoundItem as Usuario;

            if (usuario == null) return;

            _usuarioSeleccionado = usuario;

            var t = Traductor.Instancia;

            ConfigurarModoEdicion();
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtDNI.Text = usuario.DNI;
            txtEmail.Text = usuario.Email;
            txtNombreUsuario.Text = usuario.NombreUsuario;

            chkActivo.Checked = usuario.Activo;
            btnActivarDesactivarUsuario.Text = usuario.Activo? t.Traducir("frmAdministrador.BtnDesactivar"): t.Traducir("frmAdministrador.BtnActivar");

            // Mostrar el rol actual del usuario en el combo
            if (cboRol.Items.Count > 0 && usuario.IdRol > 0)
                cboRol.SelectedValue = usuario.IdRol;

            lblMensaje.Text = t.Traducir("frmAdministrador.MsgUsuarioSeleccionado") + " " + usuario.NombreUsuario;
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
            btnActivarDesactivarUsuario.Text = Traductor.Instancia.Traducir("frmAdministrador.BtnActivarDesactivar");
            lblMensaje.Text = string.Empty;

            dgvUsuarios.SelectionChanged -= dgvUsuarios_SelectionChanged;
            dgvUsuarios.ClearSelection();
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            ConfigurarModoCreacion();
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
            MostrarMensaje(Traductor.Instancia.Traducir("frmAdministrador.MsgCamposLimpiados"));
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            try
            {
                int idRolSeleccionado = cboRol.SelectedValue is int v ? v : 0;
                _usuarioBLL.CrearUsuario(txtNombre.Text, txtApellido.Text, txtDNI.Text, txtEmail.Text, chkActivo.Checked, idRolSeleccionado);
                LimpiarCampos();
                CargarUsuarios();
                lblMensaje.Text = t.Traducir("frmAdministrador.MsgUsuarioCreado");
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmAdministrador.BtnCrearUsuario"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            if (_usuarioSeleccionado == null)
            {
                lblMensaje.Text = t.Traducir("frmAdministrador.MsgFaltaSeleccionModificar");
                MessageBox.Show(t.Traducir("frmAdministrador.MsgFaltaSeleccionModificar"), t.Traducir("frmAdministrador.BtnModificarUsuario"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(t.Traducir("frmAdministrador.ConfirmarModificar"),t.Traducir("frmAdministrador.BtnModificarUsuario"),MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (respuesta == DialogResult.No) return;

            try
            {
                int idRolSeleccionado = cboRol.SelectedValue is int v ? v : 0;
                _usuarioBLL.ModificarUsuario(_usuarioSeleccionado.IdUsuario, txtNombre.Text, txtApellido.Text, txtDNI.Text, txtEmail.Text, txtNombreUsuario.Text, chkActivo.Checked, idRolSeleccionado);

                CargarUsuarios();
                LimpiarCampos();

                lblMensaje.Text = t.Traducir("frmAdministrador.MsgUsuarioModificado");

                MessageBox.Show(t.Traducir("frmAdministrador.MsgUsuarioModificado"),t.Traducir("frmAdministrador.BtnModificarUsuario"),MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmAdministrador.BtnModificarUsuario"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDesbloquearUsuario_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            if (_usuarioSeleccionado == null)
            {
                lblMensaje.Text = t.Traducir("frmAdministrador.MsgFaltaSeleccionDesbloquear");
                MessageBox.Show(t.Traducir("frmAdministrador.MsgFaltaSeleccionDesbloquear"), t.Traducir("frmAdministrador.BtnDesbloquearUsuario"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(t.Traducir("frmAdministrador.ConfirmarDesbloquear"),t.Traducir("frmAdministrador.BtnDesbloquearUsuario"),MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                _usuarioBLL.DesbloquearUsuario(_usuarioSeleccionado);
                CargarUsuarios();
                LimpiarCampos();

                lblMensaje.Text = t.Traducir("frmAdministrador.MsgUsuarioDesbloqueado");

                MessageBox.Show(t.Traducir("frmAdministrador.MsgUsuarioDesbloqueado"),t.Traducir("frmAdministrador.BtnDesbloquearUsuario"),MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmAdministrador.BtnDesbloquearUsuario"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnActivarDesactivarUsuario_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            if (_usuarioSeleccionado == null)
            {
                lblMensaje.Text = t.Traducir("frmAdministrador.MsgFaltaSeleccionActivar");
                MessageBox.Show(t.Traducir("frmAdministrador.MsgFaltaSeleccionActivar"), t.Traducir("frmAdministrador.BtnActivarDesactivar"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mensajeConfirmacion = _usuarioSeleccionado.Activo? t.Traducir("frmAdministrador.ConfirmarDesactivar"): t.Traducir("frmAdministrador.ConfirmarActivar");

            DialogResult respuesta = MessageBox.Show(mensajeConfirmacion,t.Traducir("frmAdministrador.BtnActivarDesactivar"),MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (respuesta == DialogResult.No) return;

            try
            {
                bool quedoActivo = _usuarioBLL.ActivarDesactivarUsuario(_usuarioSeleccionado.IdUsuario);

                CargarUsuarios();
                LimpiarCampos();

                string mensaje = quedoActivo? t.Traducir("frmAdministrador.MsgUsuarioActivado"): t.Traducir("frmAdministrador.MsgUsuarioDesactivado");

                lblMensaje.Text = mensaje;

                MessageBox.Show(mensaje, t.Traducir("frmAdministrador.BtnActivarDesactivar"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmAdministrador.BtnActivarDesactivar"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVolver_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
