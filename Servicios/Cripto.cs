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
                byte[] bytesTexto = Encoding.UTF8.GetBytes(texto);
                byte[] bytesHash = sha256.ComputeHash(bytesTexto);

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
