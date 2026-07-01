using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.DigitoVerificador
{
    /// Describe una tabla protegida por Digito Verificador.
    /// 
    /// Es la UNICA fuente de verdad sobre que columnas entran al DVH y en que
    /// orden. Tanto la generacion como la verificacion leen de aca, nunca de
    /// listas escritas a mano por separado: asi es imposible que el orden de
    /// campos se desincronice entre generar y verificar.
    
    public class TablaProtegida
    {
        /// Nombre de la tabla en la BD
        public string Nombre { get; }


        /// Columnas que entran al calculo del DVH, EN EL ORDEN CONGELADO.
        /// NO incluye la columna DVH (un digito no se verifica a si mismo).
        /// Para las tablas con datos, incluye tambien la PK .
        
        public IReadOnlyList<string> ColumnasDatos { get; }



        /// Columnas que forman la clave primaria. Sirven para construir el
        /// identificador del registro que se reporta como inconsistente.
        /// 1 columna en tablas con PK simple, 2 en las tablas puente.
        
        public IReadOnlyList<string> ColumnasPk { get; }

        public TablaProtegida(string nombre, string[] columnasDatos, string[] columnasPk)
        {
            Nombre = nombre;
            ColumnasDatos = columnasDatos;
            ColumnasPk = columnasPk;
        }
    }

    /// Catalogo de las 10 tablas protegidas por Digito Verificador.
    /// BitacoraEvento queda EXCLUIDA por no ser tabla sensible

    /// El orden de las columnas en ColumnasDatos es el ORDEN CONGELADO del DVH.
    /// Cambiar este orden obliga a regenerar todos los DVH de la base.
    
    public static class TablasProtegidas
    {
        public static readonly IReadOnlyList<TablaProtegida> Todas = new List<TablaProtegida>
        {
            // ----- Tablas con datos (PK simple) -----
 
            new TablaProtegida("Usuario",
                new[] { "IdUsuario", "Nombre", "Apellido", "DNI", "Email", "NombreUsuario",
                        "PasswordHash", "Activo", "Bloqueado", "IntentosFallidos",
                        "DebeCambiarClave", "IdIdioma", "IdRol" },
                new[] { "IdUsuario" }),

            new TablaProtegida("Rol",
                new[] { "IdRol", "Nombre" },
                new[] { "IdRol" }),

            new TablaProtegida("Familia",
                new[] { "IdFamilia", "Nombre" },
                new[] { "IdFamilia" }),

            new TablaProtegida("PermisoSimple",
                new[] { "IdPermisoSimple", "Codigo", "Nombre" },
                new[] { "IdPermisoSimple" }),

            new TablaProtegida("Idioma",
                new[] { "IdIdioma", "Codigo", "Nombre" },
                new[] { "IdIdioma" }),
 
            new TablaProtegida("RespaldoBD",
                new[] { "IdRespaldo", "FechaHora", "TipoOperacion","RutaArchivo",
                        "NombreArchivo","Resultado","Descripcion","IdUsuario"},
                new[] { "IdRespaldo" }),

            // ----- Tablas puente (PK compuesta) -----
            // Sus unicas columnas son las dos FK: son a la vez su PK y todo su
            // contenido, por eso ColumnasDatos == ColumnasPk.
 
            new TablaProtegida("Rol_Familia",
                new[] { "IdRol", "IdFamilia" },
                new[] { "IdRol", "IdFamilia" }),

            new TablaProtegida("Rol_PermisoSimple",
                new[] { "IdRol", "IdPermisoSimple" },
                new[] { "IdRol", "IdPermisoSimple" }),

            new TablaProtegida("Familia_Familia",
                new[] { "IdFamiliaPadre", "IdFamiliaHija" },
                new[] { "IdFamiliaPadre", "IdFamiliaHija" }),

            new TablaProtegida("PermisoSimple_Familia",
                new[] { "IdFamilia", "IdPermisoSimple" },
                new[] { "IdFamilia", "IdPermisoSimple" }),
        };
    }
}
