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
    }
}
