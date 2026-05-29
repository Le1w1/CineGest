namespace UI
{
    partial class frmSesion
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
            btnCambiarClave = new Button();
            btnCerrarSesion = new Button();
            btnVolver = new Button();
            label1 = new Label();
            panel1 = new Panel();
            label3 = new Label();
            label2 = new Label();
            txtEmail = new TextBox();
            txtContraseña = new TextBox();
            btnReLogin = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F);
            lblTitulo.Location = new Point(190, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(84, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sesión";
            // 
            // btnCambiarClave
            // 
            btnCambiarClave.Location = new Point(31, 111);
            btnCambiarClave.Name = "btnCambiarClave";
            btnCambiarClave.Size = new Size(111, 51);
            btnCambiarClave.TabIndex = 1;
            btnCambiarClave.Text = "Cambiar Clave";
            btnCambiarClave.UseVisualStyleBackColor = true;
            btnCambiarClave.Click += btnCambiarClave_Click;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(31, 175);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(111, 48);
            btnCerrarSesion.TabIndex = 2;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(180, 256);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(111, 46);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(149, 52);
            label1.Name = "label1";
            label1.Size = new Size(183, 19);
            label1.TabIndex = 4;
            label1.Text = "Opciones de la sesión Actual";
            // 
            // panel1
            // 
            panel1.BackColor = Color.PowderBlue;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(txtContraseña);
            panel1.Controls.Add(btnReLogin);
            panel1.Controls.Add(btnCambiarClave);
            panel1.Controls.Add(lblTitulo);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnVolver);
            panel1.Controls.Add(btnCerrarSesion);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(478, 323);
            panel1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(252, 149);
            label3.Name = "label3";
            label3.Size = new Size(70, 15);
            label3.TabIndex = 9;
            label3.Text = "Contraseña:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(252, 114);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 8;
            label2.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(346, 111);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(111, 23);
            txtEmail.TabIndex = 7;
            // 
            // txtContraseña
            // 
            txtContraseña.Location = new Point(346, 146);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.Size = new Size(111, 23);
            txtContraseña.TabIndex = 6;
            // 
            // btnReLogin
            // 
            btnReLogin.Location = new Point(346, 175);
            btnReLogin.Name = "btnReLogin";
            btnReLogin.Size = new Size(111, 48);
            btnReLogin.TabIndex = 5;
            btnReLogin.Text = "Re-Login";
            btnReLogin.UseVisualStyleBackColor = true;
            btnReLogin.Click += btnReLogin_Click;
            // 
            // frmSesion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 323);
            ControlBox = false;
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSesion";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CineGest - Sesión";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private Button btnCambiarClave;
        private Button btnCerrarSesion;
        private Button btnVolver;
        private Label label1;
        private Panel panel1;
        private Button btnReLogin;
        private TextBox txtEmail;
        private TextBox txtContraseña;
        private Label label3;
        private Label label2;
    }
}