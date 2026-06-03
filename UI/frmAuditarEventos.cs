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
    public partial class frmAuditarEventos : Form
    {
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;
        private readonly UsuarioBLL _usuarioBLL;

        public frmAuditarEventos()
        {
            InitializeComponent();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
            _usuarioBLL = new UsuarioBLL();
        }

        private void dgvEventos_SelectionChanged(object sender, EventArgs e)
        {
            CargarNombreApellidoDesdeEventoSeleccionado();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarEventos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtUsuario.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtDescripcion.Clear();

            DtpFechaDesde.Value = DateTime.Today.AddDays(-3);
            dtpFechaHasta.Value = DateTime.Today;

            cmbModulo.SelectedIndex = 0;
            cmbAccion.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            cmbResultado.SelectedIndex = 0;

            lblMensaje.Text = string.Empty;

            BuscarEventos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAuditarEventos_Load(object sender, EventArgs e)
        {
            ConfigurarCombos();
            ConfigurarEstadoInicial();
            BuscarEventos();
        }

        
        #region "Confiracion de Estados y Combos"
        private void ConfigurarEstadoInicial()
        {

            DtpFechaDesde.Value = DateTime.Today.AddDays(-3);
            dtpFechaHasta.Value = DateTime.Today;

            txtNombre.Clear();
            txtApellido.Clear();

            txtNombre.ReadOnly = true;
            txtApellido.ReadOnly = true;

            lblMensaje.Text = string.Empty;

            dgvEventos.DataSource = null;
            dgvEventos.ClearSelection();
        }

        private void ConfigurarCombos()
        {
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Todos");
            cmbModulo.Items.Add("Usuario");
            cmbModulo.Items.Add("Administrador");
            cmbModulo.SelectedIndex = 0;

            cmbAccion.Items.Clear();
            cmbAccion.Items.Add("Todos");
            cmbAccion.Items.Add("Inicio de sesión");
            cmbAccion.Items.Add("Cierre de sesión");
            cmbAccion.Items.Add("Cambio de usuario");
            cmbAccion.Items.Add("Bloqueo de cuenta");
            cmbAccion.Items.Add("Crear Usuario");
            cmbAccion.Items.Add("Modificar Usuario");
            cmbAccion.Items.Add("Activar Usuario");
            cmbAccion.Items.Add("Desactivar Usuario");
            cmbAccion.Items.Add("Desbloquear Usuario");
            cmbAccion.Items.Add("Cambio de clave");
            cmbAccion.SelectedIndex = 0;

            cmbCriticidad.Items.Clear();
            cmbCriticidad.Items.Add("Todas");
            cmbCriticidad.Items.Add("Baja");
            cmbCriticidad.Items.Add("Media");
            cmbCriticidad.Items.Add("Alta");
            cmbCriticidad.SelectedIndex = 0;

            cmbResultado.Items.Clear();
            cmbResultado.Items.Add("Todos");
            cmbResultado.Items.Add("Exitoso");
            cmbResultado.Items.Add("Fallido");
            cmbResultado.SelectedIndex = 0;
        }
        #endregion

        private void BuscarEventos()
        {
            try
            {
                lblMensaje.Text = string.Empty;

                DateTime fechaDesde = DtpFechaDesde.Value.Date;
                DateTime fechaHasta = dtpFechaHasta.Value.Date;

                string usuario = txtUsuario.Text.Trim();
                string modulo = cmbModulo.SelectedItem.ToString();
                string accion = cmbAccion.SelectedItem.ToString();
                string criticidad = cmbCriticidad.SelectedItem.ToString();
                string resultado = cmbResultado.SelectedItem.ToString();
                string descripcion = txtDescripcion.Text.Trim();

                List<BitacoraEvento> eventos = _bitacoraEventoBLL.ObtenerEventos(fechaDesde,fechaHasta,usuario,modulo,accion,criticidad,resultado,descripcion);

                dgvEventos.DataSource = null;
                dgvEventos.DataSource = eventos;

                ConfigurarColumnasGrilla();

                if (eventos.Count > 0)
                {
                    dgvEventos.ClearSelection();
                    dgvEventos.Rows[0].Selected = true;
                    dgvEventos.CurrentCell = dgvEventos.Rows[0].Cells["Usuario"];

                    CargarNombreApellidoDesdeEventoSeleccionado();
                }
                else
                {
                    txtNombre.Clear();
                    txtApellido.Clear();
                }

                lblMensaje.Text = eventos.Count == 0 ? "No se encontraron eventos con los filtros ingresados.": "Eventos encontrados: " + eventos.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"CineGest",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        private void CargarNombreApellidoDesdeEventoSeleccionado()
        {
            txtNombre.Clear();
            txtApellido.Clear();

            if (dgvEventos.CurrentRow == null)
            {
                return;
            }

            BitacoraEvento evento = dgvEventos.CurrentRow.DataBoundItem as BitacoraEvento;

            if (evento == null)
            {
                return;
            }

            txtNombre.Text = evento.Nombre;
            txtApellido.Text = evento.Apellido;
        }

        private void ConfigurarColumnasGrilla()
        {
            dgvEventos.ReadOnly = true;
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEventos.MultiSelect = false;
            dgvEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            int orden = 0;

            if (dgvEventos.Columns.Contains("IdEvento"))
            {
                dgvEventos.Columns["IdEvento"].Visible = false;

            }

            if (dgvEventos.Columns.Contains("IdUsuario"))
            {
                dgvEventos.Columns["IdUsuario"].Visible = false;
            }

            if (dgvEventos.Columns.Contains("Usuario"))
            {
                dgvEventos.Columns["Usuario"].HeaderText = "Login";
                dgvEventos.Columns["Usuario"].Width = 110;
                dgvEventos.Columns["Usuario"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Nombre"))
            {
                dgvEventos.Columns["Nombre"].Visible = false;
            }

            if (dgvEventos.Columns.Contains("Apellido"))
            {
                dgvEventos.Columns["Apellido"].Visible = false;
            }

            if (dgvEventos.Columns.Contains("FechaHora"))
            {
                dgvEventos.Columns["FechaHora"].Visible = false;
            }

            if (dgvEventos.Columns.Contains("Fecha"))
            {
                dgvEventos.Columns["Fecha"].HeaderText = "Fecha";
                dgvEventos.Columns["Fecha"].Width = 100;
                dgvEventos.Columns["Fecha"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Hora"))
            {
                dgvEventos.Columns["Hora"].HeaderText = "Hora";
                dgvEventos.Columns["Hora"].Width = 90;
                dgvEventos.Columns["Hora"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Modulo"))
            {
                dgvEventos.Columns["Modulo"].HeaderText = "Módulo";
                dgvEventos.Columns["Modulo"].Width = 120;
                dgvEventos.Columns["Modulo"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Accion"))
            {
                dgvEventos.Columns["Accion"].HeaderText = "Evento";
                dgvEventos.Columns["Accion"].Width = 150;
                dgvEventos.Columns["Accion"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Criticidad"))
            {
                dgvEventos.Columns["Criticidad"].HeaderText = "Criticidad";
                dgvEventos.Columns["Criticidad"].Width = 100;
                dgvEventos.Columns["Criticidad"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Resultado"))
            {
                dgvEventos.Columns["Resultado"].HeaderText = "Resultado";
                dgvEventos.Columns["Resultado"].Width = 100;
                dgvEventos.Columns["Resultado"].DisplayIndex = orden++;
            }

            if (dgvEventos.Columns.Contains("Descripcion"))
            {
                dgvEventos.Columns["Descripcion"].HeaderText = "Descripción";
                dgvEventos.Columns["Descripcion"].Width = 350;
                dgvEventos.Columns["Descripcion"].DisplayIndex = orden++;
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidad pendiente: Imprimir auditoría de eventos.","Auditar Eventos",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}