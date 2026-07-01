using Microsoft.Data.SqlClient;
using Servicios.DigitoVerificador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// Acceso a datos del Digito Verificador.

    /// Trabaja de forma GENERICA sobre cualquier tabla protegida: recibe la
    /// definicion ("TablaProtegida") y opera con sus columnas. 
    /// No conoce entidades concretas (Usuario, Rol, etc.), por eso una sola clase cubre las 9 tablas.
    
    /// NOTA sobre seguridad: los nombres de tabla y columna se interpolan en el
    /// SQL porque SQL no permite parametrizar identificadores (solo valores).
    /// Esto es seguro porque esos nombres provienen SIEMPRE de TablasProtegidas
    /// (constantes del codigo), nunca de input del usuario.

    public class DigitoVerificadorDAL
    {
        private readonly DAO_AccesoDatos _conexionDAL;

        public DigitoVerificadorDAL()
        {
            _conexionDAL = new DAO_AccesoDatos();
        }


        /// Lee todas las filas de una tabla protegida, ordenadas por PK.
        
        /// Devuelve una lista de "registros", donde cada registro es un
        /// diccionario columna -> valor. Incluye tanto las columnas de datos
        /// como la columna DVH (para poder comparar el guardado con el recalculado durante la verificacion.
        public List<Dictionary<string, object>> LeerRegistros(TablaProtegida tabla)
        {
            var registros = new List<Dictionary<string, object>>();

            // Columnas a traer: las de datos + DVH. Ordenamos por PK para que el DVV sea determinista.

            string columnas = string.Join(", ", tabla.ColumnasDatos) + ", DVH";
            string orderBy = string.Join(", ", tabla.ColumnasPk);

            string query = "SELECT " + columnas + " FROM " + tabla.Nombre + " ORDER BY " + orderBy;

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var registro = new Dictionary<string, object>();

                        foreach (string col in tabla.ColumnasDatos)
                            registro[col] = reader[col];

                        registro["DVH"] = reader["DVH"];

                        registros.Add(registro);
                    }
                }
            }

            return registros;
        }

        /// Actualiza el DVH de UN registro, identificado por su PK.
        /// Los valores de la PK van como parametros (son datos, no identificadores).
        public void ActualizarDVH(TablaProtegida tabla, string dvh, Dictionary<string, object> valoresPk)
        {
            // Construye "IdRol = @pk0 AND IdFamilia = @pk1" para PK simple o compuesta.
            var condiciones = new List<string>();
            int i = 0;
            foreach (string colPk in tabla.ColumnasPk)
            {
                condiciones.Add(colPk + " = @pk" + i);
                i++;
            }
            string where = string.Join(" AND ", condiciones);

            string query = "UPDATE " + tabla.Nombre + " SET DVH = @DVH WHERE " + where;

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@DVH", dvh);

                i = 0;
                foreach (string colPk in tabla.ColumnasPk)
                {
                    comando.Parameters.AddWithValue("@pk" + i, valoresPk[colPk]);
                    i++;
                }

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        /// Lee el DVV guardado de una tabla desde DigitoVerificador.
        /// Devuelve null si no existe la fila o si el DVV esta en NULL (base sin DV generado todavia).
        public string LeerDVV(string nombreTabla)
        {
            string query = "SELECT DVV FROM DigitoVerificador WHERE NombreTabla = @NombreTabla";

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@NombreTabla", nombreTabla);
                conexion.Open();

                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == System.DBNull.Value)
                    return null;

                return resultado.ToString();
            }
        }

        /// Guarda el DVV de una tabla en DigitoVerificador.
        /// La fila ya existe (la siembra el script SQL), asi que es un UPDATE.
        public void GuardarDVV(string nombreTabla, string dvv)
        {
            string query = "UPDATE DigitoVerificador SET DVV = @DVV WHERE NombreTabla = @NombreTabla";

            using (SqlConnection conexion = _conexionDAL.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@DVV", dvv);
                comando.Parameters.AddWithValue("@NombreTabla", nombreTabla);
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }

}
