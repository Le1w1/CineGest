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
            gbFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(147, 20);
            label1.TabIndex = 0;
            label1.Text = "Auditoria de eventos";
            // 
            // gbFiltros
            // 
            gbFiltros.BackColor = Color.LightSkyBlue;
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
            gbFiltros.Location = new Point(91, 339);
            gbFiltros.Name = "gbFiltros";
            gbFiltros.Size = new Size(448, 312);
            gbFiltros.TabIndex = 1;
            gbFiltros.TabStop = false;
            gbFiltros.Text = "Filtros de busqueda";
            // 
            // cmbResultado
            // 
            cmbResultado.FormattingEnabled = true;
            cmbResultado.Location = new Point(126, 268);
            cmbResultado.Name = "cmbResultado";
            cmbResultado.Size = new Size(151, 28);
            cmbResultado.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 268);
            label5.Name = "label5";
            label5.Size = new Size(82, 20);
            label5.TabIndex = 11;
            label5.Text = "Resultado: ";
            // 
            // cmbCriticidad
            // 
            cmbCriticidad.FormattingEnabled = true;
            cmbCriticidad.Location = new Point(126, 218);
            cmbCriticidad.Name = "cmbCriticidad";
            cmbCriticidad.Size = new Size(151, 28);
            cmbCriticidad.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 218);
            label4.Name = "label4";
            label4.Size = new Size(80, 20);
            label4.TabIndex = 9;
            label4.Text = "Criticidad: ";
            // 
            // cmbModulo
            // 
            cmbModulo.FormattingEnabled = true;
            cmbModulo.Location = new Point(126, 171);
            cmbModulo.Name = "cmbModulo";
            cmbModulo.Size = new Size(151, 28);
            cmbModulo.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 171);
            label3.Name = "label3";
            label3.Size = new Size(68, 20);
            label3.TabIndex = 7;
            label3.Text = "Modulo: ";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(126, 121);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(298, 27);
            txtUsuario.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 121);
            label2.Name = "label2";
            label2.Size = new Size(66, 20);
            label2.TabIndex = 5;
            label2.Text = "Usuario: ";
            // 
            // lblFechaHasta
            // 
            lblFechaHasta.AutoSize = true;
            lblFechaHasta.Location = new Point(16, 74);
            lblFechaHasta.Name = "lblFechaHasta";
            lblFechaHasta.Size = new Size(92, 20);
            lblFechaHasta.TabIndex = 4;
            lblFechaHasta.Text = "Fecha Hasta:";
            // 
            // dtpFechaHasta
            // 
            dtpFechaHasta.Location = new Point(126, 74);
            dtpFechaHasta.Name = "dtpFechaHasta";
            dtpFechaHasta.Size = new Size(298, 27);
            dtpFechaHasta.TabIndex = 3;
            // 
            // DtpFechaDesde
            // 
            DtpFechaDesde.Location = new Point(126, 25);
            DtpFechaDesde.Name = "DtpFechaDesde";
            DtpFechaDesde.Size = new Size(298, 27);
            DtpFechaDesde.TabIndex = 2;
            // 
            // lblFechaDesde
            // 
            lblFechaDesde.AutoSize = true;
            lblFechaDesde.Location = new Point(16, 30);
            lblFechaDesde.Name = "lblFechaDesde";
            lblFechaDesde.Size = new Size(100, 20);
            lblFechaDesde.TabIndex = 0;
            lblFechaDesde.Text = "Fecha Desde: ";
            // 
            // dgvEventos
            // 
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEventos.Location = new Point(186, 41);
            dgvEventos.Name = "dgvEventos";
            dgvEventos.ReadOnly = true;
            dgvEventos.RowHeadersWidth = 51;
            dgvEventos.Size = new Size(1014, 271);
            dgvEventos.TabIndex = 2;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(12, 85);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(147, 59);
            btnBuscar.TabIndex = 3;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(12, 180);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(147, 59);
            btnLimpiar.TabIndex = 4;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(1056, 607);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(144, 57);
            btnVolver.TabIndex = 5;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblMensaje
            // 
            lblMensaje.AutoSize = true;
            lblMensaje.Location = new Point(814, 371);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(0, 20);
            lblMensaje.TabIndex = 6;
            // 
            // frmAuditarEventos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(1216, 676);
            Controls.Add(lblMensaje);
            Controls.Add(btnVolver);
            Controls.Add(btnLimpiar);
            Controls.Add(btnBuscar);
            Controls.Add(dgvEventos);
            Controls.Add(gbFiltros);
            Controls.Add(label1);
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
    }
}