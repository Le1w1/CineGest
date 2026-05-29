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
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F);
            lblTitulo.Location = new Point(190, 30);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(84, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sesión";
            // 
            // btnCambiarClave
            // 
            btnCambiarClave.Location = new Point(180, 107);
            btnCambiarClave.Name = "btnCambiarClave";
            btnCambiarClave.Size = new Size(111, 51);
            btnCambiarClave.TabIndex = 1;
            btnCambiarClave.Text = "Cambiar Clave";
            btnCambiarClave.UseVisualStyleBackColor = true;
            btnCambiarClave.Click += btnCambiarClave_Click;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(180, 164);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(111, 48);
            btnCerrarSesion.TabIndex = 2;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(180, 218);
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
            label1.Location = new Point(149, 73);
            label1.Name = "label1";
            label1.Size = new Size(183, 19);
            label1.TabIndex = 4;
            label1.Text = "Opciones de la sesión Actual";
            // 
            // panel1
            // 
            panel1.BackColor = Color.PowderBlue;
            panel1.Controls.Add(btnCambiarClave);
            panel1.Controls.Add(lblTitulo);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnVolver);
            panel1.Controls.Add(btnCerrarSesion);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(479, 317);
            panel1.TabIndex = 5;
            // 
            // frmSesion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(479, 317);
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
    }
}