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
using BLL;

namespace UI
{
    public partial class frmMenuPrincipal : Form, IObservadorIdioma
    {
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();

        public frmMenuPrincipal()
        {
            InitializeComponent();

            // Traducir antes de que el form se cargue.
            ActualizarIdioma();
        }

        private void frmMenuPrincipal_Load(object sender, EventArgs e)
        {
            // Suscripcion al Observer de idioma.
            SM.Instancia.Suscribir(this);
            CargarDatosSesion();
            AplicarPermisos();
        }

        /// Recorre los items del menu y los habilita/deshabilita segun los permisos del usuario logueado. 
        /// Excepcion: "Cerrar Sesion" SIEMPRE habilitado por seguridad operativa, sin importar los permisos del rol.
        
        private void AplicarPermisos()
        {
            var sm = SM.Instancia;

            // --- Menu Usuario (sesion) ---
            reLoginToolStripMenuItem.Enabled = sm.TienePermiso("SES_RELOGIN");
            cambiarClaveToolStripMenuItem.Enabled = sm.TienePermiso("SES_CAMBIAR_CLAVE");
            cambiarIdiomaToolStripMenuItem.Enabled = sm.TienePermiso("SES_CAMBIAR_IDIOMA");
            cerrarSesionToolStripMenuItem.Enabled = true; // siempre habilitado

            // --- Menu Boleteria ---
            empleadoDeBoleteríaToolStripMenuItem.Enabled = sm.TienePermiso("BOL_VENDER")|| sm.TienePermiso("BOL_DEVOLVER")|| sm.TienePermiso("BOL_CONSULTAR");

            // --- Menu Cartelera ---
            gerenciaToolStripMenuItem.Enabled =sm.TienePermiso("CART_VER")|| sm.TienePermiso("CART_CREAR_FUNCION")|| sm.TienePermiso("CART_MODIFICAR_FUNCION")|| sm.TienePermiso("CART_ELIMINAR_FUNCION")|| sm.TienePermiso("CART_GESTIONAR_PELICULAS");

            // --- Menu Gerencia ---
            gerenciaToolStripMenuItem1.Enabled =sm.TienePermiso("GER_REPORTES")|| sm.TienePermiso("GER_DASHBOARD")|| sm.TienePermiso("GER_EXPORTAR_DATOS");

            // --- Menu Administrador ---
            usuariosToolStripMenuItem.Enabled =sm.TienePermiso("USR_LISTAR")|| sm.TienePermiso("USR_CREAR")|| sm.TienePermiso("USR_MODIFICAR");

            bitacoraEventosToolStripMenuItem.Enabled = sm.TienePermiso("BIT_AUDITAR");

            gestionarPerfilToolStripMenuItem.Enabled =sm.TienePermiso("ROL_GESTIONAR")|| sm.TienePermiso("FAM_GESTIONAR");

            // Menus padre: deshabilitados si TODOS sus hijos estan deshabilitados 
            mnuAdministrador.Enabled = usuariosToolStripMenuItem.Enabled|| bitacoraEventosToolStripMenuItem.Enabled|| gestionarPerfilToolStripMenuItem.Enabled;

        }

        private void frmMenuPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        /// Implementacion del Observer: el SM nos llama cuando cambia el idioma. Recorre todos los textos del form y aplica las traducciones.
        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmMenuPrincipal.Title");

            // Menu Usuario
            mnuSesion.Text = t.Traducir("frmMenuPrincipal.MenuUsuario");
            reLoginToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuReLogin");
            cambiarClaveToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuCambiarClave");
            cambiarIdiomaToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuCambiarIdioma");
            cerrarSesionToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuCerrarSesion");

            // Menus principales
            empleadoDeBoleteríaToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuBoleteria");
            gerenciaToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuCartelera");
            gerenciaToolStripMenuItem1.Text = t.Traducir("frmMenuPrincipal.MenuGerencia");

            // Menu Administrador
            mnuAdministrador.Text = t.Traducir("frmMenuPrincipal.MenuAdministrador");
            usuariosToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuUsuarios");
            bitacoraEventosToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuBitacora");
            gestionarPerfilToolStripMenuItem.Text = t.Traducir("frmMenuPrincipal.MenuGestionPerfiles");

            // Panel central
            lblTituloInicio.Text = t.Traducir("frmMenuPrincipal.Titulo");
            lblDescripcionInicio.Text = t.Traducir("frmMenuPrincipal.Descripcion");

            // Status bar (depende de la sesion)
            CargarDatosSesion();
        }

        private void CargarDatosSesion()
        {
            var t = Traductor.Instancia;

            if (SM.Instancia.HaySesionActiva())
            {
                Usuario usuario = SM.Instancia.UsuarioActual;

                lblUsuarioSesion.Text = t.Traducir("frmMenuPrincipal.UsuarioLabel") + " " + usuario.NombreUsuario;
                lblEstadoSesion.Text = t.Traducir("frmMenuPrincipal.SesionActiva");
            }
            else
            {
                lblUsuarioSesion.Text = t.Traducir("frmMenuPrincipal.UsuarioLabelSinSesion");
                lblEstadoSesion.Text = t.Traducir("frmMenuPrincipal.SinSesion");
            }
        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionDeUsuario formAdministrador = new frmGestionDeUsuario();
            formAdministrador.ShowDialog();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(t.Traducir("frmMenuPrincipal.ConfirmarCerrarSesion"),t.Traducir("frmMenuPrincipal.CerrarSesionTitulo"),MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    _usuarioBLL.Logout();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CineGest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void reLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin formSesion = new frmLogin(true);
            formSesion.ShowDialog();
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCambiarClave cambiarClave = new frmCambiarClave();
            cambiarClave.ShowDialog();
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCambiarIdioma cambiarIdioma = new frmCambiarIdioma();
            cambiarIdioma.ShowDialog();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionDeUsuario formAdministrador = new frmGestionDeUsuario();
            formAdministrador.ShowDialog();
        }

        private void bitacoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAuditarEventos formAuditarEventos = new frmAuditarEventos();
            formAuditarEventos.ShowDialog();
        }

        private void gestionarPerfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGestionarRolesYFamilias formGestionar = new frmGestionarRolesYFamilias();
            formGestionar.ShowDialog();
        }

    }
}
