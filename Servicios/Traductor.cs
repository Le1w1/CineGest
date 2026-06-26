using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios
{
    
    public class Traductor
    {
        private static Traductor _instancia;
        // Diccionario para almacenar las traducciones cargadas desde el archivo JSON
        private Dictionary<string, string> _traducciones;
        // Variable para almacenar el código del idioma actual
        private string _codigoIdiomaActual;

        private Traductor()
        {
            _traducciones = new Dictionary<string, string>();
            // Inicializar el idioma por defecto (por ejemplo, español)
            _codigoIdiomaActual = "ES";
        }

        // Propiedad para acceder a la instancia única de Traductor
        public static Traductor Instancia
        {
            get
            {
                if (_instancia == null) _instancia = new Traductor();
                return _instancia;
            }
        }

        public string CodigoIdiomaActual { get { return _codigoIdiomaActual; } }

       
        public void CargarIdioma(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Debe indicar el codigo de idioma a cargar.");

            string codigoNormalizado = codigo.Trim().ToUpper();

            // Construir la ruta completa del archivo JSON de idioma
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory;
            string rutaArchivo = Path.Combine(rutaBase, "Recursos", "Idiomas", codigoNormalizado.ToLower() + ".json");

            if (!File.Exists(rutaArchivo))
                throw new Exception("No se encontro el archivo de idioma: " + rutaArchivo);

            string contenido = File.ReadAllText(rutaArchivo, Encoding.UTF8);

            // Deserializar el contenido del archivo JSON a un diccionario
            var diccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(contenido);

            _traducciones = diccionario ?? new Dictionary<string, string>();
            _codigoIdiomaActual = codigoNormalizado;
        }

        
        /// Si la clave no existe en el JSON, devuelve la propia clave asi el form no queda en blanco y vemos cual falta
        public string Traducir(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave)) return string.Empty;
            // Intentar obtener la traducción desde el diccionario
            if (_traducciones != null && _traducciones.TryGetValue(clave, out string valor))
            {
                return valor;
            }

            return clave;
        }
    }
}
