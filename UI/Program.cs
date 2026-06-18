using Servicios;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Cargar el idioma por defecto (ES) en el Traductor antes de abrir el Login.
            // Asi el frmLogin ya sale traducido. Si falla (no esta el JSON), seguimos:
            // los textos se mostraran como las claves crudas, lo cual hace evidente el problema.
            try { Traductor.Instancia.CargarIdioma("ES"); } catch { }

            Application.Run(new frmLogin());
        }
    }
}
