using System.Security.Cryptography;
using System.Text;

namespace OnePWA.Helpers
{
    public class EncriptacionHelper
    {
        public static string GetHash(string texto)
        {
            texto += "SGTCURSO1";//Agregar salt
            using SHA256 sha256 = SHA256.Create();// Convertir el texto a bytes
            byte[] bytes = Encoding.UTF8.GetBytes(texto); // Calcular el hash
            byte[] hashBytes = sha256.ComputeHash(bytes); // Convertir el hash a formato hexadecimal
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2")); // Formato hexadecimal
            }
            return sb.ToString();
        }
    }
}
