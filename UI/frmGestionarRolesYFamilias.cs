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
    public partial class frmGestionarRolesYFamilias : Form, IObservadorIdioma
    {
        private readonly FamiliaBLL _familiaBLL;
        private readonly RolBLL _rolBLL;
        private readonly PermisoSimpleBLL _permisoSimpleBLL;

        // Componente actual en edicion (Familia o Rol)
        private Componente _enEdicion;

        // True si _enEdicion es un componente nuevo que aun no fue persistido
        private bool _esNuevo;

        // Flags para evitar disparos en cadena de eventos durante recargas
        private bool _cargandoCombo;
        private bool _refrescandoCheckList;

        public frmGestionarRolesYFamilias()
        {
            InitializeComponent();
            _familiaBLL = new FamiliaBLL();
            _rolBLL = new RolBLL();
            _permisoSimpleBLL = new PermisoSimpleBLL();

            ActualizarIdioma();

            this.Load += frmGestionar_Load;
            this.FormClosed += frmGestionar_FormClosed;
        }

        private void frmGestionar_Load(object sender, EventArgs e)
        {
            SM.Instancia.Suscribir(this);
            RecargarTodo();
        }

        private void frmGestionar_FormClosed(object sender, FormClosedEventArgs e)
        {
            SM.Instancia.Desuscribir(this);
        }

        public void ActualizarIdioma()
        {
            var t = Traductor.Instancia;

            this.Text = t.Traducir("frmGestionarRolesYFamilias.Title");
            lblTitulo.Text = t.Traducir("frmGestionarRolesYFamilias.LblTitulo");
            lblRolFamilia.Text = t.Traducir("frmGestionarRolesYFamilias.LblRolFamilia");
            gbDisponibles.Text = t.Traducir("frmGestionarRolesYFamilias.GbDisponibles");
            gbComposicion.Text = t.Traducir("frmGestionarRolesYFamilias.GbComposicion");
            gbDatos.Text = t.Traducir("frmGestionarRolesYFamilias.GbDatos");
            lblNombre.Text = t.Traducir("frmGestionarRolesYFamilias.LblNombre");
            btnCrear.Text = t.Traducir("frmGestionarRolesYFamilias.BtnCrear");
            rbFamilia.Text = t.Traducir("frmGestionarRolesYFamilias.RbFamilia");
            rbRol.Text = t.Traducir("frmGestionarRolesYFamilias.RbRol");
            btnEliminarSeleccionado.Text = t.Traducir("frmGestionarRolesYFamilias.BtnEliminarSeleccionado");
            btnGuardarCambios.Text = t.Traducir("frmGestionarRolesYFamilias.BtnGuardar");
            btnCancelar.Text = t.Traducir("frmGestionarRolesYFamilias.BtnCancelar");
            btnEliminar.Text = t.Traducir("frmGestionarRolesYFamilias.BtnEliminar");
        }

        #region "Carga y refresco"

        private void RecargarTodo()
        {
            _enEdicion = null;
            _esNuevo = false;
            lblMensaje.Text = string.Empty;

            CargarDisponibles();
            CargarCombo();

            tvComposicion.Nodes.Clear();
            txtNombre.Clear();
            rbFamilia.Checked = true;

            ActivarEstadoInicial();
        }

        private void CargarDisponibles()
        {
            _refrescandoCheckList = true;

            clbDisponibles.Items.Clear();

            // PermisoSimples (todos)
            List<PermisoSimple> permisos = _permisoSimpleBLL.ListarTodos();
            foreach (PermisoSimple p in permisos) clbDisponibles.Items.Add(p);

            // Familias activas (las inactivas no se ofrecen para componer)
            List<Familia> familias = _familiaBLL.ListarTodas();
            foreach (Familia f in familias) clbDisponibles.Items.Add(f);

            _refrescandoCheckList = false;
        }

        private void CargarCombo()
        {
            _cargandoCombo = true;

            cboRolFamilia.Items.Clear();

            // Item especial: "(Nuevo)" como entry de creacion
            cboRolFamilia.Items.Add(Traductor.Instancia.Traducir("frmGestionarRolesYFamilias.ItemNuevo"));

            List<Familia> familias = _familiaBLL.ListarTodas();
            foreach (Familia f in familias) cboRolFamilia.Items.Add(f);

            List<Rol> roles = _rolBLL.ListarTodos();
            foreach (Rol r in roles) cboRolFamilia.Items.Add(r);

            cboRolFamilia.SelectedIndex = 0;
            _cargandoCombo = false;
        }

        private void RefrescarTreeView()
        {
            tvComposicion.Nodes.Clear();
            if (_enEdicion == null) return;

            List<Componente> hijos = ObtenerHijos(_enEdicion);
            foreach (Componente hijo in hijos)
            {
                tvComposicion.Nodes.Add(BuildNode(hijo));
            }
            tvComposicion.ExpandAll();
        }

        private TreeNode BuildNode(Componente c)
        {
            TreeNode node = new TreeNode(c.ToString());
            node.Tag = c;

            // Hijos: solo Familias y Roles tienen estructura recursiva
            if (c is Familia f)
            {
                foreach (Componente hijo in f.Hijos) node.Nodes.Add(BuildNode(hijo));
            }
            else if (c is Rol r)
            {
                foreach (Componente hijo in r.Hijos) node.Nodes.Add(BuildNode(hijo));
            }

            return node;
        }

        private void RefrescarCheckListSegunComponente()
        {
            // Marca los items del clbDisponibles que estan en la composicion
            // del componente en edicion. Asi al cambiar de seleccion en el
            // combo, el checklist refleja el estado actual.
            _refrescandoCheckList = true;

            try
            {
                List<Componente> hijosActuales = _enEdicion == null
                    ? new List<Componente>()
                    : ObtenerHijos(_enEdicion);

                for (int i = 0; i < clbDisponibles.Items.Count; i++)
                {
                    Componente item = clbDisponibles.Items[i] as Componente;
                    bool deberiaEstarMarcado = item != null && hijosActuales.Contains(item);
                    clbDisponibles.SetItemChecked(i, deberiaEstarMarcado);
                }
            }
            finally
            {
                _refrescandoCheckList = false;
            }
        }

        #endregion

        #region "Estados del form (habilitacion de controles)"

        private void ActivarEstadoInicial()
        {
            // Sin entidad en edicion: se puede crear una nueva, pero no editar nada.
            rbFamilia.Enabled = true;
            rbRol.Enabled = true;
            txtNombre.Enabled = true;
            btnCrear.Enabled = true;

            clbDisponibles.Enabled = false;
            btnEliminarSeleccionado.Enabled = false;
            btnGuardarCambios.Enabled = false;
            btnEliminar.Enabled = false;

            RefrescarCheckListSegunComponente(); // limpia checks
        }

        private void ActivarEstadoEdicion()
        {
            // Hay una entidad en edicion: bloqueamos los controles de creacion
            // y habilitamos los de edicion de composicion.
            rbFamilia.Enabled = false;
            rbRol.Enabled = false;
            txtNombre.Enabled = false;
            btnCrear.Enabled = false;

            clbDisponibles.Enabled = true;
            btnEliminarSeleccionado.Enabled = true;
            btnGuardarCambios.Enabled = true;

            // Solo se puede desactivar una entidad YA persistida
            btnEliminar.Enabled = !_esNuevo;

            RefrescarCheckListSegunComponente();
        }

        #endregion

        #region "Helpers Composite"

        private List<Componente> ObtenerHijos(Componente c)
        {
            if (c is Familia f) return f.Hijos;
            if (c is Rol r) return r.Hijos;
            return new List<Componente>();
        }

        private void AgregarHijo(Componente padre, Componente hijo)
        {
            if (padre is Familia f) f.Agregar(hijo);
            else if (padre is Rol r) r.Agregar(hijo);
        }

        private void QuitarHijo(Componente padre, Componente hijo)
        {
            if (padre is Familia f) f.Quitar(hijo);
            else if (padre is Rol r) r.Quitar(hijo);
        }

        #endregion

        #region "Handlers de eventos"

        private void cboRolFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombo) return;

            object seleccionado = cboRolFamilia.SelectedItem;

            // Si hay un borrador (componente nuevo NO guardado) y se esta saliendo
            // de el (selecciono otro item), preguntar al usuario si descartarlo.
            if (_esNuevo && _enEdicion != null && !ReferenceEquals(seleccionado, _enEdicion))
            {
                var t = Traductor.Instancia;
                DialogResult resp = MessageBox.Show(
                    t.Traducir("frmGestionarRolesYFamilias.ConfirmarDescartarBorrador"),
                    t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resp != DialogResult.Yes)
                {
                    // Revertir: volver a seleccionar el borrador
                    _cargandoCombo = true;
                    cboRolFamilia.SelectedItem = _enEdicion;
                    _cargandoCombo = false;
                    return;
                }

                // Descartar el borrador: sacarlo del combo
                Componente borrador = _enEdicion;
                _cargandoCombo = true;
                cboRolFamilia.Items.Remove(borrador);
                _cargandoCombo = false;

                // Despues del Remove, el SelectedItem puede haber cambiado.
                // Lo recapturamos para procesar la seleccion final del usuario.
                seleccionado = cboRolFamilia.SelectedItem;

                _enEdicion = null;
                _esNuevo = false;
            }

            // (Nuevo) → estado inicial
            if (seleccionado is string)
            {
                _enEdicion = null;
                _esNuevo = false;
                tvComposicion.Nodes.Clear();
                ActivarEstadoInicial();
                return;
            }

            if (seleccionado is Componente c)
            {
                _enEdicion = c;
                _esNuevo = false;
                ActivarEstadoEdicion();
                RefrescarTreeView();
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            string nombre = (txtNombre.Text ?? string.Empty).Trim();
            var t = Traductor.Instancia;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                lblMensaje.Text = t.Traducir("Errores.RBAC.NombreObligatorio");
                MessageBox.Show(t.Traducir("Errores.RBAC.NombreObligatorio"),
                    t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Componente nuevo;
            if (rbFamilia.Checked)
            {
                nuevo = new Familia { Nombre = nombre };
            }
            else
            {
                nuevo = new Rol { Nombre = nombre };
            }

            // Agregamos al combo y seleccionamos
            _cargandoCombo = true;
            cboRolFamilia.Items.Add(nuevo);
            cboRolFamilia.SelectedItem = nuevo;
            _cargandoCombo = false;

            _enEdicion = nuevo;
            _esNuevo = true;

            ActivarEstadoEdicion();
            RefrescarTreeView();

            txtNombre.Clear();
            lblMensaje.Text = string.Empty;
        }

        private void clbDisponibles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_refrescandoCheckList) return;
            if (_enEdicion == null) return;

            Componente item = clbDisponibles.Items[e.Index] as Componente;
            if (item == null) return;

            // CheckedListBox.ItemCheck se dispara ANTES de cambiar el estado,
            // por eso usamos e.NewValue (no .CurrentValue).
            if (e.NewValue == CheckState.Checked)
            {
                AgregarHijo(_enEdicion, item);
            }
            else
            {
                QuitarHijo(_enEdicion, item);
            }

            // Refrescamos el TreeView en el siguiente tick (cuando el checkbox
            // ya cambio efectivamente de estado).
            BeginInvoke(new Action(RefrescarTreeView));
        }

        private void btnEliminarSeleccionado_Click(object sender, EventArgs e)
        {
            if (_enEdicion == null || tvComposicion.SelectedNode == null) return;

            TreeNode nodo = tvComposicion.SelectedNode;

            // Solo permitimos eliminar items del primer nivel del componente
            // en edicion. Los hijos anidados (de Familias) NO se pueden quitar
            // desde aca — para eso hay que editar esa Familia especificamente.
            if (nodo.Parent != null)
            {
                MessageBox.Show(
                    Traductor.Instancia.Traducir("frmGestionarRolesYFamilias.MsgSoloPrimerNivel"),
                    Traductor.Instancia.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Componente comp = nodo.Tag as Componente;
            if (comp == null) return;

            QuitarHijo(_enEdicion, comp);

            // Tambien desmarcamos en clbDisponibles para mantener consistencia
            for (int i = 0; i < clbDisponibles.Items.Count; i++)
            {
                if (clbDisponibles.Items[i] == comp)
                {
                    _refrescandoCheckList = true;
                    clbDisponibles.SetItemChecked(i, false);
                    _refrescandoCheckList = false;
                    break;
                }
            }

            RefrescarTreeView();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (_enEdicion == null) return;

            var t = Traductor.Instancia;

            // Caso: composición quedó vacía.
            //  - Si es NUEVO → no se puede guardar (mensaje específico).
            //  - Si es EXISTENTE → ofrecer auto-desactivar la entidad.
            bool composicionVacia = ObtenerHijos(_enEdicion).Count == 0;
            if (composicionVacia)
            {
                if (_esNuevo)
                {
                    MessageBox.Show(t.Traducir("Errores.RBAC.ComposicionVacia"),
                        t.Traducir("frmGestionarRoles.Title"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                EliminarPorComposicionVacia();
                return;
            }

            try
            {
                if (_enEdicion is Familia f)
                {
                    if (_esNuevo) _familiaBLL.Crear(f);
                    else _familiaBLL.Modificar(f);
                }
                else if (_enEdicion is Rol r)
                {
                    if (_esNuevo) _rolBLL.Crear(r);
                    else _rolBLL.Modificar(r);
                }

                MessageBox.Show(
                    t.Traducir("frmGestionarRolesYFamilias.MsgGuardadoOK"),
                    t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarTodo();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Pregunta al admin si quiere eliminar la entidad (Familia o Rol)
        /// porque su composición quedó vacía. Si confirma, ejecuta el Eliminar
        /// del BLL correspondiente (que valida que no esté en uso). Si no,
        /// la entidad queda como estaba en BD y se descarta el cambio en memoria.
        /// </summary>
        private void EliminarPorComposicionVacia()
        {
            var t = Traductor.Instancia;

            DialogResult resp = MessageBox.Show(
                t.Traducir("frmGestionarRolesYFamilias.ConfirmarEliminarPorVacio"),
                t.Traducir("frmGestionarRolesYFamilias.Title"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp != DialogResult.Yes)
            {
                // El admin dijo NO: no eliminamos, pero los cambios en memoria
                // siguen ahí. Le avisamos que use Cancelar si quiere descartar.
                lblMensaje.Text = t.Traducir("frmGestionarRolesYFamilias.MsgNoEliminado");
                return;
            }

            try
            {
                if (_enEdicion is Familia f) _familiaBLL.Eliminar(f.IdFamilia);
                else if (_enEdicion is Rol r) _rolBLL.Eliminar(r.IdRol);

                MessageBox.Show(
                    t.Traducir("frmGestionarRolesYFamilias.MsgEliminadoOK"),
                    t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarTodo();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            RecargarTodo();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_enEdicion == null || _esNuevo) return;

            var t = Traductor.Instancia;

            DialogResult respuesta = MessageBox.Show(
                t.Traducir("frmGestionarRolesYFamilias.ConfirmarEliminar"),
                t.Traducir("frmGestionarRolesYFamilias.Title"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                if (_enEdicion is Familia f) _familiaBLL.Eliminar(f.IdFamilia);
                else if (_enEdicion is Rol r) _rolBLL.Eliminar(r.IdRol);

                MessageBox.Show(
                    t.Traducir("frmGestionarRolesYFamilias.MsgEliminadoOK"),
                    t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarTodo();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
                MessageBox.Show(ex.Message, t.Traducir("frmGestionarRolesYFamilias.Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
