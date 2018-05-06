using System;
using System.Security.Cryptography;
using System.Text;

namespace AwsSig4Authentication
{
    public static class Extensions
    {
        public static byte[] ToBytes(this string content)
        {
            return Encoding.UTF8.GetBytes(content.ToCharArray());
        }

        public static byte[] Hash(this byte[] bytes)
        {
            return SHA256.Create().ComputeHash(bytes);
        }

        public static string HexEncode(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}