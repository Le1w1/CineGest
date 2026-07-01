using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{

    public partial class frmGestionarRespaldo : Form, IObservadorIdioma
    {
        private const string PERMISO = "BAK_GESTIONAR";

        private readonly RespaldoBLL _respaldoBLL;
        private readonly bool _soloRestore;

        /// Constructor por defecto: modo normal (backup + restore habilitados).
        /// Se usa desde el menu principal en operacion normal (T07).
        public frmGestionarRespaldo() : this(false)
        {
        }

        /// Constructor para MODO SOLO RESTORE: oculta el grupo de Backup.
        /// Se usa desde la pantalla de reparacion (T08.3), donde la base
        /// esta corrupta y hacer un backup no tiene sentido.
        public frmGestionarRespaldo(bool soloRestore)
        {
            _soloRestore = soloRestore;

            InitializeComponent();
            _respaldoBLL = new RespaldoBLL();

            AplicarPermisos();
            ActualizarIdioma();

            this.Load += frmGestionarRespaldo_Load;
            this.FormClosed += frmGestionarRespaldo_FormClosed;
        }
       
        private void frmGestionarRespaldo_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
        }

        private void frmGestionarRespaldo_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmGestionarRespaldo.Title");
            lblTitulo.Text = t.Traducir("frmGestionarRespaldo.LblTitulo");

            gbBackup.Text = t.Traducir("frmGestionarRespaldo.GbBackup");
            lblCarpetaDestino.Text = t.Traducir("frmGestionarRespaldo.LblCarpetaDestino");
            lblDescripcionBackup.Text = t.Traducir("frmGestionarRespaldo.LblDescripcionBackup");
            btnExaminarCarpeta.Text = t.Traducir("frmGestionarRespaldo.BtnExaminarCarpeta");
            btnRealizarBackup.Text = t.Traducir("frmGestionarRespaldo.BtnRealizarBackup");

            gbRestore.Text = t.Traducir("frmGestionarRespaldo.GbRestore");
            lblArchivoBak.Text = t.Traducir("frmGestionarRespaldo.LblArchivoBak");
            lblDescripcionRestore.Text = t.Traducir("frmGestionarRespaldo.LblDescripcionRestore");
            btnExaminarArchivo.Text = t.Traducir("frmGestionarRespaldo.BtnExaminarArchivo");
            btnRestaurar.Text = t.Traducir("frmGestionarRespaldo.BtnRestaurar");
            lblAdvertenciaRestore.Text = t.Traducir("frmGestionarRespaldo.LblAdvertenciaRestore");
        }


        /// Ademas, si se abrio en MODO SOLO RESTORE,
        /// oculta completamente el grupo de Backup y reubica el grupo de
        /// Restore para que el form no quede con un espacio vacio arriba.

        private void AplicarPermisos()
        {
            bool puedeGestionar = SM.Instancia.TienePermiso(PERMISO);

            btnExaminarCarpeta.Enabled = puedeGestionar;
            btnRealizarBackup.Enabled = puedeGestionar;
            btnExaminarArchivo.Enabled = puedeGestionar;
            btnRestaurar.Enabled = puedeGestionar;

            if (_soloRestore)
            {
                // Ocultar el grupo de Backup completo.
                gbBackup.Visible = false;

                // Reubicar el grupo de Restore donde estaba el de Backup
                gbRestore.Location = new System.Drawing.Point(gbRestore.Location.X,gbBackup.Location.Y);

                // Achicar el form a la altura estrictamente necesaria.
                this.ClientSize = new System.Drawing.Size(this.ClientSize.Width,gbRestore.Location.Y + gbRestore.Height + 20);
            }
        }


        //  BACKUP
        private void btnExaminarCarpeta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = Traductor.Instancia.Traducir("frmGestionarRespaldo.DialogCarpeta");
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                    txtCarpetaDestino.Text = dialog.SelectedPath;
            }
        }

        private void btnRealizarBackup_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string rutaGenerada = _respaldoBLL.EjecutarBackup(txtCarpetaDestino.Text,txtDescripcionBackup.Text);

                Cursor.Current = Cursors.Default;

                MessageBox.Show(string.Format(t.Traducir("frmGestionarRespaldo.BackupExitoso"), rutaGenerada),t.Traducir("frmGestionarRespaldo.Title"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                // Limpiar campos para el proximo backup
                txtCarpetaDestino.Text = string.Empty;
                txtDescripcionBackup.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                MessageBox.Show(ex.Message,t.Traducir("frmGestionarRespaldo.Title"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        //  RESTORE

        private void btnExaminarArchivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = Traductor.Instancia.Traducir("frmGestionarRespaldo.DialogArchivo");
                dialog.Filter = Traductor.Instancia.Traducir("frmGestionarRespaldo.FiltroBak");
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                    txtArchivoBak.Text = dialog.FileName;
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(t.Traducir("frmGestionarRespaldo.ConfirmarRestore"),t.Traducir("frmGestionarRespaldo.Title"),MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                _respaldoBLL.EjecutarRestore(txtArchivoBak.Text,txtDescripcionRestore.Text);

                Cursor.Current = Cursors.Default;

                // Restore exitoso. Notificar y reiniciar.
                // Application.Restart() cierra la app actual y arranca un nuevo proceso, lo que fuerza reconexion limpia y nuevo login.
                MessageBox.Show(t.Traducir("frmGestionarRespaldo.RestoreExitoso"),t.Traducir("frmGestionarRespaldo.Title"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                Application.Restart();
                Environment.Exit(0);   // garantiza cierre inmediato del proceso actual
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message,t.Traducir("frmGestionarRespaldo.Title"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
