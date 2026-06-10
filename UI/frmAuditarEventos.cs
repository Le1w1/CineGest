using BLL;
using iTextSharp.text.pdf;
using Servicios;
using System.Data;
using System.Diagnostics;
using ITextBaseColor = iTextSharp.text.BaseColor;
using ITextDocument = iTextSharp.text.Document;
using ITextElement = iTextSharp.text.Element;
using ITextFont = iTextSharp.text.Font;
using ITextPageSize = iTextSharp.text.PageSize;
using ITextParagraph = iTextSharp.text.Paragraph;
using ITextPhrase = iTextSharp.text.Phrase;

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
            cmbAccion.Items.Add("Re-Login");
            cmbAccion.Items.Add("Bloqueo de cuenta");
            cmbAccion.Items.Add("Crear Usuario");
            cmbAccion.Items.Add("Modificar Usuario");
            cmbAccion.Items.Add("Activar Usuario");
            cmbAccion.Items.Add("Desactivar Usuario");
            cmbAccion.Items.Add("Desbloquear Usuario");
            cmbAccion.Items.Add("Cambio de clave");
            cmbAccion.Items.Add("Imprimir PDF");
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

        #region "Imprimir y Exportar a PDF"
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> filas = dgvEventos.Rows.Cast<DataGridViewRow>().Where(f => !f.IsNewRow).ToList();

            if (filas.Count == 0)
            {
                RegistrarEventoImprimirPdf("Fallido","El usuario intentó imprimir/exportar a PDF la auditoría de eventos, pero no había eventos para exportar.");
                MessageBox.Show("No hay eventos para exportar.","Auditar Eventos",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Exportar auditoría de eventos";
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = "AuditoriaEventos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                saveFileDialog.DefaultExt = "pdf";

                if (saveFileDialog.ShowDialog() != DialogResult.OK){return;}
                try
                {
                    ExportarEventosAPdf(saveFileDialog.FileName);

                    lblMensaje.Text = "PDF exportado correctamente.";
                    RegistrarEventoImprimirPdf("Exitoso","El usuario imprimió/exportó a PDF la auditoría de eventos. Eventos exportados: " + filas.Count);

                    DialogResult respuesta = MessageBox.Show("PDF exportado correctamente.\n¿Desea abrir el archivo?","Auditar Eventos",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                    if (respuesta == DialogResult.Yes)
                    {
                        ProcessStartInfo proceso = new ProcessStartInfo(saveFileDialog.FileName);
                        proceso.UseShellExecute = true;
                        Process.Start(proceso);
                    }
                }
                catch (Exception ex)
                {
                    RegistrarEventoImprimirPdf("Fallido","No se pudo imprimir/exportar a PDF la auditoría de eventos. Error: " + ex.Message);
                    MessageBox.Show("No se pudo exportar el PDF.\n" + ex.Message,"Auditar Eventos",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

        }
        private void RegistrarEventoImprimirPdf(string resultado, string descripcion)
        {
            Usuario usuarioActual = SM.Instancia.UsuarioActual;

            int idUsuario = usuarioActual != null ? usuarioActual.IdUsuario : 0;
            string nombreUsuario = usuarioActual != null ? usuarioActual.NombreUsuario : "Sistema";

            _bitacoraEventoBLL.Registrar(idUsuario,nombreUsuario,"Administrador","Imprimir PDF","Media",resultado,descripcion);
        }
        private void ExportarEventosAPdf(string rutaArchivo)
        {
            List<DataGridViewColumn> columnas = ObtenerColumnasParaExportar();

            if (columnas.Count == 0)
            {
                throw new Exception("No hay columnas disponibles para exportar.");
            }

            using (FileStream stream = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                ITextDocument documento = new ITextDocument(ITextPageSize.A4.Rotate(), 25, 25, 30, 30);

                PdfWriter.GetInstance(documento, stream);

                documento.Open();

                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA,BaseFont.CP1252,BaseFont.NOT_EMBEDDED);

                ITextFont fuenteTitulo = new ITextFont(baseFont, 16, ITextFont.BOLD);
                ITextFont fuenteSubtitulo = new ITextFont(baseFont, 11, ITextFont.BOLD);
                ITextFont fuenteNormal = new ITextFont(baseFont, 9, ITextFont.NORMAL);
                ITextFont fuenteCabecera = new ITextFont(baseFont, 8, ITextFont.BOLD);
                ITextFont fuenteCelda = new ITextFont(baseFont, 8, ITextFont.NORMAL);

                ITextParagraph titulo = new ITextParagraph("CineGest - Auditoría de Eventos", fuenteTitulo);
                titulo.Alignment = ITextElement.ALIGN_CENTER;
                titulo.SpacingAfter = 10;
                documento.Add(titulo);

                documento.Add(new ITextParagraph("Fecha de exportación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),fuenteNormal));

                documento.Add(new ITextParagraph("Eventos exportados: " + dgvEventos.Rows.Cast<DataGridViewRow>().Count(f => !f.IsNewRow),fuenteNormal));

                documento.Add(new ITextParagraph(" ", fuenteNormal));

                documento.Add(new ITextParagraph("Filtros aplicados", fuenteSubtitulo));

                documento.Add(new ITextParagraph("Fecha desde: " + DtpFechaDesde.Value.ToString("dd/MM/yyyy") + " | Fecha hasta: " + dtpFechaHasta.Value.ToString("dd/MM/yyyy"),fuenteNormal));

                documento.Add(new ITextParagraph("Login: " + ObtenerTextoFiltro(txtUsuario.Text) + " | Módulo: " + cmbModulo.Text + " | Evento: " + cmbAccion.Text, fuenteNormal));

                documento.Add(new ITextParagraph("Criticidad: " + cmbCriticidad.Text + " | Resultado: " + cmbResultado.Text + " | Descripción: " + ObtenerTextoFiltro(txtDescripcion.Text),fuenteNormal));

                if (!string.IsNullOrWhiteSpace(txtNombre.Text) || !string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    documento.Add(new ITextParagraph("Usuario seleccionado - Nombre: " + ObtenerTextoFiltro(txtNombre.Text) +" | Apellido: " + ObtenerTextoFiltro(txtApellido.Text),fuenteNormal));
                }

                documento.Add(new ITextParagraph(" ", fuenteNormal));

                PdfPTable tabla = new PdfPTable(columnas.Count);
                tabla.WidthPercentage = 100;

                float[] anchos = columnas.Select(c => (float)Math.Max(c.Width, 60)).ToArray();

                tabla.SetWidths(anchos);

                foreach (DataGridViewColumn columna in columnas)
                {
                    AgregarCeldaCabecera(tabla, columna.HeaderText, fuenteCabecera);
                }

                foreach (DataGridViewRow fila in dgvEventos.Rows)
                {
                    if (fila.IsNewRow)
                    {
                        continue;
                    }

                    foreach (DataGridViewColumn columna in columnas)
                    {
                        string valor = ObtenerValorCelda(fila, columna.Name);
                        AgregarCeldaDato(tabla, valor, fuenteCelda);
                    }
                }

                documento.Add(tabla);

                documento.Close();
            }
        }
        private List<DataGridViewColumn> ObtenerColumnasParaExportar()
        {
            return dgvEventos.Columns
                .Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .Where(c => c.Name != "IdUsuario")
                .Where(c => c.Name != "FechaHora")
                .Where(c => c.Name != "Nombre")
                .Where(c => c.Name != "Apellido")
                .OrderBy(c => c.DisplayIndex)
                .ToList();
        }
        private void AgregarCeldaCabecera(PdfPTable tabla, string texto, ITextFont fuente)
        {
            PdfPCell celda = new PdfPCell(new ITextPhrase(texto, fuente));
            celda.BackgroundColor = ITextBaseColor.LIGHT_GRAY;
            celda.HorizontalAlignment = ITextElement.ALIGN_CENTER;
            celda.VerticalAlignment = ITextElement.ALIGN_MIDDLE;
            celda.Padding = 5;

            tabla.AddCell(celda);
        }
        private void AgregarCeldaDato(PdfPTable tabla, string texto, ITextFont fuente)
        {
            PdfPCell celda = new PdfPCell(new ITextPhrase(texto, fuente));
            celda.HorizontalAlignment = ITextElement.ALIGN_LEFT;
            celda.VerticalAlignment = ITextElement.ALIGN_TOP;
            celda.Padding = 4;

            tabla.AddCell(celda);
        }
        private string ObtenerValorCelda(DataGridViewRow fila, string nombreColumna)
        {
            if (!dgvEventos.Columns.Contains(nombreColumna))
            {
                return string.Empty;
            }

            object valor = fila.Cells[nombreColumna].FormattedValue;

            if (valor == null)
            {
                return string.Empty;
            }

            return valor.ToString();
        }
        private string ObtenerTextoFiltro(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return "Todos";
            }

            return valor.Trim();
        }
        #endregion
    }
}