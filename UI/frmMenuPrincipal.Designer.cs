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
            reLoginToolStripMenuItem = new ToolStripMenuItem();
            cambiarClaveToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesionToolStripMenuItem = new ToolStripMenuItem();
            empleadoDeBoleteríaToolStripMenuItem = new ToolStripMenuItem();
            gerenciaToolStripMenuItem = new ToolStripMenuItem();
            gerenciaToolStripMenuItem1 = new ToolStripMenuItem();
            mnuAdministrador = new ToolStripMenuItem();
            usuariosToolStripMenuItem = new ToolStripMenuItem();
            bitacoraEventosToolStripMenuItem = new ToolStripMenuItem();
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
            menuPrincipal.ImageScalingSize = new Size(20, 20);
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuSesion, empleadoDeBoleteríaToolStripMenuItem, gerenciaToolStripMenuItem, gerenciaToolStripMenuItem1, mnuAdministrador });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Padding = new Padding(7, 3, 0, 3);
            menuPrincipal.Size = new Size(914, 30);
            menuPrincipal.TabIndex = 0;
            menuPrincipal.Text = "menuStrip1";
            // 
            // mnuSesion
            // 
            mnuSesion.DropDownItems.AddRange(new ToolStripItem[] { reLoginToolStripMenuItem, cambiarClaveToolStripMenuItem, cerrarSesionToolStripMenuItem });
            mnuSesion.Name = "mnuSesion";
            mnuSesion.Size = new Size(73, 24);
            mnuSesion.Text = "Usuario";
            // 
            // reLoginToolStripMenuItem
            // 
            reLoginToolStripMenuItem.Name = "reLoginToolStripMenuItem";
            reLoginToolStripMenuItem.Size = new Size(188, 26);
            reLoginToolStripMenuItem.Text = "Re-Login";
            reLoginToolStripMenuItem.Click += reLoginToolStripMenuItem_Click;
            // 
            // cambiarClaveToolStripMenuItem
            // 
            cambiarClaveToolStripMenuItem.Name = "cambiarClaveToolStripMenuItem";
            cambiarClaveToolStripMenuItem.Size = new Size(188, 26);
            cambiarClaveToolStripMenuItem.Text = "Cambiar Clave";
            cambiarClaveToolStripMenuItem.Click += cambiarClaveToolStripMenuItem_Click;
            // 
            // cerrarSesionToolStripMenuItem
            // 
            cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            cerrarSesionToolStripMenuItem.Size = new Size(188, 26);
            cerrarSesionToolStripMenuItem.Text = "Cerrar Sesion";
            cerrarSesionToolStripMenuItem.Click += cerrarSesionToolStripMenuItem_Click;
            // 
            // empleadoDeBoleteríaToolStripMenuItem
            // 
            empleadoDeBoleteríaToolStripMenuItem.Name = "empleadoDeBoleteríaToolStripMenuItem";
            empleadoDeBoleteríaToolStripMenuItem.Size = new Size(83, 24);
            empleadoDeBoleteríaToolStripMenuItem.Text = "Boletería";
            // 
            // gerenciaToolStripMenuItem
            // 
            gerenciaToolStripMenuItem.Name = "gerenciaToolStripMenuItem";
            gerenciaToolStripMenuItem.Size = new Size(83, 24);
            gerenciaToolStripMenuItem.Text = "Cartelera";
            // 
            // gerenciaToolStripMenuItem1
            // 
            gerenciaToolStripMenuItem1.Name = "gerenciaToolStripMenuItem1";
            gerenciaToolStripMenuItem1.Size = new Size(81, 24);
            gerenciaToolStripMenuItem1.Text = "Gerencia";
            // 
            // mnuAdministrador
            // 
            mnuAdministrador.DropDownItems.AddRange(new ToolStripItem[] { usuariosToolStripMenuItem, bitacoraEventosToolStripMenuItem });
            mnuAdministrador.Name = "mnuAdministrador";
            mnuAdministrador.Size = new Size(118, 24);
            mnuAdministrador.Text = "Administrador";
            // 
            // usuariosToolStripMenuItem
            // 
            usuariosToolStripMenuItem.Name = "usuariosToolStripMenuItem";
            usuariosToolStripMenuItem.Size = new Size(224, 26);
            usuariosToolStripMenuItem.Text = "Usuarios";
            usuariosToolStripMenuItem.Click += usuariosToolStripMenuItem_Click;
            // 
            // bitacoraEventosToolStripMenuItem
            // 
            bitacoraEventosToolStripMenuItem.Name = "bitacoraEventosToolStripMenuItem";
            bitacoraEventosToolStripMenuItem.Size = new Size(224, 26);
            bitacoraEventosToolStripMenuItem.Text = "Bitacora Eventos";
            bitacoraEventosToolStripMenuItem.Click += bitacoraEventosToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblUsuarioSesion, lblEstadoSesion });
            statusStrip1.Location = new Point(0, 574);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(914, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblUsuarioSesion
            // 
            lblUsuarioSesion.Name = "lblUsuarioSesion";
            lblUsuarioSesion.Size = new Size(62, 20);
            lblUsuarioSesion.Text = "Usuario:";
            lblUsuarioSesion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEstadoSesion
            // 
            lblEstadoSesion.Name = "lblEstadoSesion";
            lblEstadoSesion.Size = new Size(835, 20);
            lblEstadoSesion.Spring = true;
            lblEstadoSesion.Text = "Sesión activa";
            lblEstadoSesion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlInicio
            // 
            pnlInicio.Controls.Add(tableLayoutPanel1);
            pnlInicio.Dock = DockStyle.Fill;
            pnlInicio.Location = new Point(0, 30);
            pnlInicio.Margin = new Padding(3, 4, 3, 4);
            pnlInicio.Name = "pnlInicio";
            pnlInicio.Size = new Size(914, 544);
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
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.Size = new Size(914, 544);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // lblDescripcionInicio
            // 
            lblDescripcionInicio.Dock = DockStyle.Fill;
            lblDescripcionInicio.Font = new Font("Segoe UI", 11F);
            lblDescripcionInicio.Location = new Point(3, 244);
            lblDescripcionInicio.Name = "lblDescripcionInicio";
            lblDescripcionInicio.Size = new Size(908, 43);
            lblDescripcionInicio.TabIndex = 1;
            lblDescripcionInicio.Text = "Seleccione un módulo desde el menú superior para comenzar.";
            lblDescripcionInicio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTituloInicio
            // 
            lblTituloInicio.Dock = DockStyle.Fill;
            lblTituloInicio.Font = new Font("Segoe UI", 22F);
            lblTituloInicio.Location = new Point(3, 163);
            lblTituloInicio.Name = "lblTituloInicio";
            lblTituloInicio.Size = new Size(908, 81);
            lblTituloInicio.TabIndex = 0;
            lblTituloInicio.Text = "Bienvenido a Cinegest";
            lblTituloInicio.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(pnlInicio);
            Controls.Add(statusStrip1);
            Controls.Add(menuPrincipal);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuPrincipal;
            Margin = new Padding(3, 4, 3, 4);
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
        private ToolStripMenuItem reLoginToolStripMenuItem;
        private ToolStripMenuItem cambiarClaveToolStripMenuItem;
        private ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private ToolStripMenuItem usuariosToolStripMenuItem;
        private ToolStripMenuItem bitacoraEventosToolStripMenuItem;
    }
}