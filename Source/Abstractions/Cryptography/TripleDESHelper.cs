using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public static class TripleDESHelper
    {
        private static readonly Encoding g_encoding = Encoding.UTF8;

        public static Pair<byte[]> CreateKeyVector(string passphrase, int keySize)
        {
            if (keySize < 128 || keySize > 192)
            {
                throw new ArgumentOutOfRangeException("keySize");
            }

            var bytes = g_encoding.GetBytes(passphrase);
            var hashAlgorithm = new SHA256Managed();
            var hash = hashAlgorithm.ComputeHash(bytes);
            hashAlgorithm.Clear();
            var cryptoKey = new byte[keySize / 8];
            var cryptoIV = new byte[8];
            Buffer.BlockCopy(hash, 0, cryptoKey, 0, cryptoKey.Length);
            Buffer.BlockCopy(hash, 24, cryptoIV, 0, 8);
            return new Pair<byte[]>(cryptoKey, cryptoIV);
        }

        public static ICryptoTransform CreateEncryptor(TripleDES provider, string key, int keySize)
        {
            var kv = CreateKeyVector(key, keySize);
            return provider.CreateEncryptor(kv.First, kv.Second);
        }

        public static ICryptoTransform CreateDecryptor(TripleDES provider, string key, int keySize)
        {
            var kv = CreateKeyVector(key, keySize);
            return provider.CreateDecryptor(kv.First, kv.Second);
        }
    }
}
