using DAL;
using Servicios.DigitoVerificador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// Logica de negocio del Digito Verificador. Orquesta las tres piezas:
    ///   - DigitoVerificador (Servicios): la formula (concatenar + hashear).
    ///   - TablasProtegidas (Servicios): que columnas y PK tiene cada tabla.
    ///   - DigitoVerificadorDAL (DAL): leer/escribir DVH por fila y DVV por tabla.
    

    /// Expone dos operaciones
    ///   - RecalcularDV(tabla): modo GENERACION. Recalcula y persiste el DVH de
    ///     cada fila y el DVV de la tabla. Se llama despues de cada escritura.
    ///   - VerificarIntegridad(): modo DETECCION. Recalcula y compara contra lo
    ///     guardado; devuelve la lista de inconsistencias. Se llama en el Login.

    public class DigitoVerificadorBLL
    {
        private readonly DigitoVerificadorDAL _dvDAL;
        private readonly DigitoVerificador _digitoVerificador;

        public DigitoVerificadorBLL()
        {
            _dvDAL = new DigitoVerificadorDAL();
            _digitoVerificador = new DigitoVerificador();
        }

        //  MODO GENERACION
        /// Recalcula y persiste los Digitos Verificadores de UNA tabla:
        /// el DVH de cada fila y el DVV de la tabla. Se invoca despues de cada
        /// alta/modificacion/baja sobre esa tabla.
        
        public void RecalcularDV(string nombreTabla)
        {
            TablaProtegida tabla = ObtenerTabla(nombreTabla);
            var registros = _dvDAL.LeerRegistros(tabla);

            var dvhCalculados = new List<string>();

            foreach (var registro in registros)
            {
                // DVH de la fila: se calcula con las columnas de datos EN ORDEN.
                string dvh = CalcularDVHDeRegistro(tabla, registro);

                // Persistir el DVH en la fila, identificada por su PK.
                var valoresPk = ExtraerPk(tabla, registro);
                _dvDAL.ActualizarDVH(tabla, dvh, valoresPk);

                dvhCalculados.Add(dvh);
            }

            // DVV de la tabla: hash de la concatenacion de los DVH ordenados por
            // PK. Los registros ya vienen ordenados por PK del DAL, asi que
            // dvhCalculados respeta ese orden.
            string dvv = _digitoVerificador.CalcularDVV(dvhCalculados);
            _dvDAL.GuardarDVV(tabla.Nombre, dvv);
        }

        
        /// Recalcula el DV de TODAS las tablas protegidas. Util para el arranque
        /// inicial del sistema de cero o para la reparacion por recalculo.
        public void RecalcularTodo()
        {
            foreach (TablaProtegida tabla in TablasProtegidas.Todas)
                RecalcularDV(tabla.Nombre);
        }

     

        //  MODO DETECCION
        /// Verifica la integridad de todas las tablas protegidas.
        /// Devuelve la lista de inconsistencias encontradas (vacia = todo integro).
        /// NO se corta en la primera: reporta todas las filas y tablas afectadas.
        ///
        /// Una tabla con DVV guardado en NULL se considera "sin DV generado
        /// todavia" (base recien instalada), NO corrupta: se saltea.
        public List<Inconsistencia> VerificarIntegridad()
        {
            var inconsistencias = new List<Inconsistencia>();

            foreach (TablaProtegida tabla in TablasProtegidas.Todas)
            {
                string dvvGuardado = _dvDAL.LeerDVV(tabla.Nombre);

                // Base sin DV generado: no es corrupcion, se saltea la tabla.
                if (dvvGuardado == null)
                    continue;

                var registros = _dvDAL.LeerRegistros(tabla);
                var dvhCalculados = new List<string>();

                foreach (var registro in registros)
                {
                    string dvhCalculado = CalcularDVHDeRegistro(tabla, registro);
                    dvhCalculados.Add(dvhCalculado);

                    string dvhGuardado = registro["DVH"] == null || registro["DVH"] == System.DBNull.Value? null: registro["DVH"].ToString();

                    // DVH de fila: si no coincide, ese registro fue modificado.
                    if (dvhCalculado != dvhGuardado)
                    {
                        inconsistencias.Add(new Inconsistencia
                        {
                            Tabla = tabla.Nombre,
                            Tipo = Inconsistencia.TipoInconsistencia.DVH,
                            IdentificadorPk = ConstruirIdentificadorPk(tabla, registro)
                        });
                    }
                }

                // DVV de tabla: si no coincide, se altero el conjunto (alta/baja).
                string dvvCalculado = _digitoVerificador.CalcularDVV(dvhCalculados);

                if (dvvCalculado != dvvGuardado)
                {
                    inconsistencias.Add(new Inconsistencia
                    {
                        Tabla = tabla.Nombre,
                        Tipo = Inconsistencia.TipoInconsistencia.DVV,
                        IdentificadorPk = string.Empty
                    });
                }
            }

            return inconsistencias;
        }

        //  AUXILIARES
        /// Calcula el DVH de un registro tomando sus columnas de datos EN EL
        /// ORDEN definido en TablaProtegida. Es el mismo metodo usado al generar
        /// y al verificar: por eso nunca se desincronizan.
        private string CalcularDVHDeRegistro(TablaProtegida tabla, Dictionary<string, object> registro)
        {
            var valores = tabla.ColumnasDatos.Select(col => registro[col]);
            return _digitoVerificador.CalcularDVH(valores);
        }

        /// Extrae del registro solo las columnas que forman la PK.
        private Dictionary<string, object> ExtraerPk(TablaProtegida tabla, Dictionary<string, object> registro)
        {
            var pk = new Dictionary<string, object>();
            foreach (string colPk in tabla.ColumnasPk)
                pk[colPk] = registro[colPk];
            return pk;
        }

        /// Arma el identificador legible de la PK para reportar la fila.
        /// Ej: "IdUsuario=5" o "IdRol=3, IdFamilia=7".
        private string ConstruirIdentificadorPk(TablaProtegida tabla, Dictionary<string, object> registro)
        {
            var partes = tabla.ColumnasPk.Select(col => col + "=" + registro[col]);
            return string.Join(", ", partes);
        }

        /// Busca la definicion de una tabla protegida por nombre. 
        private TablaProtegida ObtenerTabla(string nombreTabla)
        {
            TablaProtegida tabla = TablasProtegidas.Todas.FirstOrDefault(t => t.Nombre == nombreTabla);

            if (tabla == null)
                throw new System.Exception("La tabla '" + nombreTabla + "' no esta registrada como tabla protegida.");

            return tabla;
        }
    }
}
