using System;
using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class FNV32HashAlgorithm : HashAlgorithm
    {
        // http://www.isthe.com/chongo/tech/comp/fnv/
        private const uint Prime = 16777619;
        private const uint Initial = 2166136261;

        private uint m_hash;

        public FNV32HashAlgorithm()
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
            return BitConverter.GetBytes(m_hash);
        }
    }
}
