namespace UI
{
    partial class frmAdministrador
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
            lblTitulo = new Label();
            gbFiltros = new GroupBox();
            rbTodos = new RadioButton();
            rbBloqueados = new RadioButton();
            rbActivos = new RadioButton();
            lblCantidadUsuarios = new Label();
            gbDatosUsuario = new GroupBox();
            chkBloqueado = new CheckBox();
            chkActivo = new CheckBox();
            txtNombreUsuario = new TextBox();
            lblNombreUsuario = new Label();
            txtEmail = new TextBox();
            txtDNI = new TextBox();
            lblEmail = new Label();
            lblDNI = new Label();
            txtApellido = new TextBox();
            txtNombre = new TextBox();
            lblApellido = new Label();
            lblNombre = new Label();
            dgvUsuarios = new DataGridView();
            btnCrearUsuario = new Button();
            btnModificarUsuario = new Button();
            btnDesbloquearUsuario = new Button();
            btnActivarDesactivarUsuario = new Button();
            btnLimpiar = new Button();
            btnVolver = new Button();
            lblMensaje = new Label();
            btnAuditarBitacora = new Button();
            gbFiltros.SuspendLayout();
            gbDatosUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 16F);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(890, 35);
            lblTitulo.TabIndex = 11;
            lblTitulo.Text = "ADMINISTRADOR";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbFiltros
            // 
            gbFiltros.BackColor = Color.LightSkyBlue;
            gbFiltros.Controls.Add(rbTodos);
            gbFiltros.Controls.Add(rbBloqueados);
            gbFiltros.Controls.Add(rbActivos);
            gbFiltros.Location = new Point(20, 60);
            gbFiltros.Name = "gbFiltros";
            gbFiltros.Size = new Size(330, 65);
            gbFiltros.TabIndex = 12;
            gbFiltros.TabStop = false;
            gbFiltros.Text = "Filtros de Usuarios";
            // 
            // rbTodos
            // 
            rbTodos.AutoSize = true;
            rbTodos.Location = new Point(235, 28);
            rbTodos.Name = "rbTodos";
            rbTodos.Size = new Size(57, 19);
            rbTodos.TabIndex = 2;
            rbTodos.Text = "Todos";
            rbTodos.UseVisualStyleBackColor = true;
            // 
            // rbBloqueados
            // 
            rbBloqueados.AutoSize = true;
            rbBloqueados.Location = new Point(120, 28);
            rbBloqueados.Name = "rbBloqueados";
            rbBloqueados.Size = new Size(87, 19);
            rbBloqueados.TabIndex = 1;
            rbBloqueados.Text = "Bloqueados";
            rbBloqueados.UseVisualStyleBackColor = true;
            // 
            // rbActivos
            // 
            rbActivos.AutoSize = true;
            rbActivos.Checked = true;
            rbActivos.Location = new Point(15, 28);
            rbActivos.Name = "rbActivos";
            rbActivos.Size = new Size(64, 19);
            rbActivos.TabIndex = 0;
            rbActivos.TabStop = true;
            rbActivos.Text = "Activos";
            rbActivos.UseVisualStyleBackColor = true;
            // 
            // lblCantidadUsuarios
            // 
            lblCantidadUsuarios.Font = new Font("Segoe UI", 10F);
            lblCantidadUsuarios.Location = new Point(650, 80);
            lblCantidadUsuarios.Name = "lblCantidadUsuarios";
            lblCantidadUsuarios.Size = new Size(260, 25);
            lblCantidadUsuarios.TabIndex = 13;
            lblCantidadUsuarios.Text = "Número de Usuarios: 0";
            lblCantidadUsuarios.TextAlign = ContentAlignment.MiddleRight;
            // 
            // gbDatosUsuario
            // 
            gbDatosUsuario.BackColor = Color.LightSkyBlue;
            gbDatosUsuario.Controls.Add(chkBloqueado);
            gbDatosUsuario.Controls.Add(chkActivo);
            gbDatosUsuario.Controls.Add(txtNombreUsuario);
            gbDatosUsuario.Controls.Add(lblNombreUsuario);
            gbDatosUsuario.Controls.Add(txtEmail);
            gbDatosUsuario.Controls.Add(txtDNI);
            gbDatosUsuario.Controls.Add(lblEmail);
            gbDatosUsuario.Controls.Add(lblDNI);
            gbDatosUsuario.Controls.Add(txtApellido);
            gbDatosUsuario.Controls.Add(txtNombre);
            gbDatosUsuario.Controls.Add(lblApellido);
            gbDatosUsuario.Controls.Add(lblNombre);
            gbDatosUsuario.Location = new Point(20, 365);
            gbDatosUsuario.Name = "gbDatosUsuario";
            gbDatosUsuario.Size = new Size(610, 145);
            gbDatosUsuario.TabIndex = 14;
            gbDatosUsuario.TabStop = false;
            gbDatosUsuario.Text = "Datos del Usuario";
            // 
            // chkBloqueado
            // 
            chkBloqueado.Location = new Point(457, 100);
            chkBloqueado.Name = "chkBloqueado";
            chkBloqueado.RightToLeft = RightToLeft.No;
            chkBloqueado.Size = new Size(136, 23);
            chkBloqueado.TabIndex = 12;
            chkBloqueado.Text = "Bloqueado";
            chkBloqueado.UseVisualStyleBackColor = true;
            // 
            // chkActivo
            // 
            chkActivo.Checked = true;
            chkActivo.CheckState = CheckState.Checked;
            chkActivo.Location = new Point(330, 100);
            chkActivo.Name = "chkActivo";
            chkActivo.RightToLeft = RightToLeft.No;
            chkActivo.Size = new Size(108, 23);
            chkActivo.TabIndex = 11;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Location = new Point(150, 97);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(160, 23);
            txtNombreUsuario.TabIndex = 10;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Location = new Point(20, 100);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(120, 23);
            lblNombreUsuario.TabIndex = 8;
            lblNombreUsuario.Text = "Nombre Usuario:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(430, 62);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(150, 23);
            txtEmail.TabIndex = 7;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(150, 62);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(160, 23);
            txtDNI.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(330, 65);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 23);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email:";
            // 
            // lblDNI
            // 
            lblDNI.Location = new Point(20, 65);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(120, 23);
            lblDNI.TabIndex = 4;
            lblDNI.Text = "DNI:";
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(430, 27);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(150, 23);
            txtApellido.TabIndex = 3;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(150, 27);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(160, 23);
            txtNombre.TabIndex = 2;
            // 
            // lblApellido
            // 
            lblApellido.Location = new Point(330, 30);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(100, 23);
            lblApellido.TabIndex = 1;
            lblApellido.Text = "Apellido:";
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(20, 30);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(120, 23);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(20, 140);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(722, 183);
            dgvUsuarios.TabIndex = 1;
            // 
            // btnCrearUsuario
            // 
            btnCrearUsuario.Location = new Point(748, 140);
            btnCrearUsuario.Name = "btnCrearUsuario";
            btnCrearUsuario.Size = new Size(162, 38);
            btnCrearUsuario.TabIndex = 15;
            btnCrearUsuario.Text = "Crear Usuario";
            btnCrearUsuario.UseVisualStyleBackColor = true;
            btnCrearUsuario.Click += btnCrearUsuario_Click;
            // 
            // btnModificarUsuario
            // 
            btnModificarUsuario.Location = new Point(748, 185);
            btnModificarUsuario.Name = "btnModificarUsuario";
            btnModificarUsuario.Size = new Size(162, 38);
            btnModificarUsuario.TabIndex = 16;
            btnModificarUsuario.Text = "Modificar Usuario";
            btnModificarUsuario.UseVisualStyleBackColor = true;
            btnModificarUsuario.Click += btnModificarUsuario_Click;
            // 
            // btnDesbloquearUsuario
            // 
            btnDesbloquearUsuario.Location = new Point(748, 230);
            btnDesbloquearUsuario.Name = "btnDesbloquearUsuario";
            btnDesbloquearUsuario.Size = new Size(162, 38);
            btnDesbloquearUsuario.TabIndex = 17;
            btnDesbloquearUsuario.Text = "Desbloquear Usuario";
            btnDesbloquearUsuario.UseVisualStyleBackColor = true;
            btnDesbloquearUsuario.Click += btnDesbloquearUsuario_Click;
            // 
            // btnActivarDesactivarUsuario
            // 
            btnActivarDesactivarUsuario.Location = new Point(748, 275);
            btnActivarDesactivarUsuario.Name = "btnActivarDesactivarUsuario";
            btnActivarDesactivarUsuario.Size = new Size(162, 38);
            btnActivarDesactivarUsuario.TabIndex = 18;
            btnActivarDesactivarUsuario.Text = "Activar / Desactivar Usuario";
            btnActivarDesactivarUsuario.UseVisualStyleBackColor = true;
            btnActivarDesactivarUsuario.Click += btnActivarDesactivarUsuario_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(680, 410);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(110, 35);
            btnLimpiar.TabIndex = 20;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(800, 410);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 35);
            btnVolver.TabIndex = 21;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click_1;
            // 
            // lblMensaje
            // 
            lblMensaje.ForeColor = Color.DarkRed;
            lblMensaje.Location = new Point(20, 525);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(890, 25);
            lblMensaje.TabIndex = 22;
            lblMensaje.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnAuditarBitacora
            // 
            btnAuditarBitacora.Location = new Point(748, 320);
            btnAuditarBitacora.Name = "btnAuditarBitacora";
            btnAuditarBitacora.Size = new Size(162, 38);
            btnAuditarBitacora.TabIndex = 19;
            btnAuditarBitacora.Text = "Auditar Bitácora";
            btnAuditarBitacora.UseVisualStyleBackColor = true;
            btnAuditarBitacora.Click += btnAuditarBitacora_Click;
            // 
            // frmAdministrador
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(934, 561);
            Controls.Add(lblMensaje);
            Controls.Add(btnVolver);
            Controls.Add(btnLimpiar);
            Controls.Add(btnAuditarBitacora);
            Controls.Add(btnActivarDesactivarUsuario);
            Controls.Add(btnDesbloquearUsuario);
            Controls.Add(btnModificarUsuario);
            Controls.Add(btnCrearUsuario);
            Controls.Add(gbDatosUsuario);
            Controls.Add(lblCantidadUsuarios);
            Controls.Add(gbFiltros);
            Controls.Add(lblTitulo);
            Controls.Add(dgvUsuarios);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAdministrador";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CineGest - Administrador";
            gbFiltros.ResumeLayout(false);
            gbFiltros.PerformLayout();
            gbDatosUsuario.ResumeLayout(false);
            gbDatosUsuario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label lblTitulo;
        private GroupBox gbFiltros;
        private RadioButton rbBloqueados;
        private RadioButton rbActivos;
        private RadioButton rbTodos;
        private Label lblCantidadUsuarios;
        private GroupBox gbDatosUsuario;
        private TextBox txtApellido;
        private TextBox txtNombre;
        private Label lblApellido;
        private Label lblNombre;
        private DataGridView dgvUsuarios;
        private TextBox txtNombreUsuario;
        private Label lblNombreUsuario;
        private TextBox txtEmail;
        private TextBox txtDNI;
        private Label lblEmail;
        private Label lblDNI;
        private CheckBox chkBloqueado;
        private CheckBox chkActivo;
        private Button btnCrearUsuario;
        private Button btnModificarUsuario;
        private Button btnDesbloquearUsuario;
        private Button btnActivarDesactivarUsuario;
        private Button btnLimpiar;
        private Button btnVolver;
        private Label lblMensaje;
        private Button btnAuditarBitacora;
    }
}