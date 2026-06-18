using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IdiomaBLL
    {
        private readonly IdiomaDAL _idiomaDAL;
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;

        public IdiomaBLL()
        {
            _idiomaDAL = new IdiomaDAL();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
        }

        public List<Idioma> ListarIdiomas()
        {
            return _idiomaDAL.ListarIdiomas();
        }

        public Idioma ObtenerPorId(int idIdioma)
        {
            return _idiomaDAL.ObtenerPorId(idIdioma);
        }

        public Idioma ObtenerPorCodigo(string codigo)
        {
            return _idiomaDAL.ObtenerPorCodigo(codigo);
        }

        /// <summary>
        /// Cambia el idioma EN MEMORIA (no toca la BD).
        /// La persistencia se realiza al Logout, comparando con el idioma inicial.
        /// </summary>
        public void CambiarIdioma(Idioma idioma)
        {
            if (!SM.Instancia.HaySesionActiva())
            {
                throw new Exception(Traductor.Instancia.Traducir("Errores.NoHaySesionActiva"));
            }

            if (idioma == null)
            {
                throw new Exception(Traductor.Instancia.Traducir("Errores.DebeSeleccionarIdioma"));
            }

            Idioma idiomaActual = SM.Instancia.IdiomaActual;

            if (idiomaActual != null && idiomaActual.IdIdioma == idioma.IdIdioma)
            {
                throw new Exception(Traductor.Instancia.Traducir("Errores.IdiomaIgualActual"));
            }

            SM.Instancia.CambiarIdioma(idioma);

            Usuario usuario = SM.Instancia.UsuarioActual;

            _bitacoraEventoBLL.Registrar(
                usuario.IdUsuario,
                usuario.NombreUsuario,
                "Idioma",
                "Cambio de idioma",
                "Baja",
                "Exitoso",
                "El usuario cambió el idioma a " + idioma.Nombre + " (en memoria, se persiste al cerrar sesión).");
        }
    }
}
