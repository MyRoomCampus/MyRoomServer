using System.Security.Cryptography;
using System.Text;

namespace MyRoomServer.Extentions
{
    public static class StringExtention
    {
        public static string Sha256(this string str, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str + salt));
            var stringbuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                stringbuilder.Append(bytes[i].ToString("x2"));
            }
            return stringbuilder.ToString();
        }
    }
}
