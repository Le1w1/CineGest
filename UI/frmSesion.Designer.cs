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
            lblTitulo.Location = new Point(217, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(105, 41);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sesión";
            // 
            // btnCambiarClave
            // 
            btnCambiarClave.Location = new Point(35, 148);
            btnCambiarClave.Margin = new Padding(3, 4, 3, 4);
            btnCambiarClave.Name = "btnCambiarClave";
            btnCambiarClave.Size = new Size(127, 68);
            btnCambiarClave.TabIndex = 1;
            btnCambiarClave.Text = "Cambiar Clave";
            btnCambiarClave.UseVisualStyleBackColor = true;
            btnCambiarClave.Click += btnCambiarClave_Click;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(35, 233);
            btnCerrarSesion.Margin = new Padding(3, 4, 3, 4);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(127, 64);
            btnCerrarSesion.TabIndex = 2;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(206, 341);
            btnVolver.Margin = new Padding(3, 4, 3, 4);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(127, 61);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(170, 69);
            label1.Name = "label1";
            label1.Size = new Size(228, 23);
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
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(546, 463);
            panel1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(253, 199);
            label3.Name = "label3";
            label3.Size = new Size(86, 20);
            label3.TabIndex = 9;
            label3.Text = "Contraseña:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(253, 148);
            label2.Name = "label2";
            label2.Size = new Size(49, 20);
            label2.TabIndex = 8;
            label2.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(349, 148);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(173, 27);
            txtEmail.TabIndex = 7;
            // 
            // txtContraseña
            // 
            txtContraseña.Location = new Point(349, 195);
            txtContraseña.Margin = new Padding(3, 4, 3, 4);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.Size = new Size(173, 27);
            txtContraseña.TabIndex = 6;
            // 
            // btnReLogin
            // 
            btnReLogin.Location = new Point(349, 233);
            btnReLogin.Margin = new Padding(3, 4, 3, 4);
            btnReLogin.Name = "btnReLogin";
            btnReLogin.Size = new Size(127, 64);
            btnReLogin.TabIndex = 5;
            btnReLogin.Text = "Re-Login";
            btnReLogin.UseVisualStyleBackColor = true;
            btnReLogin.Click += btnReLogin_Click;
            // 
            // frmSesion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(546, 463);
            ControlBox = false;
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
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