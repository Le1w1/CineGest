namespace UI
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblContrasenia = new Label();
            txtContrasenia = new TextBox();
            chkMostrarContrasenia = new CheckBox();
            btnIngresar = new Button();
            btnSalir = new Button();
            btnVolver = new Button();
            cboIdioma = new ComboBox();
            lblMensaje = new Label();
            errorProviderLogin = new ErrorProvider(components);
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProviderLogin).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Segoe UI", 20F);
            lblTitulo.Location = new Point(134, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(136, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "CineGest";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.Font = new Font("Segoe UI", 13F);
            lblSubtitulo.Location = new Point(124, 46);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(146, 34);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Inicio de Sesión";
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(68, 116);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(41, 19);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(69, 138);
            txtEmail.MaxLength = 100;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(242, 25);
            txtEmail.TabIndex = 0;
            // 
            // lblContrasenia
            // 
            lblContrasenia.AutoSize = true;
            lblContrasenia.Location = new Point(69, 184);
            lblContrasenia.Name = "lblContrasenia";
            lblContrasenia.Size = new Size(79, 19);
            lblContrasenia.TabIndex = 4;
            lblContrasenia.Text = "Contraseña";
            // 
            // txtContrasenia
            // 
            txtContrasenia.Location = new Point(69, 206);
            txtContrasenia.MaxLength = 50;
            txtContrasenia.Name = "txtContrasenia";
            txtContrasenia.Size = new Size(242, 25);
            txtContrasenia.TabIndex = 1;
            txtContrasenia.UseSystemPasswordChar = true;
            // 
            // chkMostrarContrasenia
            // 
            chkMostrarContrasenia.AutoSize = true;
            chkMostrarContrasenia.Location = new Point(71, 237);
            chkMostrarContrasenia.Name = "chkMostrarContrasenia";
            chkMostrarContrasenia.Size = new Size(77, 23);
            chkMostrarContrasenia.TabIndex = 2;
            chkMostrarContrasenia.Text = "Mostrar";
            chkMostrarContrasenia.UseVisualStyleBackColor = true;
            chkMostrarContrasenia.CheckedChanged += chkMostrarContrasenia_CheckedChanged;
            // 
            // btnIngresar
            // 
            btnIngresar.Location = new Point(68, 295);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(243, 37);
            btnIngresar.TabIndex = 3;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = true;
            btnIngresar.Click += btnIngresar_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(68, 358);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(243, 36);
            btnSalir.TabIndex = 4;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(68, 358);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(243, 36);
            btnVolver.TabIndex = 5;
            btnVolver.Text = "Volver al Menú Principal";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Visible = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // cboIdioma
            // 
            cboIdioma.DropDownStyle = ComboBoxStyle.DropDownList;
            cboIdioma.Font = new Font("Segoe UI", 9F);
            cboIdioma.FormattingEnabled = true;
            cboIdioma.Location = new Point(294, 12);
            cboIdioma.Name = "cboIdioma";
            cboIdioma.Size = new Size(98, 23);
            cboIdioma.TabIndex = 10;
            cboIdioma.SelectedIndexChanged += cboIdioma_SelectedIndexChanged;
            // 
            // lblMensaje
            // 
            lblMensaje.ForeColor = Color.Red;
            lblMensaje.Location = new Point(68, 419);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(243, 53);
            lblMensaje.TabIndex = 9;
            lblMensaje.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // errorProviderLogin
            // 
            errorProviderLogin.ContainerControl = this;
            // 
            // button1
            // 
            button1.Location = new Point(325, 439);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 11;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // frmLogin
            // 
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            CancelButton = btnSalir;
            ClientSize = new Size(404, 481);
            Controls.Add(button1);
            Controls.Add(lblTitulo);
            Controls.Add(lblMensaje);
            Controls.Add(cboIdioma);
            Controls.Add(btnVolver);
            Controls.Add(btnSalir);
            Controls.Add(btnIngresar);
            Controls.Add(chkMostrarContrasenia);
            Controls.Add(txtContrasenia);
            Controls.Add(lblContrasenia);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(lblSubtitulo);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CineGest - Incio de Sesion";
            ((System.ComponentModel.ISupportInitialize)errorProviderLogin).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblContrasenia;
        private TextBox txtContrasenia;
        private CheckBox chkMostrarContrasenia;
        private Button btnIngresar;
        private Button btnSalir;
        private Button btnVolver;
        private ComboBox cboIdioma;
        private Label lblMensaje;
        private ErrorProvider errorProviderLogin;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private Button button1;
    }
}
