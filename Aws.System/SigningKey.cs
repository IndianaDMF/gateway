using System.Security.Cryptography;
using System.Text;

namespace Aws.System
{
    public class SigningKey
    {

        private readonly string algorithm = "HmacSHA256";
        private readonly string request = "aws4_request";
        private readonly string version = "AWS4";

        private byte[] HmacSHA256(string data, byte[] key)
        {            
            KeyedHashAlgorithm kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;
            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes((version + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp, kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256(request, kService);
            return kSigning;
        }
    }
}
