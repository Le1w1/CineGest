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
    public partial class frmCambiarIdioma : Form, IObservadorIdioma
    {
        private readonly IdiomaBLL _idiomaBLL;

        public frmCambiarIdioma()
        {
            InitializeComponent();
            _idiomaBLL = new IdiomaBLL();

            // Traducir YA, antes de que el form se pinte.
            ActualizarIdioma();
        }

        private void frmCambiarIdioma_Load(object sender, EventArgs e)
        {
            CargarIdiomas();

            // Suscripcion al Observer: si el idioma cambia mientras
            // este form esta abierto, ActualizarIdioma() se ejecuta solo.
            SM.Instancia.Suscribir(this);
        }

        private void frmCambiarIdioma_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Desuscribirse para evitar memory leaks y notificaciones a un form cerrado.
            SM.Instancia.Desuscribir(this);
        }

        private void CargarIdiomas()
        {
            try
            {
                List<Idioma> idiomas = _idiomaBLL.ListarIdiomas();

                cboIdiomas.DataSource = idiomas;
                cboIdiomas.DisplayMember = "Nombre";
                cboIdiomas.ValueMember = "IdIdioma";

                // Pre-seleccionar el idioma actual de la sesion
                Idioma actual = SM.Instancia.IdiomaActual;
                if (actual != null)
                {
                    for (int i = 0; i < idiomas.Count; i++)
                    {
                        if (idiomas[i].IdIdioma == actual.IdIdioma)
                        {
                            cboIdiomas.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, "Cambiar Idioma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;

            Idioma seleccionado = cboIdiomas.SelectedItem as Idioma;

            if (seleccionado == null)
            {
                lblMensaje.Text = Traductor.Instancia.Traducir("frmCambiarIdioma.MsgSeleccioneIdioma");
                return;
            }

            try
            {
                _idiomaBLL.CambiarIdioma(seleccionado);

                MessageBox.Show(
                    Traductor.Instancia.Traducir("frmCambiarIdioma.MsgCambioExitoso"),
                    Traductor.Instancia.Traducir("frmCambiarIdioma.Title"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, "Cambiar Idioma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Implementacion de IObservadorIdioma.
        /// Aplica las traducciones a todos los controles del form.
        /// </summary>
        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmCambiarIdioma.Title");
            lblIdioma.Text = t.Traducir("frmCambiarIdioma.LblIdioma");
            btnGuardar.Text = t.Traducir("frmCambiarIdioma.BtnGuardar");
            btnVolver.Text = t.Traducir("frmCambiarIdioma.BtnVolver");
        }
    }
}
