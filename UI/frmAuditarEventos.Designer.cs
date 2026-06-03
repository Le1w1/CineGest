namespace UI
{
    partial class frmAuditarEventos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            gbFiltros = new GroupBox();
            txtDescripcion = new TextBox();
            cmbAccion = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            cmbResultado = new ComboBox();
            label5 = new Label();
            cmbCriticidad = new ComboBox();
            label4 = new Label();
            cmbModulo = new ComboBox();
            label3 = new Label();
            txtUsuario = new TextBox();
            label2 = new Label();
            lblFechaHasta = new Label();
            dtpFechaHasta = new DateTimePicker();
            DtpFechaDesde = new DateTimePicker();
            lblFechaDesde = new Label();
            dgvEventos = new DataGridView();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            btnVolver = new Button();
            lblMensaje = new Label();
            btnImprimir = new Button();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            label8 = new Label();
            label9 = new Label();
            gbFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 7);
            label1.Name = "label1";
            label1.Size = new Size(116, 15);
            label1.TabIndex = 0;
            label1.Text = "Auditoria de eventos";
            // 
            // gbFiltros
            // 
            gbFiltros.BackColor = Color.LightSkyBlue;
            gbFiltros.Controls.Add(txtDescripcion);
            gbFiltros.Controls.Add(cmbAccion);
            gbFiltros.Controls.Add(label7);
            gbFiltros.Controls.Add(label6);
            gbFiltros.Controls.Add(cmbResultado);
            gbFiltros.Controls.Add(label5);
            gbFiltros.Controls.Add(cmbCriticidad);
            gbFiltros.Controls.Add(label4);
            gbFiltros.Controls.Add(cmbModulo);
            gbFiltros.Controls.Add(label3);
            gbFiltros.Controls.Add(txtUsuario);
            gbFiltros.Controls.Add(label2);
            gbFiltros.Controls.Add(lblFechaHasta);
            gbFiltros.Controls.Add(dtpFechaHasta);
            gbFiltros.Controls.Add(DtpFechaDesde);
            gbFiltros.Controls.Add(lblFechaDesde);
            gbFiltros.Location = new Point(77, 286);
            gbFiltros.Margin = new Padding(3, 2, 3, 2);
            gbFiltros.Name = "gbFiltros";
            gbFiltros.Padding = new Padding(3, 2, 3, 2);
            gbFiltros.Size = new Size(794, 218);
            gbFiltros.TabIndex = 1;
            gbFiltros.TabStop = false;
            gbFiltros.Text = "Filtros de busqueda";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(512, 53);
            txtDescripcion.Margin = new Padding(3, 2, 3, 2);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(261, 23);
            txtDescripcion.TabIndex = 15;
            // 
            // cmbAccion
            // 
            cmbAccion.FormattingEnabled = true;
            cmbAccion.Location = new Point(110, 191);
            cmbAccion.Margin = new Padding(3, 2, 3, 2);
            cmbAccion.Name = "cmbAccion";
            cmbAccion.Size = new Size(261, 23);
            cmbAccion.TabIndex = 14;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 194);
            label7.Name = "label7";
            label7.Size = new Size(43, 15);
            label7.TabIndex = 13;
            label7.Text = "Evento";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(409, 56);
            label6.Name = "label6";
            label6.Size = new Size(69, 15);
            label6.TabIndex = 12;
            label6.Text = "Descripcion";
            // 
            // cmbResultado
            // 
            cmbResultado.FormattingEnabled = true;
            cmbResultado.Location = new Point(110, 159);
            cmbResultado.Margin = new Padding(3, 2, 3, 2);
            cmbResultado.Name = "cmbResultado";
            cmbResultado.Size = new Size(261, 23);
            cmbResultado.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 159);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 11;
            label5.Text = "Resultado: ";
            // 
            // cmbCriticidad
            // 
            cmbCriticidad.FormattingEnabled = true;
            cmbCriticidad.Location = new Point(110, 126);
            cmbCriticidad.Margin = new Padding(3, 2, 3, 2);
            cmbCriticidad.Name = "cmbCriticidad";
            cmbCriticidad.Size = new Size(261, 23);
            cmbCriticidad.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 128);
            label4.Name = "label4";
            label4.Size = new Size(64, 15);
            label4.TabIndex = 9;
            label4.Text = "Criticidad: ";
            // 
            // cmbModulo
            // 
            cmbModulo.FormattingEnabled = true;
            cmbModulo.Location = new Point(110, 88);
            cmbModulo.Margin = new Padding(3, 2, 3, 2);
            cmbModulo.Name = "cmbModulo";
            cmbModulo.Size = new Size(261, 23);
            cmbModulo.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 90);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 7;
            label3.Text = "Modulo: ";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(110, 53);
            txtUsuario.Margin = new Padding(3, 2, 3, 2);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(261, 23);
            txtUsuario.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 53);
            label2.Name = "label2";
            label2.Size = new Size(53, 15);
            label2.TabIndex = 5;
            label2.Text = "Usuario: ";
            // 
            // lblFechaHasta
            // 
            lblFechaHasta.AutoSize = true;
            lblFechaHasta.Location = new Point(409, 23);
            lblFechaHasta.Name = "lblFechaHasta";
            lblFechaHasta.Size = new Size(74, 15);
            lblFechaHasta.TabIndex = 4;
            lblFechaHasta.Text = "Fecha Hasta:";
            // 
            // dtpFechaHasta
            // 
            dtpFechaHasta.Location = new Point(512, 20);
            dtpFechaHasta.Margin = new Padding(3, 2, 3, 2);
            dtpFechaHasta.Name = "dtpFechaHasta";
            dtpFechaHasta.Size = new Size(261, 23);
            dtpFechaHasta.TabIndex = 3;
            // 
            // DtpFechaDesde
            // 
            DtpFechaDesde.Location = new Point(110, 19);
            DtpFechaDesde.Margin = new Padding(3, 2, 3, 2);
            DtpFechaDesde.Name = "DtpFechaDesde";
            DtpFechaDesde.Size = new Size(261, 23);
            DtpFechaDesde.TabIndex = 2;
            DtpFechaDesde.Value = new DateTime(2026, 6, 2, 21, 21, 5, 0);
            // 
            // lblFechaDesde
            // 
            lblFechaDesde.AutoSize = true;
            lblFechaDesde.Location = new Point(14, 22);
            lblFechaDesde.Name = "lblFechaDesde";
            lblFechaDesde.Size = new Size(79, 15);
            lblFechaDesde.TabIndex = 0;
            lblFechaDesde.Text = "Fecha Desde: ";
            // 
            // dgvEventos
            // 
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEventos.Location = new Point(102, 46);
            dgvEventos.Margin = new Padding(3, 2, 3, 2);
            dgvEventos.Name = "dgvEventos";
            dgvEventos.ReadOnly = true;
            dgvEventos.RowHeadersWidth = 51;
            dgvEventos.Size = new Size(923, 176);
            dgvEventos.TabIndex = 2;
            dgvEventos.SelectionChanged += dgvEventos_SelectionChanged;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(900, 271);
            btnBuscar.Margin = new Padding(3, 2, 3, 2);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(129, 44);
            btnBuscar.TabIndex = 3;
            btnBuscar.Text = "Aplicar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(900, 320);
            btnLimpiar.Margin = new Padding(3, 2, 3, 2);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(129, 44);
            btnLimpiar.TabIndex = 4;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(10, 46);
            btnVolver.Margin = new Padding(3, 2, 3, 2);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(84, 43);
            btnVolver.TabIndex = 5;
            btnVolver.Text = "Salir";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblMensaje
            // 
            lblMensaje.AutoSize = true;
            lblMensaje.Location = new Point(900, 233);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(0, 15);
            lblMensaje.TabIndex = 6;
            // 
            // btnImprimir
            // 
            btnImprimir.Location = new Point(902, 370);
            btnImprimir.Margin = new Padding(3, 2, 3, 2);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(126, 42);
            btnImprimir.TabIndex = 7;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(284, 247);
            txtNombre.Margin = new Padding(3, 2, 3, 2);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(110, 23);
            txtNombre.TabIndex = 16;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(520, 247);
            txtApellido.Margin = new Padding(3, 2, 3, 2);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(110, 23);
            txtApellido.TabIndex = 17;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(210, 249);
            label8.Name = "label8";
            label8.Size = new Size(51, 15);
            label8.TabIndex = 18;
            label8.Text = "Nombre";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(457, 249);
            label9.Name = "label9";
            label9.Size = new Size(51, 15);
            label9.TabIndex = 19;
            label9.Text = "Apellido";
            // 
            // frmAuditarEventos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(1064, 507);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(btnImprimir);
            Controls.Add(lblMensaje);
            Controls.Add(btnVolver);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBuscar);
            Controls.Add(dgvEventos);
            Controls.Add(gbFiltros);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmAuditarEventos";
            Text = "frmAuditarEventos";
            Load += frmAuditarEventos_Load;
            gbFiltros.ResumeLayout(false);
            gbFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private GroupBox gbFiltros;
        private ComboBox cmbModulo;
        private Label label4;
        private ComboBox cmbCriticidad;
        private Label label3;
        private TextBox txtUsuario;
        private Label label2;
        private Label lblFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private DateTimePicker DtpFechaDesde;
        private Label lblFechaDesde;
        private ComboBox cmbResultado;
        private Label label5;
        private DataGridView dgvEventos;
        private Button btnBuscar;
        private Button btnLimpiar;
        private Button btnVolver;
        private Label lblMensaje;
        private TextBox txtDescripcion;
        private ComboBox cmbAccion;
        private Label label7;
        private Label label6;
        private Button btnImprimir;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private Label label8;
        private Label label9;
    }
}