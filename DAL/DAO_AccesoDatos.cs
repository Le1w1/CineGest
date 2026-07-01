using Microsoft.Data.SqlClient;

namespace DAL
{
    public class DAO_AccesoDatos
    {
        private readonly string _cadenaConexion;

        public DAO_AccesoDatos()
        {
            _cadenaConexion = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=CineGestDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        }
        // en la uai hay que cambiar la cadena el data source = localhost/SQLEXPRESS por "Data Source =." ;

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }

        /// Devuelve una conexion apuntando a la base [master] en lugar de CineGestDB.
        /// Se usa exclusivamente para las operaciones BACKUP DATABASE y RESTORE DATABASE,
        /// que no pueden ejecutarse desde una conexion contra la propia base a restaurar.
        /// Reutiliza los demas parametros de la cadena original (Data Source, seguridad,
        /// encriptacion) cambiando solo el Initial Catalog.
        public SqlConnection ObtenerConexionMaster()
        {
            // Se construye una nueva cadena a partir de la original, cambiando solo el Initial Catalog
            var builder = new SqlConnectionStringBuilder(_cadenaConexion);
            builder.InitialCatalog = "master";
            return new SqlConnection(builder.ConnectionString);
        }


        /// Devuelve el nombre de la base de datos objetivo (CineGestDB).
        /// Necesario para armar comandos BACKUP/RESTORE que referencian a la base por nombre.
        /// Se obtiene de la cadena para que si algun dia se parametriza la cadena
        /// (App.config, instalador), este metodo siga funcionando sin cambios.
        public string ObtenerNombreBaseDatos()
        {
            var builder = new SqlConnectionStringBuilder(_cadenaConexion);
            return builder.InitialCatalog;
        }
    }
}
