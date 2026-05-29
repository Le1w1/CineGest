namespace UI
{
    partial class frmCambiarClave
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            txtClaveActual = new TextBox();
            txtNuevaClave = new TextBox();
            chkMostrarClaves = new CheckBox();
            lblMensaje = new Label();
            btnGuardar = new Button();
            btnLimpiar = new Button();
            btnVolver = new Button();
            txtConfirmarClave = new TextBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            errorProviderCambiarClave = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProviderCambiarClave).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 7);
            label1.Name = "label1";
            label1.Size = new Size(84, 15);
            label1.TabIndex = 0;
            label1.Text = "Cambiar Clave";
            // 
            // txtClaveActual
            // 
            txtClaveActual.Location = new Point(192, 51);
            txtClaveActual.Margin = new Padding(3, 2, 3, 2);
            txtClaveActual.Name = "txtClaveActual";
            txtClaveActual.Size = new Size(190, 23);
            txtClaveActual.TabIndex = 1;
            txtClaveActual.UseSystemPasswordChar = true;
            // 
            // txtNuevaClave
            // 
            txtNuevaClave.Location = new Point(192, 76);
            txtNuevaClave.Margin = new Padding(3, 2, 3, 2);
            txtNuevaClave.Name = "txtNuevaClave";
            txtNuevaClave.Size = new Size(190, 23);
            txtNuevaClave.TabIndex = 2;
            txtNuevaClave.UseSystemPasswordChar = true;
            // 
            // chkMostrarClaves
            // 
            chkMostrarClaves.AutoSize = true;
            chkMostrarClaves.Location = new Point(192, 145);
            chkMostrarClaves.Margin = new Padding(3, 2, 3, 2);
            chkMostrarClaves.Name = "chkMostrarClaves";
            chkMostrarClaves.Size = new Size(104, 19);
            chkMostrarClaves.TabIndex = 3;
            chkMostrarClaves.Text = "Mostrar Claves";
            chkMostrarClaves.UseVisualStyleBackColor = true;
            chkMostrarClaves.CheckedChanged += chkMostrarClaves_CheckedChanged;
            // 
            // lblMensaje
            // 
            lblMensaje.AutoSize = true;
            lblMensaje.Location = new Point(52, 291);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(0, 15);
            lblMensaje.TabIndex = 4;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(40, 190);
            btnGuardar.Margin = new Padding(3, 2, 3, 2);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(82, 22);
            btnGuardar.TabIndex = 5;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(149, 190);
            btnLimpiar.Margin = new Padding(3, 2, 3, 2);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(82, 22);
            btnLimpiar.TabIndex = 6;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Location = new Point(258, 190);
            btnVolver.Margin = new Padding(3, 2, 3, 2);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(82, 22);
            btnVolver.TabIndex = 7;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // txtConfirmarClave
            // 
            txtConfirmarClave.Location = new Point(192, 103);
            txtConfirmarClave.Margin = new Padding(3, 2, 3, 2);
            txtConfirmarClave.Name = "txtConfirmarClave";
            txtConfirmarClave.Size = new Size(190, 23);
            txtConfirmarClave.TabIndex = 8;
            txtConfirmarClave.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 51);
            label3.Name = "label3";
            label3.Size = new Size(76, 15);
            label3.TabIndex = 9;
            label3.Text = "Clave Actual:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 78);
            label4.Name = "label4";
            label4.Size = new Size(73, 15);
            label4.TabIndex = 10;
            label4.Text = "Nueva Clave";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 105);
            label5.Name = "label5";
            label5.Size = new Size(96, 15);
            label5.TabIndex = 11;
            label5.Text = "Confirmar Clave:";
            // 
            // errorProviderCambiarClave
            // 
            errorProviderCambiarClave.ContainerControl = this;
            // 
            // frmCambiarClave
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(439, 280);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtConfirmarClave);
            Controls.Add(btnVolver);
            Controls.Add(btnLimpiar);
            Controls.Add(btnGuardar);
            Controls.Add(lblMensaje);
            Controls.Add(chkMostrarClaves);
            Controls.Add(txtNuevaClave);
            Controls.Add(txtClaveActual);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCambiarClave";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CineGest - Cambiar Clave";
            ((System.ComponentModel.ISupportInitialize)errorProviderCambiarClave).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtClaveActual;
        private TextBox txtNuevaClave;
        private CheckBox chkMostrarClaves;
        private Label lblMensaje;
        private Button btnGuardar;
        private Button btnLimpiar;
        private Button btnVolver;
        private TextBox txtConfirmarClave;
        private Label label3;
        private Label label4;
        private Label label5;
        private ErrorProvider errorProviderCambiarClave;
    }
}