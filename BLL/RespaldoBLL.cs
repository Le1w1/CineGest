using DAL;
using Microsoft.Data.SqlClient;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RespaldoBLL
    {
        private const string PERMISO = "BAK_GESTIONAR";
        private const string TIPO_BACKUP = "BACKUP";
        private const string TIPO_RESTORE = "RESTORE";
        private const string RESULTADO_EXITOSO = "Exitoso";
        private const string RESULTADO_FALLIDO = "Fallido";
        private const string MODULO = "Respaldo";

        private readonly RespaldoDAL _respaldoDAL;
        private readonly BitacoraEventoBLL _bitacoraEventoBLL;
        private readonly DigitoVerificadorBLL _digitoVerificadorBLL; 
        private static string T(string clave) => Traductor.Instancia.Traducir(clave);

        public RespaldoBLL()
        {
            _respaldoDAL = new RespaldoDAL();
            _bitacoraEventoBLL = new BitacoraEventoBLL();
            _digitoVerificadorBLL = new DigitoVerificadorBLL();
        }

       
        /// Ejecuta un backup completo de la base en la carpeta indicada.
        /// El nombre del archivo se genera automaticamente con timestamp.
        /// Devuelve el path completo del archivo generado.
        public string EjecutarBackup(string carpetaDestino, string descripcion)
        {
            ValidarPermiso();
            ValidarCarpeta(carpetaDestino);

            Usuario usuarioActual = SM.Instancia.UsuarioActual;
            DateTime ahora = DateTime.Now;
            string nombreArchivo = GenerarNombreArchivoBackup(ahora);
            string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            try
            {
                _respaldoDAL.EjecutarBackup(rutaCompleta);

                // Backup exitoso: registrar en Bitacora y en la tabla RespaldoBD.
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario,MODULO,"Crear Backup","Alta",RESULTADO_EXITOSO,$"Se realizo un backup de la base en: {rutaCompleta}");
                _respaldoDAL.RegistrarEnRespaldoBD(ahora, TIPO_BACKUP, rutaCompleta, nombreArchivo, RESULTADO_EXITOSO, descripcion, usuarioActual.IdUsuario);
                _digitoVerificadorBLL.RecalcularDV("RespaldoBD");
                return rutaCompleta;
            }
            catch (Exception ex)
            {
                // Backup fallido: registrar en Bitacora y en RespaldoBD.
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario,MODULO,"Crear Backup","Alta",RESULTADO_FALLIDO,$"Fallo el backup en: {rutaCompleta}. Detalle: {ex.Message}");
                _respaldoDAL.RegistrarEnRespaldoBD(ahora, TIPO_BACKUP, rutaCompleta, nombreArchivo,RESULTADO_FALLIDO, descripcion, usuarioActual.IdUsuario);
                _digitoVerificadorBLL.RecalcularDV("RespaldoBD");
                throw new Exception("No se pudo completar el backup: " + ex.Message, ex);
            }
        }





        /// Despues de un restore exitoso se reinicia la aplicacion
        public void EjecutarRestore(string rutaArchivo, string descripcion)
        {
            ValidarPermiso();
            ValidarArchivo(rutaArchivo);

            Usuario usuarioActual = SM.Instancia.UsuarioActual;
            DateTime ahora = DateTime.Now;
            string nombreArchivo = Path.GetFileName(rutaArchivo);

            // Registrar el INTENTO en Bitacora ANTES del restore.
            // Si el restore triunfa, este registro se pierde con el resto de
            // Bitacora, pero al menos el admin sabe que se intento.
            _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario,MODULO,"Restaurar Backup","Alta","Iniciado",$"Se inicia restore de la base desde: {rutaArchivo}");
            try
            {
                _respaldoDAL.EjecutarRestore(rutaArchivo);
            }
            catch (Exception ex)
            {
                // Restore fallido: la base sigue en su estado anterior (el
                // finally del DAL la volvio a MULTI_USER). Podemos registrar.
                _bitacoraEventoBLL.Registrar(usuarioActual.IdUsuario,usuarioActual.NombreUsuario,MODULO,"Restaurar Backup","Alta",RESULTADO_FALLIDO,$"Fallo el restore desde: {rutaArchivo}. Detalle: {ex.Message}");
                _respaldoDAL.RegistrarEnRespaldoBD(ahora, TIPO_RESTORE, rutaArchivo, nombreArchivo,RESULTADO_FALLIDO, descripcion, usuarioActual.IdUsuario);
                _digitoVerificadorBLL.RecalcularDV("RespaldoBD");

                throw new Exception("No se pudo completar el restore: " + ex.Message, ex);
            }
        }

       
        /// Genera un nombre de archivo con timestamp ordenable alfabeticamente.
        /// Formato: CineGestDB_Backup_yyyyMMdd_HHmmss.bak
        /// Ej: CineGestDB_Backup_20260701_143045.bak
        
        private string GenerarNombreArchivoBackup(DateTime fechaHora)
        {
            return $"CineGestDB_Backup_{fechaHora:yyyyMMdd_HHmmss}.bak";
        }

        private void ValidarPermiso()
        {
            if (!SM.Instancia.TienePermiso(PERMISO))
                throw new UnauthorizedAccessException(T("Errores.SinPermisoRespaldo"));
        }

        private void ValidarCarpeta(string carpeta)
        {
            if (string.IsNullOrWhiteSpace(carpeta))
                throw new ArgumentException(T("Errores.DebeSeleccionarCarpeta"));

            if (!Directory.Exists(carpeta))
                throw new DirectoryNotFoundException(
                    string.Format(T("Errores.CarpetaNoExiste"), carpeta));
        }

        private void ValidarArchivo(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                throw new ArgumentException(T("Errores.DebeSeleccionarArchivo"));

            if (!File.Exists(ruta))
                throw new FileNotFoundException(
                    string.Format(T("Errores.ArchivoNoExiste"), ruta));

            if (!ruta.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(T("Errores.ArchivoNoBak"));
        }
    }
}
