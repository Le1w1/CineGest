using Servicios.DigitoVerificador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.DigitoVerificadorServicio
{
    /// Se lanza durante el Login cuando la verificacion de Digito Verificador
    /// detecta que la integridad de una o mas tablas protegidas fue comprometida.
    ///
    /// Lleva la lista de inconsistencias detectadas para que la UI pueda mostrar
    /// al administrador que tablas/registros estan afectados y ofrecer la reparacion.
    public class IntegridadComprometidaException : Exception
    {
        public IReadOnlyList<Inconsistencia> Inconsistencias { get; }

        public IntegridadComprometidaException(string mensaje, IReadOnlyList<Inconsistencia> inconsistencias): base(mensaje)
        {
            Inconsistencias = inconsistencias;
        }
    }
}
