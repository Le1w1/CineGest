namespace UI
{
    partial class frmCambiarIdioma
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
            lblIdioma = new Label();
            cboIdiomas = new ComboBox();
            btnGuardar = new Button();
            btnVolver = new Button();
            lblMensaje = new Label();
            SuspendLayout();
            // 
            // lblIdioma
            // 
            lblIdioma.AutoSize = true;
            lblIdioma.Font = new Font("Segoe UI", 10F);
            lblIdioma.Location = new Point(30, 35);
            lblIdioma.Name = "lblIdioma";
            lblIdioma.Size = new Size(54, 19);
            lblIdioma.TabIndex = 0;
            lblIdioma.Text = "Idioma:";
            // 
            // cboIdiomas
            // 
            cboIdiomas.DropDownStyle = ComboBoxStyle.DropDownList;
            cboIdiomas.Font = new Font("Segoe UI", 10F);
            cboIdiomas.FormattingEnabled = true;
            cboIdiomas.Location = new Point(110, 32);
            cboIdiomas.Name = "cboIdiomas";
            cboIdiomas.Size = new Size(240, 25);
            cboIdiomas.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = SystemColors.ButtonHighlight;
            btnGuardar.Font = new Font("Segoe UI", 10F);
            btnGuardar.Location = new Point(110, 110);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 35);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnVolver
            // 
            btnVolver.Font = new Font("Segoe UI", 10F);
            btnVolver.Location = new Point(240, 110);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 35);
            btnVolver.TabIndex = 3;
            btnVolver.Text = "Volver";
            btnVolver.UseVisualStyleBackColor = true;
            btnVolver.Click += btnVolver_Click;
            // 
            // lblMensaje
            // 
            lblMensaje.AutoSize = true;
            lblMensaje.Font = new Font("Segoe UI", 9F);
            lblMensaje.ForeColor = Color.DarkRed;
            lblMensaje.Location = new Point(30, 75);
            lblMensaje.Name = "lblMensaje";
            lblMensaje.Size = new Size(0, 15);
            lblMensaje.TabIndex = 4;
            // 
            // frmCambiarIdioma
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(400, 180);
            Controls.Add(lblMensaje);
            Controls.Add(btnVolver);
            Controls.Add(btnGuardar);
            Controls.Add(cboIdiomas);
            Controls.Add(lblIdioma);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCambiarIdioma";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cambiar Idioma";
            FormClosed += frmCambiarIdioma_FormClosed;
            Load += frmCambiarIdioma_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblIdioma;
        private ComboBox cboIdiomas;
        private Button btnGuardar;
        private Button btnVolver;
        private Label lblMensaje;
    }
}
