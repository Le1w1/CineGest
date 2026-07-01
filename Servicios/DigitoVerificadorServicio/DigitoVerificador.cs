using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.DigitoVerificador
{
    public class DigitoVerificador
    {
        /// Calcula los Digitos Verificadores del sistema.
        ///
        /// Responsabilidad UNICA: dada una lista de valores ya ordenada, los une
        /// con un separador y devuelve el hash (DVH). Y dada una lista de DVH ya
        /// ordenada por PK, los une y devuelve el hash (DVV).
        ///
        /// NO conoce tablas, ni columnas, ni SQL. El "que campos van y en que orden"
        /// vive en la definicion de tablas y en el DAL. De esta forma, generar y
        /// verificar usan exactamente el mismo metodo y es imposible que la formula
        /// se desincronice.
        
        
            // Separador entre campos. Garantiza que la concatenacion sea correcta:
            // registros distintos producen cadenas distintas. Sin separador,
            // Por ejemplo: ("USR","CrearUsuario") y ("USRCrear","Usuario") colisionarian.
            private const string SEPARADOR = "|";

            private readonly Cripto _cripto;

            public DigitoVerificador()
            {
                _cripto = new Cripto();
            }

            /// Calcula el DVH (Digito Verificador Horizontal) de un registro.
            /// 
            /// Recibe los valores de los campos del registro YA EN EL ORDEN 
            /// para esa tabla. Los une con "|" y los hashea con SHA-256.
            ///
            /// IMPORTANTE: la columna DVH del registro NUNCA debe estar entre los
            /// valores que se pasan aca (un digito no se verifica a si mismo).
            public string CalcularDVH(IEnumerable<object> valoresCampos)
            {
                if (valoresCampos == null)
                    throw new ArgumentNullException(nameof(valoresCampos));

                string concatenado = string.Join(SEPARADOR, valoresCampos.Select(Normalizar));

                return _cripto.ObtenerHashSha256(concatenado);
            }

            /// Calcula el DVV (Digito Verificador Vertical) de una tabla.
            /// 
            /// Recibe los DVH de TODOS los registros de la tabla YA ORDENADOS POR PK.
            /// Los une con "|" y los hashea. El orden por PK es obligatorio: garantiza
            /// que el resultado sea determinista (mismo conjunto -> mismo DVV).
            public string CalcularDVV(IEnumerable<string> dvhOrdenadosPorPk)
            {
                if (dvhOrdenadosPorPk == null)
                    throw new ArgumentNullException(nameof(dvhOrdenadosPorPk));

                // Cada DVH ya es un string; lo normalizamos por las dudas (null -> "").
                string concatenado = string.Join(SEPARADOR, dvhOrdenadosPorPk.Select(Normalizar));

                return _cripto.ObtenerHashSha256(concatenado);
            }

            /// Convierte un valor de campo a su representacion textual estable.
            /// 
            /// Reglas congeladas:
            ///   - null  -> cadena vacia
            ///   - bool  -> "1" / "0" (para coincidir con la representacion de SQL)
            ///   - resto -> ToString con CultureInfo.InvariantCulture (independiente
            ///              del idioma/region de la maquina: el decimal siempre con
            ///              punto, etc.)
            private static string Normalizar(object valor)
            {
                if (valor == null || valor == DBNull.Value)
                    return string.Empty;

                if (valor is bool b)
                    return b ? "1" : "0";

                if (valor is IFormattable formattable)
                    return formattable.ToString(null, CultureInfo.InvariantCulture);

                return valor.ToString();
       
            }
    }
}

