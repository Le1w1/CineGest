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
            txtNombreUsuario = new TextBox();
            lblNombreUsuario = new Label();
            chkActivo = new CheckBox();
            txtEmail = new TextBox();
            txtDNI = new TextBox();
            lblEmail = new Label();
            lblDNI = new Label();
            txtApellido = new TextBox();
            txtNombre = new TextBox();
            lblApellido = new Label();
            lblNombre = new Label();
            lblRol = new Label();
            cboRol = new ComboBox();
            dgvUsuarios = new DataGridView();
            btnCrearUsuario = new Button();
            btnModificarUsuario = new Button();
            btnDesbloquearUsuario = new Button();
            btnActivarDesactivarUsuario = new Button();
            btnLimpiar = new Button();
            btnVolver = new Button();
            lblMensaje = new Label();
            gbFiltros.SuspendLayout();
            gbDatosUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 16F);
            lblTitulo.Location = new Point(23, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1017, 47);
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
            gbFiltros.Location = new Point(23, 80);
            gbFiltros.Margin = new Padding(3, 4, 3, 4);
            gbFiltros.Name = "gbFiltros";
            gbFiltros.Padding = new Padding(3, 4, 3, 4);
            gbFiltros.Size = new Size(377, 87);
            gbFiltros.TabIndex = 12;
            gbFiltros.TabStop = false;
            gbFiltros.Text = "Filtros de Usuarios";
            // 
            // rbTodos
            // 
            rbTodos.AutoSize = true;
            rbTodos.Location = new Point(269, 37);
            rbTodos.Margin = new Padding(3, 4, 3, 4);
            rbTodos.Name = "rbTodos";
            rbTodos.Size = new Size(70, 24);
            rbTodos.TabIndex = 2;
            rbTodos.Text = "Todos";
            rbTodos.UseVisualStyleBackColor = true;
            rbTodos.CheckedChanged += rbFiltros_CheckedChanged;
            // 
            // rbBloqueados
            // 
            rbBloqueados.AutoSize = true;
            rbBloqueados.Location = new Point(137, 37);
            rbBloqueados.Margin = new Padding(3, 4, 3, 4);
            rbBloqueados.Name = "rbBloqueados";
            rbBloqueados.Size = new Size(109, 24);
            rbBloqueados.TabIndex = 1;
            rbBloqueados.Text = "Bloqueados";
            rbBloqueados.UseVisualStyleBackColor = true;
            rbBloqueados.CheckedChanged += rbFiltros_CheckedChanged;
            // 
            // rbActivos
            // 
            rbActivos.AutoSize = true;
            rbActivos.Checked = true;
            rbActivos.Location = new Point(17, 37);
            rbActivos.Margin = new Padding(3, 4, 3, 4);
            rbActivos.Name = "rbActivos";
            rbActivos.Size = new Size(78, 24);
            rbActivos.TabIndex = 0;
            rbActivos.TabStop = true;
            rbActivos.Text = "Activos";
            rbActivos.UseVisualStyleBackColor = true;
            rbActivos.CheckedChanged += rbFiltros_CheckedChanged;
            // 
            // lblCantidadUsuarios
            // 
            lblCantidadUsuarios.Font = new Font("Segoe UI", 10F);
            lblCantidadUsuarios.Location = new Point(743, 107);
            lblCantidadUsuarios.Name = "lblCantidadUsuarios";
            lblCantidadUsuarios.Size = new Size(297, 33);
            lblCantidadUsuarios.TabIndex = 13;
            lblCantidadUsuarios.Text = "Número de Usuarios: 0";
            lblCantidadUsuarios.TextAlign = ContentAlignment.MiddleRight;
            // 
            // gbDatosUsuario
            // 
            gbDatosUsuario.BackColor = Color.LightSkyBlue;
            gbDatosUsuario.Controls.Add(lblRol);
            gbDatosUsuario.Controls.Add(cboRol);
            gbDatosUsuario.Controls.Add(txtNombreUsuario);
            gbDatosUsuario.Controls.Add(lblNombreUsuario);
            gbDatosUsuario.Controls.Add(chkActivo);
            gbDatosUsuario.Controls.Add(txtEmail);
            gbDatosUsuario.Controls.Add(txtDNI);
            gbDatosUsuario.Controls.Add(lblEmail);
            gbDatosUsuario.Controls.Add(lblDNI);
            gbDatosUsuario.Controls.Add(txtApellido);
            gbDatosUsuario.Controls.Add(txtNombre);
            gbDatosUsuario.Controls.Add(lblApellido);
            gbDatosUsuario.Controls.Add(lblNombre);
            gbDatosUsuario.Location = new Point(23, 487);
            gbDatosUsuario.Margin = new Padding(3, 4, 3, 4);
            gbDatosUsuario.Name = "gbDatosUsuario";
            gbDatosUsuario.Padding = new Padding(3, 4, 3, 4);
            gbDatosUsuario.Size = new Size(697, 250);
            gbDatosUsuario.TabIndex = 14;
            gbDatosUsuario.TabStop = false;
            gbDatosUsuario.Text = "Datos del Usuario";
            //
            // lblRol
            //
            lblRol.Location = new Point(23, 188);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(137, 31);
            lblRol.TabIndex = 15;
            lblRol.Text = "Rol:";
            //
            // cboRol
            //
            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Font = new Font("Segoe UI", 10F);
            cboRol.FormattingEnabled = true;
            cboRol.Location = new Point(171, 185);
            cboRol.Name = "cboRol";
            cboRol.Size = new Size(255, 27);
            cboRol.TabIndex = 16;
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Location = new Point(171, 133);
            txtNombreUsuario.Margin = new Padding(3, 4, 3, 4);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(182, 27);
            txtNombreUsuario.TabIndex = 14;
            // 
            // lblNombreUsuario
            // 
            lblNombreUsuario.Location = new Point(23, 137);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Size = new Size(137, 31);
            lblNombreUsuario.TabIndex = 13;
            lblNombreUsuario.Text = "Nombre de Usuario:";
            // 
            // chkActivo
            // 
            chkActivo.Checked = true;
            chkActivo.CheckState = CheckState.Checked;
            chkActivo.Location = new Point(491, 133);
            chkActivo.Margin = new Padding(3, 4, 3, 4);
            chkActivo.Name = "chkActivo";
            chkActivo.RightToLeft = RightToLeft.No;
            chkActivo.Size = new Size(123, 31);
            chkActivo.TabIndex = 11;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(491, 83);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(171, 27);
            txtEmail.TabIndex = 7;
            // 
            // txtDNI
            // 
            txtDNI.Location = new Point(171, 83);
            txtDNI.Margin = new Padding(3, 4, 3, 4);
            txtDNI.Name = "txtDNI";
            txtDNI.Size = new Size(182, 27);
            txtDNI.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(377, 87);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(114, 31);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email:";
            // 
            // lblDNI
            // 
            lblDNI.Location = new Point(23, 87);
            lblDNI.Name = "lblDNI";
            lblDNI.Size = new Size(137, 31);
            lblDNI.TabIndex = 4;
            lblDNI.Text = "DNI:";
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(491, 36);
            txtApellido.Margin = new Padding(3, 4, 3, 4);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(171, 27);
            txtApellido.TabIndex = 3;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(171, 36);
            txtNombre.Margin = new Padding(3, 4, 3, 4);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(182, 27);
            txtNombre.TabIndex = 2;
            // 
            // lblApellido
            // 
            lblApellido.Location = new Point(377, 40);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(114, 31);
            lblApellido.TabIndex = 1;
            lblApellido.Text = "Apellido:";
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(23, 40);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(137, 31);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeColumns = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(23, 203);
            dgvUsuarios.Margin = new Padding(3, 4, 3, 4);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowHeadersWidth = 51;
            dgvUsuarios.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(825, 228);
            dgvUsuarios.TabIndex = 1;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            // 
            // btnCrearUsuario
            // 
            btnCrearUsuario.Location = new Point(854, 203);
            btnCrearUsuario.Margin = new Padding(3, 4, 3, 4);
            btnCrearUsuario.Name = "btnCrearUsuario";
            btnCrearUsuario.Size = new Size(185, 51);
            btnCrearUsuario.TabIndex = 15;
            btnCrearUsuario.Text = "Crear Usuario";
            btnCrearUsuario.UseVisualStyleBackColor = true;
            btnCrearUsuario.Click += btnCrearUsuario_Click;
            // 
            // btnModificarUsuario
            // 
            btnModificarUsuario.Location = new Point(855, 262);
            btnModificarUsuario.Margin = new Padding(3, 4, 3, 4);
            btnModificarUsuario.Name = "btnModificarUsuario";
            btnModificarUsuario.Size = new Size(185, 51);
            btnModificarUsuario.TabIndex = 16;
            btnModificarUsuario.Text = "Modificar Usuario";
            btnModificarUsuario.UseVisualStyleBackColor = true;
            btnModificarUsuario.Click += btnModificarUsuario_Click;
            // 
            // btnDesbloquearUsuario
            // 
            btnDesbloquearUsuario.Location = new Point(854, 321);
            btnDesbloquearUsuario.Margin = new Padding(3, 4, 3, 4);
            btnDesbloquearUsuario.Name = "btnDesbloquearUsuario";
            btnDesbloquearUsuario.Size = new Size(185, 51);
            btnDesbloquearUsuario.TabIndex = 17;
            btnDesbloquearUsuario.Text = "Desbloquear Usuario";
            btnDesbloquearUsuario.UseVisualStyleBackColor = true;
            btnDesbloquearUsuario.Click += btnDesbloquearUsuario_Click;
            // 
            // btnActivarDesactivarUsuario
            // 
            btnActivarDesactivarUsuario.Location = new Point(854, 380);
            btnActivarDesactivarUsuario.Margin = new Padding(3, 4, 3, 4);
            btnActivarDesactivarUsuario.Name = "btnActivarDesactivarUsuario";
            btnActivarDesactivarUsuario.Size = new Size(185, 51);
            btnActivarDesactivarUsuario.TabIndex = 18;
            btnActivarDesactivarUsuario.Text = "Activar / Desactivar Usuario";
            btnActivarDesactivarUsuario.UseVisualStyleBackColor = true;
            btnActivarDesactivarUsuario.Click += btnActivarDesactivarUsuario_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(777, 547);
            btnLimpiar.Margin = new Padding(3, 4, 3, 4);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(126, 47);
            btnLimpiar.TabIndex = 20;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(914, 547);
            btnVolver.Margin = new Padding(3, 4, 3, 4);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(126, 47);
            btnVolver.TabIndex = 21;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click_1;
            // 
            // lblMensaje
            // 
            lblMensaje.ForeColor = Color.DarkRed;
            lblMensaje.Location = new Point(23, 700);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(1017, 33);
            lblMensaje.TabIndex = 22;
            lblMensaje.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmAdministrador
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(1067, 748);
            Controls.Add(lblMensaje);
            Controls.Add(btnVolver);
            Controls.Add(btnLimpiar);
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
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
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
        private TextBox txtEmail;
        private TextBox txtDNI;
        private Label lblEmail;
        private Label lblDNI;
        private CheckBox chkActivo;
        private Button btnCrearUsuario;
        private Button btnModificarUsuario;
        private Button btnDesbloquearUsuario;
        private Button btnActivarDesactivarUsuario;
        private Button btnLimpiar;
        private Button btnVolver;
        private Label lblMensaje;
        private TextBox txtNombreUsuario;
        private Label lblNombreUsuario;
        private Label lblRol;
        private ComboBox cboRol;
    }
}