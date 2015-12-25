using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public static class RC2Helper
    {
        private static readonly Encoding g_encoding = Encoding.UTF8;

        public static Pair<byte[]> CreateKeyVector(string passphrase, int keySize)
        {
            if (keySize < 40 || keySize > 128)
            {
                throw new ArgumentOutOfRangeException("keySize");
            }

            var bytes = g_encoding.GetBytes(passphrase);
            var hashAlgorithm = new SHA1CryptoServiceProvider();
            var hash = hashAlgorithm.ComputeHash(bytes);
            hashAlgorithm.Clear();
            var cryptoKey = new byte[keySize / 8];
            var cryptoIV = new byte[8];
            Buffer.BlockCopy(hash, 0, cryptoKey, 0, cryptoKey.Length);
            Buffer.BlockCopy(hash, 12, cryptoIV, 0, 8);
            return new Pair<byte[]>(cryptoKey, cryptoIV);
        }

        public static ICryptoTransform CreateEncryptor(RC2 provider, string key, int keySize)
        {
            var kv = CreateKeyVector(key, keySize);
            return provider.CreateEncryptor(kv.First, kv.Second);
        }

        public static ICryptoTransform CreateDecryptor(RC2 provider, string key, int keySize)
        {
            var kv = CreateKeyVector(key, keySize);
            return provider.CreateDecryptor(kv.First, kv.Second);
        }
    }
}
