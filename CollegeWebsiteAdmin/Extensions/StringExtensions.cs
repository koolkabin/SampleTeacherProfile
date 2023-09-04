using System.Security.Cryptography;
using System.Text;

namespace CollegeWebsiteAdmin.Extensions
{
    public static class StringExtensions
    {
        public static string EncryptSha256(this string input)
        {
            using (SHA256 s = SHA256.Create())
            {
                byte[] bytes = s.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    b.Append(bytes[i].ToString("x2"));
                }
                return b.ToString();
            }
        }
    }
}
