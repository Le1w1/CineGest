using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Cripto
    {

        public string ObtenerHashSha256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir el texto a un arreglo de bytes
                byte[] bytesTexto = Encoding.UTF8.GetBytes(texto);
                // Calcular el hash SHA256
                byte[] bytesHash = sha256.ComputeHash(bytesTexto);
                // Convertir el hash a una cadena hexadecimal
                StringBuilder resultado = new StringBuilder();

                foreach (byte b in bytesHash)
                {
                    resultado.Append(b.ToString("x2"));
                }

                return resultado.ToString();
            }
        }
    }
}
