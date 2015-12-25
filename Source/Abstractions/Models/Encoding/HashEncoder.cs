using System;
using ReusableLibrary.Abstractions.Cryptography;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class HashEncoder : IEncoder
    {
        private readonly IEncoder m_inner;
        private readonly IHashAlgorithmProvider m_algorithmProvider;

        public HashEncoder(IHashAlgorithmProvider algorithmProvider, IEncoder inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            if (algorithmProvider == null)
            {
                throw new ArgumentNullException("algorithmProvider");
            }

            m_inner = inner;
            m_algorithmProvider = algorithmProvider;
        }

        #region IEncoder Members

        public byte[] GetBytes(string s)
        {
            var algorithm = m_algorithmProvider.Create();
            var bytes = algorithm.ComputeHash(m_inner.GetBytes(s));
            algorithm.Clear();
            return bytes;
        }

        #endregion
    }
}
