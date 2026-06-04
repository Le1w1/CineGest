namespace UI
{
    partial class frmMenuPrincipal
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
            menuPrincipal = new MenuStrip();
            mnuSesion = new ToolStripMenuItem();
            empleadoDeBoleteríaToolStripMenuItem = new ToolStripMenuItem();
            gerenciaToolStripMenuItem = new ToolStripMenuItem();
            gerenciaToolStripMenuItem1 = new ToolStripMenuItem();
            mnuAdministrador = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            lblUsuarioSesion = new ToolStripStatusLabel();
            lblEstadoSesion = new ToolStripStatusLabel();
            pnlInicio = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblDescripcionInicio = new Label();
            lblTituloInicio = new Label();
            menuPrincipal.SuspendLayout();
            statusStrip1.SuspendLayout();
            pnlInicio.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // menuPrincipal
            // 
            menuPrincipal.BackColor = Color.LightSkyBlue;
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuSesion, empleadoDeBoleteríaToolStripMenuItem, gerenciaToolStripMenuItem, gerenciaToolStripMenuItem1, mnuAdministrador });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(800, 24);
            menuPrincipal.TabIndex = 0;
            menuPrincipal.Text = "menuStrip1";
            // 
            // mnuSesion
            // 
            mnuSesion.Name = "mnuSesion";
            mnuSesion.Size = new Size(53, 20);
            mnuSesion.Text = "Sesión";
            mnuSesion.Click += mnuSesion_Click;
            // 
            // empleadoDeBoleteríaToolStripMenuItem
            // 
            empleadoDeBoleteríaToolStripMenuItem.Name = "empleadoDeBoleteríaToolStripMenuItem";
            empleadoDeBoleteríaToolStripMenuItem.Size = new Size(65, 20);
            empleadoDeBoleteríaToolStripMenuItem.Text = "Boletería";
            // 
            // gerenciaToolStripMenuItem
            // 
            gerenciaToolStripMenuItem.Name = "gerenciaToolStripMenuItem";
            gerenciaToolStripMenuItem.Size = new Size(66, 20);
            gerenciaToolStripMenuItem.Text = "Cartelera";
            // 
            // gerenciaToolStripMenuItem1
            // 
            gerenciaToolStripMenuItem1.Name = "gerenciaToolStripMenuItem1";
            gerenciaToolStripMenuItem1.Size = new Size(65, 20);
            gerenciaToolStripMenuItem1.Text = "Gerencia";
            // 
            // mnuAdministrador
            // 
            mnuAdministrador.Name = "mnuAdministrador";
            mnuAdministrador.Size = new Size(95, 20);
            mnuAdministrador.Text = "Administrador";
            mnuAdministrador.Click += administradorToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblUsuarioSesion, lblEstadoSesion });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblUsuarioSesion
            // 
            lblUsuarioSesion.Name = "lblUsuarioSesion";
            lblUsuarioSesion.Size = new Size(50, 17);
            lblUsuarioSesion.Text = "Usuario:";
            lblUsuarioSesion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEstadoSesion
            // 
            lblEstadoSesion.Name = "lblEstadoSesion";
            lblEstadoSesion.Size = new Size(735, 17);
            lblEstadoSesion.Spring = true;
            lblEstadoSesion.Text = "Sesión activa";
            lblEstadoSesion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlInicio
            // 
            pnlInicio.Controls.Add(tableLayoutPanel1);
            pnlInicio.Dock = DockStyle.Fill;
            pnlInicio.Location = new Point(0, 24);
            pnlInicio.Name = "pnlInicio";
            pnlInicio.Size = new Size(800, 404);
            pnlInicio.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.PowderBlue;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblDescripcionInicio, 0, 2);
            tableLayoutPanel1.Controls.Add(lblTituloInicio, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.Size = new Size(800, 404);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // lblDescripcionInicio
            // 
            lblDescripcionInicio.Dock = DockStyle.Fill;
            lblDescripcionInicio.Font = new Font("Segoe UI", 11F);
            lblDescripcionInicio.Location = new Point(3, 181);
            lblDescripcionInicio.Name = "lblDescripcionInicio";
            lblDescripcionInicio.Size = new Size(794, 32);
            lblDescripcionInicio.TabIndex = 1;
            lblDescripcionInicio.Text = "Seleccione un módulo desde el menú superior para comenzar.";
            lblDescripcionInicio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTituloInicio
            // 
            lblTituloInicio.Dock = DockStyle.Fill;
            lblTituloInicio.Font = new Font("Segoe UI", 22F);
            lblTituloInicio.Location = new Point(3, 121);
            lblTituloInicio.Name = "lblTituloInicio";
            lblTituloInicio.Size = new Size(794, 60);
            lblTituloInicio.TabIndex = 0;
            lblTituloInicio.Text = "Bienvenido a Cinegest";
            lblTituloInicio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlInicio);
            Controls.Add(statusStrip1);
            Controls.Add(menuPrincipal);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuPrincipal;
            Name = "frmMenuPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CineGest - Menú Principal";
            WindowState = FormWindowState.Maximized;
            Load += frmMenuPrincipal_Load;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            pnlInicio.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem mnuSesion;
        private ToolStripMenuItem empleadoDeBoleteríaToolStripMenuItem;
        private ToolStripMenuItem gerenciaToolStripMenuItem;
        private ToolStripMenuItem gerenciaToolStripMenuItem1;
        private ToolStripMenuItem mnuAdministrador;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblUsuarioSesion;
        private ToolStripStatusLabel lblEstadoSesion;
        private Panel pnlInicio;
        private Label lblDescripcionInicio;
        private Label lblTituloInicio;
        private TableLayoutPanel tableLayoutPanel1;
    }
}