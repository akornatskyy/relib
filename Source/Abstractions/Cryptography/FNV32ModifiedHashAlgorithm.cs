using System;
using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class FNV32ModifiedHashAlgorithm : HashAlgorithm
    {
        // http://home.comcast.net/~bretm/hash/6.html
        private const uint Prime = 16777619;
        private const uint Initial = 2166136261;

        private uint m_hash;

        public FNV32ModifiedHashAlgorithm()
        {
            HashSizeValue = 32;
        }

        public override void Initialize()
        {
            m_hash = Initial;
        }

        protected override void HashCore(byte[] array, int start, int size)
        {
            int length = start + size;
            for (int i = start; i < length; i++)
            {
                m_hash = (m_hash * Prime) ^ array[i];
            }
        }

        protected override byte[] HashFinal()
        {
            var hash = m_hash;
            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;
            return BitConverter.GetBytes(hash);
        }
    }
}
