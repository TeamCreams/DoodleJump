using System;
using System.Linq;
using System.Security.Cryptography;

namespace random_alphanumeric_strings
{
    public class SecureKey
    {
        public static string method3(int length)
        {
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var bits = (length * 6);
                var byte_size = ((bits + 7) / 8);
                var bytesarray = new byte[byte_size];
                crypto.GetBytes(bytesarray);
                return Convert.ToBase64String(bytesarray);
            }
        }
    }
}