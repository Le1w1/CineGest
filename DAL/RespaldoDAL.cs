using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// Tres responsabilidades:
    ///   - EjecutarBackup: ejecuta BACKUP DATABASE contra la base actual.

    ///   - EjecutarRestore: ejecuta RESTORE DATABASE contra la base actual.
    ///     Se conecta a [master] porque no se puede restaurar una base desde
    ///     una conexion contra ella misma. Ademas la pone en SINGLE_USER con
    ///     ROLLBACK IMMEDIATE (desconecta a cualquier otro usuario) y la vuelve
    ///     a MULTI_USER al terminar, incluso si el restore falla.

    ///   - RegistrarEnRespaldoBD: inserta el registro en la tabla RespaldoBD.
    ///     Se llama desde el BLL despues de cada operacion (exitosa o fallida)
    ///     para dejar la trazabilidad en base.

    public class RespaldoDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public RespaldoDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }

        /// Ejecuta BACKUP DATABASE hacia el archivo indicado.
        /// La conexion se hace contra la propia base (no hace falta master
        /// para BACKUP, solo para RESTORE). Sin timeout: un backup grande
        /// no debe morir a los 30 segundos.
        /// El nombre del archivo se inyecta directo en el SQL con formato
        /// N'...' escapando comillas simples para evitar inyeccion trivial.
        public void EjecutarBackup(string rutaArchivo)
        {
            string nombreBase = _conexionDAL.ObtenerNombreBaseDatos();
            string rutaEscapada = EscaparComillas(rutaArchivo);

            string sql = $"BACKUP DATABASE [{nombreBase}] TO DISK = N'{rutaEscapada}' " +
                         $"WITH FORMAT, INIT, NAME = N'CineGest Full Backup', SKIP, STATS = 10";

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.CommandTimeout = 0;   // sin limite; backups largos son legitimos
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// Ejecuta RESTORE DATABASE desde el archivo indicado.
        /// Secuencia:
        ///   1) Conectar a [master] (no se puede restaurar CineGestDB desde una conexion contra CineGestDB).
        ///   2) SET SINGLE_USER WITH ROLLBACK IMMEDIATE: fuerza a la base a
        ///      un solo usuario y desconecta a todos los demas (incluida
        ///      esta misma app si tenia otras conexiones abiertas).
        ///   3) RESTORE DATABASE WITH REPLACE: reemplaza los archivos.
        ///   4) SET MULTI_USER: vuelve a permitir conexiones.

        /// try/finally: si el RESTORE falla, el finally garantiza el
        /// MULTI_USER para que la base no quede bloqueada.
        /// Despues de este metodo, la app DEBE reiniciarse (el flujo del BLL
        /// y de la UI se encargan de eso). No es responsabilidad del DAL.
        public void EjecutarRestore(string rutaArchivo)
        {
            string nombreBase = _conexionDAL.ObtenerNombreBaseDatos();
            string rutaEscapada = EscaparComillas(rutaArchivo);

            string sqlSingleUser = $"ALTER DATABASE [{nombreBase}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
            string sqlRestore = $"RESTORE DATABASE [{nombreBase}] FROM DISK = N'{rutaEscapada}' WITH REPLACE, STATS = 10";
            string sqlMultiUser = $"ALTER DATABASE [{nombreBase}] SET MULTI_USER";

            SqlConnection.ClearAllPools();

            using (SqlConnection conexion = _conexionDAL.ObtenerConexionMaster())
            {
                conexion.Open();

                try
                {
                    using (SqlCommand cmdSingle = new SqlCommand(sqlSingleUser, conexion))
                    {
                        cmdSingle.CommandTimeout = 0;
                        cmdSingle.ExecuteNonQuery();
                    }

                    using (SqlCommand cmdRestore = new SqlCommand(sqlRestore, conexion))
                    {
                        cmdRestore.CommandTimeout = 0;
                        cmdRestore.ExecuteNonQuery();
                    }
                }
                finally
                {
                    // Garantiza que la base vuelva a estar accesible aunque el RESTORE haya fallado a mitad.
                    // Si esto tambien falla, el problema es serio y hay que resolverlo en SSMS a mano.
                    try
                    {
                        using (SqlCommand cmdMulti = new SqlCommand(sqlMultiUser, conexion))
                        {
                            cmdMulti.CommandTimeout = 0;
                            cmdMulti.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }



        /// Inserta un registro en la tabla RespaldoBD.
        /// El DVH se calcula desde el BLL despues de este insert (no aca)
        /// porque el DVH involucra el IdRespaldo que solo existe despues
        /// del INSERT (IDENTITY).
        public int RegistrarEnRespaldoBD(DateTime fechaHora, string tipoOperacion, string rutaArchivo, string nombreArchivo, string resultado, string descripcion, int idUsuario)
        {
            string sql = @"
                INSERT INTO RespaldoBD (FechaHora, TipoOperacion, RutaArchivo, NombreArchivo, Resultado, Descripcion, IdUsuario)
                VALUES (@FechaHora, @TipoOperacion, @RutaArchivo, @NombreArchivo, @Resultado, @Descripcion, @IdUsuario);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@FechaHora", fechaHora);
                comando.Parameters.AddWithValue("@TipoOperacion", tipoOperacion);
                comando.Parameters.AddWithValue("@RutaArchivo", rutaArchivo);
                comando.Parameters.AddWithValue("@NombreArchivo", nombreArchivo);
                comando.Parameters.AddWithValue("@Resultado", resultado);
                comando.Parameters.AddWithValue("@Descripcion",
                    (object)descripcion ?? DBNull.Value);
                comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                conexion.Open();
                object resultadoScalar = comando.ExecuteScalar();
                return Convert.ToInt32(resultadoScalar);
            }
        }

        private static string EscaparComillas(string valor)
        {
            return valor == null ? string.Empty : valor.Replace("'", "''");
        }
    }
}