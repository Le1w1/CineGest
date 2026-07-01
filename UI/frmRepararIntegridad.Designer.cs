namespace UI
{
    partial class frmRepararIntegridad
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRepararIntegridad));
            lblTitulo = new Label();
            lblExplicacion = new Label();
            dgvInconsistencias = new DataGridView();
            lblAdvertenciaGeneral = new Label();
            lblAdvertenciaRecalcular = new Label();
            btnRestaurar = new Button();
            btnRecalcular = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvInconsistencias).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = SystemColors.ActiveCaptionText;
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(238, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reparación de Integridad";
            // 
            // lblExplicacion
            // 
            lblExplicacion.Font = new Font("Segoe UI", 9F);
            lblExplicacion.Location = new Point(20, 50);
            lblExplicacion.Name = "lblExplicacion";
            lblExplicacion.Size = new Size(700, 40);
            lblExplicacion.TabIndex = 1;
            lblExplicacion.Text = "El sistema detectó las siguientes inconsistencias en la base de datos. Debe reparar el sistema antes de continuar operando.";
            // 
            // dgvInconsistencias
            // 
            dgvInconsistencias.AllowUserToAddRows = false;
            dgvInconsistencias.AllowUserToDeleteRows = false;
            dgvInconsistencias.AllowUserToResizeRows = false;
            dgvInconsistencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInconsistencias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInconsistencias.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvInconsistencias.Font = new Font("Segoe UI", 9F);
            dgvInconsistencias.Location = new Point(20, 95);
            dgvInconsistencias.MultiSelect = false;
            dgvInconsistencias.Name = "dgvInconsistencias";
            dgvInconsistencias.ReadOnly = true;
            dgvInconsistencias.RowHeadersVisible = false;
            dgvInconsistencias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInconsistencias.Size = new Size(700, 200);
            dgvInconsistencias.TabIndex = 2;
            // 
            // lblAdvertenciaGeneral
            // 
            lblAdvertenciaGeneral.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAdvertenciaGeneral.Location = new Point(20, 310);
            lblAdvertenciaGeneral.Name = "lblAdvertenciaGeneral";
            lblAdvertenciaGeneral.Size = new Size(700, 40);
            lblAdvertenciaGeneral.TabIndex = 3;
            lblAdvertenciaGeneral.Text = "Elija UNA opción de reparación. La aplicación se reiniciará al finalizar. Los cambios no realizados a través del sistema no serán recuperables.";
            // 
            // lblAdvertenciaRecalcular
            // 
            lblAdvertenciaRecalcular.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblAdvertenciaRecalcular.ForeColor = Color.Firebrick;
            lblAdvertenciaRecalcular.Location = new Point(20, 360);
            lblAdvertenciaRecalcular.Name = "lblAdvertenciaRecalcular";
            lblAdvertenciaRecalcular.Size = new Size(700, 55);
            lblAdvertenciaRecalcular.TabIndex = 4;
            lblAdvertenciaRecalcular.Text = resources.GetString("lblAdvertenciaRecalcular.Text");
            // 
            // btnRestaurar
            // 
            btnRestaurar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRestaurar.Location = new Point(20, 430);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(220, 45);
            btnRestaurar.TabIndex = 5;
            btnRestaurar.Text = "Restaurar Backup";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            // 
            // btnRecalcular
            // 
            btnRecalcular.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRecalcular.ForeColor = Color.Firebrick;
            btnRecalcular.Location = new Point(260, 430);
            btnRecalcular.Name = "btnRecalcular";
            btnRecalcular.Size = new Size(280, 45);
            btnRecalcular.TabIndex = 6;
            btnRecalcular.Text = "Recalcular Dígitos Verificadores";
            btnRecalcular.UseVisualStyleBackColor = true;
            btnRecalcular.Click += btnRecalcular_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Font = new Font("Segoe UI", 10F);
            btnCancelar.Location = new Point(620, 430);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(100, 45);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // frmRepararIntegridad
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PowderBlue;
            ClientSize = new Size(740, 500);
            ControlBox = false;
            Controls.Add(lblTitulo);
            Controls.Add(lblExplicacion);
            Controls.Add(dgvInconsistencias);
            Controls.Add(lblAdvertenciaGeneral);
            Controls.Add(lblAdvertenciaRecalcular);
            Controls.Add(btnRestaurar);
            Controls.Add(btnRecalcular);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmRepararIntegridad";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reparación de Integridad";
            ((System.ComponentModel.ISupportInitialize)dgvInconsistencias).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblExplicacion;
        private System.Windows.Forms.DataGridView dgvInconsistencias;
        private System.Windows.Forms.Label lblAdvertenciaGeneral;
        private System.Windows.Forms.Label lblAdvertenciaRecalcular;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnRecalcular;
        private System.Windows.Forms.Button btnCancelar;
    }
}