

namespace Servicios.DigitoVerificador
{
    /// Representa una inconsistencia de integridad detectada por el Digito Verificador. 
    
    /// Puede ser de dos tipos:
    ///   - DVH: un registro concreto fue modificado (su DVH recalculado no coincide con el guardado). Se identifica por su PK.
    ///   - DVV: el conjunto de la tabla cambio (se inserto o elimino una fila por fuera del sistema). No apunta a una fila concreta.
  
    public class Inconsistencia
    {
        public enum TipoInconsistencia
        {
            DVH,   // registro individual modificado
            DVV    // conjunto de la tabla alterado (alta/baja no autorizada)
        }

        /// Tabla afectada.
        public string Tabla { get; set; }

        /// Tipo de inconsistencia (DVH de fila o DVV de tabla).
        public TipoInconsistencia Tipo { get; set; }

        /// Identificador del registro afectado (solo para tipo DVH).
        /// Ej: "IdUsuario=5" o "IdRol=3, IdFamilia=7". Vacio para tipo DVV.
        public string IdentificadorPk { get; set; }

        /// Constructor para inconsistencia de tipo DVH (registro individual).
        public override string ToString()
        {
            if (Tipo == TipoInconsistencia.DVV)
                return "Tabla " + Tabla + ": el conjunto de registros fue alterado (alta o baja no autorizada).";

            return "Tabla " + Tabla + ", registro " + IdentificadorPk + ": fue modificado fuera del sistema.";
        }
    }
}

