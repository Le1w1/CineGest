namespace UI
{
    partial class frmGestionarRespaldo
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
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
            this.lblTitulo = new System.Windows.Forms.Label();

            // Grupo Backup
            this.gbBackup = new System.Windows.Forms.GroupBox();
            this.lblCarpetaDestino = new System.Windows.Forms.Label();
            this.txtCarpetaDestino = new System.Windows.Forms.TextBox();
            this.btnExaminarCarpeta = new System.Windows.Forms.Button();
            this.lblDescripcionBackup = new System.Windows.Forms.Label();
            this.txtDescripcionBackup = new System.Windows.Forms.TextBox();
            this.btnRealizarBackup = new System.Windows.Forms.Button();

            // Grupo Restore
            this.gbRestore = new System.Windows.Forms.GroupBox();
            this.lblArchivoBak = new System.Windows.Forms.Label();
            this.txtArchivoBak = new System.Windows.Forms.TextBox();
            this.btnExaminarArchivo = new System.Windows.Forms.Button();
            this.lblDescripcionRestore = new System.Windows.Forms.Label();
            this.txtDescripcionRestore = new System.Windows.Forms.TextBox();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.lblAdvertenciaRestore = new System.Windows.Forms.Label();

            this.gbBackup.SuspendLayout();
            this.gbRestore.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(210, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de Respaldo";

            // ==================== GRUPO BACKUP ====================

            // 
            // gbBackup
            // 
            this.gbBackup.Controls.Add(this.lblCarpetaDestino);
            this.gbBackup.Controls.Add(this.txtCarpetaDestino);
            this.gbBackup.Controls.Add(this.btnExaminarCarpeta);
            this.gbBackup.Controls.Add(this.lblDescripcionBackup);
            this.gbBackup.Controls.Add(this.txtDescripcionBackup);
            this.gbBackup.Controls.Add(this.btnRealizarBackup);
            this.gbBackup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbBackup.Location = new System.Drawing.Point(20, 55);
            this.gbBackup.Name = "gbBackup";
            this.gbBackup.Size = new System.Drawing.Size(520, 210);
            this.gbBackup.TabIndex = 1;
            this.gbBackup.TabStop = false;
            this.gbBackup.Text = "Backup";

            // 
            // lblCarpetaDestino
            // 
            this.lblCarpetaDestino.AutoSize = true;
            this.lblCarpetaDestino.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCarpetaDestino.Location = new System.Drawing.Point(15, 35);
            this.lblCarpetaDestino.Name = "lblCarpetaDestino";
            this.lblCarpetaDestino.Size = new System.Drawing.Size(100, 15);
            this.lblCarpetaDestino.TabIndex = 0;
            this.lblCarpetaDestino.Text = "Carpeta destino:";

            // 
            // txtCarpetaDestino
            // 
            this.txtCarpetaDestino.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCarpetaDestino.Location = new System.Drawing.Point(15, 55);
            this.txtCarpetaDestino.Name = "txtCarpetaDestino";
            this.txtCarpetaDestino.ReadOnly = true;
            this.txtCarpetaDestino.Size = new System.Drawing.Size(380, 23);
            this.txtCarpetaDestino.TabIndex = 1;

            // 
            // btnExaminarCarpeta
            // 
            this.btnExaminarCarpeta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExaminarCarpeta.Location = new System.Drawing.Point(405, 54);
            this.btnExaminarCarpeta.Name = "btnExaminarCarpeta";
            this.btnExaminarCarpeta.Size = new System.Drawing.Size(100, 25);
            this.btnExaminarCarpeta.TabIndex = 2;
            this.btnExaminarCarpeta.Text = "Examinar...";
            this.btnExaminarCarpeta.UseVisualStyleBackColor = true;
            this.btnExaminarCarpeta.Click += new System.EventHandler(this.btnExaminarCarpeta_Click);

            // 
            // lblDescripcionBackup
            // 
            this.lblDescripcionBackup.AutoSize = true;
            this.lblDescripcionBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescripcionBackup.Location = new System.Drawing.Point(15, 95);
            this.lblDescripcionBackup.Name = "lblDescripcionBackup";
            this.lblDescripcionBackup.Size = new System.Drawing.Size(150, 15);
            this.lblDescripcionBackup.TabIndex = 3;
            this.lblDescripcionBackup.Text = "Descripción (opcional):";

            // 
            // txtDescripcionBackup
            // 
            this.txtDescripcionBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescripcionBackup.Location = new System.Drawing.Point(15, 115);
            this.txtDescripcionBackup.MaxLength = 500;
            this.txtDescripcionBackup.Name = "txtDescripcionBackup";
            this.txtDescripcionBackup.Size = new System.Drawing.Size(490, 23);
            this.txtDescripcionBackup.TabIndex = 4;

            // 
            // btnRealizarBackup
            // 
            this.btnRealizarBackup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRealizarBackup.Location = new System.Drawing.Point(345, 155);
            this.btnRealizarBackup.Name = "btnRealizarBackup";
            this.btnRealizarBackup.Size = new System.Drawing.Size(160, 35);
            this.btnRealizarBackup.TabIndex = 5;
            this.btnRealizarBackup.Text = "Realizar Backup";
            this.btnRealizarBackup.UseVisualStyleBackColor = true;
            this.btnRealizarBackup.Click += new System.EventHandler(this.btnRealizarBackup_Click);

            // ==================== GRUPO RESTORE ====================

            // 
            // gbRestore
            // 
            this.gbRestore.Controls.Add(this.lblArchivoBak);
            this.gbRestore.Controls.Add(this.txtArchivoBak);
            this.gbRestore.Controls.Add(this.btnExaminarArchivo);
            this.gbRestore.Controls.Add(this.lblDescripcionRestore);
            this.gbRestore.Controls.Add(this.txtDescripcionRestore);
            this.gbRestore.Controls.Add(this.lblAdvertenciaRestore);
            this.gbRestore.Controls.Add(this.btnRestaurar);
            this.gbRestore.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbRestore.Location = new System.Drawing.Point(20, 275);
            this.gbRestore.Name = "gbRestore";
            this.gbRestore.Size = new System.Drawing.Size(520, 260);
            this.gbRestore.TabIndex = 2;
            this.gbRestore.TabStop = false;
            this.gbRestore.Text = "Restore";

            // 
            // lblArchivoBak
            // 
            this.lblArchivoBak.AutoSize = true;
            this.lblArchivoBak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblArchivoBak.Location = new System.Drawing.Point(15, 35);
            this.lblArchivoBak.Name = "lblArchivoBak";
            this.lblArchivoBak.Size = new System.Drawing.Size(80, 15);
            this.lblArchivoBak.TabIndex = 0;
            this.lblArchivoBak.Text = "Archivo .bak:";

            // 
            // txtArchivoBak
            // 
            this.txtArchivoBak.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtArchivoBak.Location = new System.Drawing.Point(15, 55);
            this.txtArchivoBak.Name = "txtArchivoBak";
            this.txtArchivoBak.ReadOnly = true;
            this.txtArchivoBak.Size = new System.Drawing.Size(380, 23);
            this.txtArchivoBak.TabIndex = 1;

            // 
            // btnExaminarArchivo
            // 
            this.btnExaminarArchivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExaminarArchivo.Location = new System.Drawing.Point(405, 54);
            this.btnExaminarArchivo.Name = "btnExaminarArchivo";
            this.btnExaminarArchivo.Size = new System.Drawing.Size(100, 25);
            this.btnExaminarArchivo.TabIndex = 2;
            this.btnExaminarArchivo.Text = "Examinar...";
            this.btnExaminarArchivo.UseVisualStyleBackColor = true;
            this.btnExaminarArchivo.Click += new System.EventHandler(this.btnExaminarArchivo_Click);

            // 
            // lblDescripcionRestore
            // 
            this.lblDescripcionRestore.AutoSize = true;
            this.lblDescripcionRestore.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDescripcionRestore.Location = new System.Drawing.Point(15, 95);
            this.lblDescripcionRestore.Name = "lblDescripcionRestore";
            this.lblDescripcionRestore.Size = new System.Drawing.Size(150, 15);
            this.lblDescripcionRestore.TabIndex = 3;
            this.lblDescripcionRestore.Text = "Descripción (opcional):";

            // 
            // txtDescripcionRestore
            // 
            this.txtDescripcionRestore.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescripcionRestore.Location = new System.Drawing.Point(15, 115);
            this.txtDescripcionRestore.MaxLength = 500;
            this.txtDescripcionRestore.Name = "txtDescripcionRestore";
            this.txtDescripcionRestore.Size = new System.Drawing.Size(490, 23);
            this.txtDescripcionRestore.TabIndex = 4;

            // 
            // lblAdvertenciaRestore
            // 
            this.lblAdvertenciaRestore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAdvertenciaRestore.ForeColor = System.Drawing.Color.Firebrick;
            this.lblAdvertenciaRestore.Location = new System.Drawing.Point(15, 150);
            this.lblAdvertenciaRestore.Name = "lblAdvertenciaRestore";
            this.lblAdvertenciaRestore.Size = new System.Drawing.Size(490, 45);
            this.lblAdvertenciaRestore.TabIndex = 5;
            this.lblAdvertenciaRestore.Text = "ADVERTENCIA: al restaurar, la aplicación se reiniciará y se perderán todos los cambios realizados desde el backup seleccionado.";

            // 
            // btnRestaurar
            // 
            this.btnRestaurar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestaurar.ForeColor = System.Drawing.Color.Firebrick;
            this.btnRestaurar.Location = new System.Drawing.Point(305, 205);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(200, 35);
            this.btnRestaurar.TabIndex = 6;
            this.btnRestaurar.Text = "Restaurar Base de Datos";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);

            // 
            // frmGestionarRespaldo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 560);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.gbBackup);
            this.Controls.Add(this.gbRestore);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmGestionarRespaldo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Respaldo";
            this.gbBackup.ResumeLayout(false);
            this.gbBackup.PerformLayout();
            this.gbRestore.ResumeLayout(false);
            this.gbRestore.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;

        // Grupo Backup
        private System.Windows.Forms.GroupBox gbBackup;
        private System.Windows.Forms.Label lblCarpetaDestino;
        private System.Windows.Forms.TextBox txtCarpetaDestino;
        private System.Windows.Forms.Button btnExaminarCarpeta;
        private System.Windows.Forms.Label lblDescripcionBackup;
        private System.Windows.Forms.TextBox txtDescripcionBackup;
        private System.Windows.Forms.Button btnRealizarBackup;

        // Grupo Restore
        private System.Windows.Forms.GroupBox gbRestore;
        private System.Windows.Forms.Label lblArchivoBak;
        private System.Windows.Forms.TextBox txtArchivoBak;
        private System.Windows.Forms.Button btnExaminarArchivo;
        private System.Windows.Forms.Label lblDescripcionRestore;
        private System.Windows.Forms.TextBox txtDescripcionRestore;
        private System.Windows.Forms.Label lblAdvertenciaRestore;
        private System.Windows.Forms.Button btnRestaurar;
    }
}