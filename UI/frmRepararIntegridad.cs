using BLL;
using Servicios;
using Servicios.DigitoVerificador;
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

    /// Modo degradado:
    ///   - No hay sesion iniciada en SM (los datos vienen de una base
    ///     potencialmente comprometida, no se confia en ellos para gobernar
    ///     una sesion completa).
    ///   - Solo se habilita la accion de reparar: restore de backup, o
    ///     recalculo de los DV.
    ///   - Cualquier reparacion exitosa reinicia la aplicacion.
    public partial class frmRepararIntegridad : Form, IObservadorIdioma
    {
        private readonly Usuario _adminReparador;
        private readonly List<Rol> _rolesAdminReparador;
        private readonly IReadOnlyList<Inconsistencia> _inconsistencias;
        private readonly DigitoVerificadorBLL _digitoVerificadorBLL;

       
        public frmRepararIntegridad(Usuario adminReparador, List<Rol> rolesAdmin,IReadOnlyList<Inconsistencia> inconsistencias)
        {
            _adminReparador = adminReparador;
            _rolesAdminReparador = rolesAdmin;
            _inconsistencias = inconsistencias;
            _digitoVerificadorBLL = new DigitoVerificadorBLL();

            InitializeComponent();

            ActualizarIdioma();
            CargarInconsistenciasEnGrilla();

            this.Load += frmRepararIntegridad_Load;
            this.FormClosed += frmRepararIntegridad_FormClosed;
        }


        private void frmRepararIntegridad_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
        }

        private void frmRepararIntegridad_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }


        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmRepararIntegridad.Title");
            lblTitulo.Text = t.Traducir("frmRepararIntegridad.LblTitulo");
            lblExplicacion.Text = t.Traducir("frmRepararIntegridad.LblExplicacion");
            lblAdvertenciaGeneral.Text = t.Traducir("frmRepararIntegridad.LblAdvertenciaGeneral");
            lblAdvertenciaRecalcular.Text = t.Traducir("frmRepararIntegridad.LblAdvertenciaRecalcular");
            btnRestaurar.Text = t.Traducir("frmRepararIntegridad.BtnRestaurar");
            btnRecalcular.Text = t.Traducir("frmRepararIntegridad.BtnRecalcular");
            btnCancelar.Text = t.Traducir("frmRepararIntegridad.BtnCancelar");

            if (dgvInconsistencias.Columns.Count > 0)
            {
                dgvInconsistencias.Columns["Tabla"].HeaderText =t.Traducir("frmRepararIntegridad.ColTabla");
                dgvInconsistencias.Columns["Tipo"].HeaderText =t.Traducir("frmRepararIntegridad.ColTipo");
                dgvInconsistencias.Columns["Registro"].HeaderText =t.Traducir("frmRepararIntegridad.ColRegistro");
            }
        }

        
        private void CargarInconsistenciasEnGrilla()
        {
            var t = Traductor.Instancia;

            dgvInconsistencias.Columns.Clear();
            dgvInconsistencias.Columns.Add("Tabla", t.Traducir("frmRepararIntegridad.ColTabla"));
            dgvInconsistencias.Columns.Add("Tipo", t.Traducir("frmRepararIntegridad.ColTipo"));
            dgvInconsistencias.Columns.Add("Registro", t.Traducir("frmRepararIntegridad.ColRegistro"));

            dgvInconsistencias.Columns["Tabla"].FillWeight = 25;
            dgvInconsistencias.Columns["Tipo"].FillWeight = 15;
            dgvInconsistencias.Columns["Registro"].FillWeight = 60;

            foreach (Inconsistencia inc in _inconsistencias)
            {
                string tipoTexto = inc.Tipo == Inconsistencia.TipoInconsistencia.DVH? t.Traducir("frmRepararIntegridad.TipoDVH"): t.Traducir("frmRepararIntegridad.TipoDVV");

                string registroTexto = inc.Tipo == Inconsistencia.TipoInconsistencia.DVV? t.Traducir("frmRepararIntegridad.RegistroDVV"): inc.IdentificadorPk;

                dgvInconsistencias.Rows.Add(inc.Tabla, tipoTexto, registroTexto);
            }
        }

       

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            // Se registra en SM temporalmente al admin para que frmGestionarRespaldo
            // (que usa SM.UsuarioActual y SM.TienePermiso) pueda operar.
            // Al terminar el restore, la app se reinicia y esta sesion se descarta.

            SM.Instancia.IniciarSesion(_adminReparador);
            SM.Instancia.EstablecerRolesUsuario(_rolesAdminReparador);

            using (var formRestore = new frmGestionarRespaldo(true))
            {
                formRestore.ShowDialog(this);
                // Si el restore fue exitoso, frmGestionarRespaldo ya llamo Application.Restart() y esta linea no se ejecuta.
                // Si el admin cancelo o el restore fallo, la ejecucion sigue aca.
            }

            // Si llegamos aca, la reparacion no se completo. Limpiamos el SM para no dejar sesion colgada.
            SM.Instancia.CerrarSesion();
        }

    
        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(t.Traducir("frmRepararIntegridad.ConfirmarRecalcular"),t.Traducir("frmRepararIntegridad.Title"),MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                _digitoVerificadorBLL.RecalcularTodo();

                Cursor.Current = Cursors.Default;

                MessageBox.Show(t.Traducir("frmRepararIntegridad.RecalcularExitoso"),t.Traducir("frmRepararIntegridad.Title"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                // Reparacion completa: reiniciar la app para estado limpio.
                Application.Restart();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message,t.Traducir("frmRepararIntegridad.Title"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

    

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
