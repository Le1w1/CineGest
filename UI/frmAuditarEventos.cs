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

        public frmAuditarEventos()
        {
            InitializeComponent();
            _bitacoraEventoBLL = new BitacoraEventoBLL();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarEventos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtUsuario.Clear();

            cmbModulo.SelectedIndex = 0;
            cmbCriticidad.SelectedIndex = 0;
            cmbResultado.SelectedIndex = 0;

            lblMensaje.Text = string.Empty;
            dgvEventos.DataSource = null;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAuditarEventos_Load(object sender, EventArgs e)
        {
            ConfigurarCombos();
            ConfigurarEstadoInicial();
        }
        #region "Confiracion de Estados y Combos"
        private void ConfigurarEstadoInicial()
        {
            DtpFechaDesde.Value = DateTime.Today;
            dtpFechaHasta.Value = DateTime.Today;

            lblMensaje.Text = string.Empty;

            dgvEventos.DataSource = null;
            dgvEventos.ClearSelection();
        }
        private void ConfigurarCombos()
        {
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Todos");
            cmbModulo.Items.Add("Login");
            cmbModulo.Items.Add("Logout");
            cmbModulo.Items.Add("Administrador");
            cmbModulo.Items.Add("Bitácora");
            cmbModulo.SelectedIndex = 0;

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

                DateTime fechaDesde = DtpFechaDesde.Value;
                DateTime fechaHasta = dtpFechaHasta.Value;
                string usuario = txtUsuario.Text.Trim();
                string modulo = cmbModulo.SelectedItem.ToString();
                string criticidad = cmbCriticidad.SelectedItem.ToString();
                string resultado = cmbResultado.SelectedItem.ToString();

                List<BitacoraEvento> eventos = _bitacoraEventoBLL.ObtenerEventos(fechaDesde,fechaHasta,usuario,modulo,criticidad,resultado);

                dgvEventos.DataSource = null;
                dgvEventos.DataSource = eventos;

                ConfigurarColumnasGrilla();

                if (eventos.Count == 0)
                {
                    lblMensaje.Text = "No se encontraron eventos con los filtros ingresados.";
                }
                else
                {
                    lblMensaje.Text = "Eventos encontrados: " + eventos.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"CineGest",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarColumnasGrilla()
        {
            if (dgvEventos.Columns.Contains("IdEvento"))
            {
                dgvEventos.Columns["IdEvento"].HeaderText = "ID";
                dgvEventos.Columns["IdEvento"].Width = 50;
            }

            if (dgvEventos.Columns.Contains("IdUsuario"))
            {
                dgvEventos.Columns["IdUsuario"].Visible = false;
            }

            if (dgvEventos.Columns.Contains("FechaHora"))
            {
                dgvEventos.Columns["FechaHora"].HeaderText = "Fecha / Hora";
                dgvEventos.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }

            if (dgvEventos.Columns.Contains("Usuario"))
            {
                dgvEventos.Columns["Usuario"].HeaderText = "Usuario";
            }

            if (dgvEventos.Columns.Contains("Modulo"))
            {
                dgvEventos.Columns["Modulo"].HeaderText = "Módulo";
            }

            if (dgvEventos.Columns.Contains("Accion"))
            {
                dgvEventos.Columns["Accion"].HeaderText = "Acción";
            }

            if (dgvEventos.Columns.Contains("Criticidad"))
            {
                dgvEventos.Columns["Criticidad"].HeaderText = "Criticidad";
            }

            if (dgvEventos.Columns.Contains("Resultado"))
            {
                dgvEventos.Columns["Resultado"].HeaderText = "Resultado";
            }

            if (dgvEventos.Columns.Contains("Descripcion"))
            {
                dgvEventos.Columns["Descripcion"].HeaderText = "Descripción";
            }

            dgvEventos.ClearSelection();
        }
    }
}
