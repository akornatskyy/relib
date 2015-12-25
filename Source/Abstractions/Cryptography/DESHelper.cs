using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public static class DESHelper
    {
        private static readonly Encoding g_encoding = Encoding.UTF8;

        public static Pair<byte[]> CreateKeyVector(string passphrase)
        {
            var bytes = g_encoding.GetBytes(passphrase);
            var hashAlgorithm = new SHA1CryptoServiceProvider();
            var hash = hashAlgorithm.ComputeHash(bytes);
            hashAlgorithm.Clear();
            var cryptoKey = new byte[8];
            var cryptoIV = new byte[8];
            Buffer.BlockCopy(hash, 0, cryptoKey, 0, 8);
            Buffer.BlockCopy(hash, 8, cryptoIV, 0, 8);

            return new Pair<byte[]>(cryptoKey, cryptoIV);
        }

        public static ICryptoTransform CreateEncryptor(DES provider, string key)
        {
            var kv = CreateKeyVector(key);
            return provider.CreateEncryptor(kv.First, kv.Second);
        }

        public static ICryptoTransform CreateDecryptor(DES provider, string key)
        {
            var kv = CreateKeyVector(key);
            return provider.CreateDecryptor(kv.First, kv.Second);
        }
    }
}
