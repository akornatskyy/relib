using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class SymmetricAlgorithmContext : Disposable, ISymmetricAlgorithmContext
    {
        private readonly SymmetricAlgorithm m_algorithm;
        private readonly LazyObject<ICryptoTransform> m_lazyEncryptor;
        private readonly LazyObject<ICryptoTransform> m_lazyDecryptor;

        public SymmetricAlgorithmContext(ISymmetricAlgorithmProvider provider)
            : this(provider.Create())
        {
        }

        public SymmetricAlgorithmContext(SymmetricAlgorithm algorithm)
        {
            m_algorithm = algorithm;
            m_lazyEncryptor = new LazyObject<ICryptoTransform>(() => m_algorithm.CreateEncryptor());
            m_lazyDecryptor = new LazyObject<ICryptoTransform>(() => m_algorithm.CreateDecryptor());
        }

        #region ISymmetricAlgorithmContext Members

        public bool EncryptorContext(Action<ICryptoTransform> action)
        {
            action(m_lazyEncryptor.Object);
            return true;
        }

        public bool DecryptorContext(Action<ICryptoTransform> action)
        {
            action(m_lazyDecryptor.Object);
            return true;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_lazyEncryptor.Loaded)
                {
                    Disposable.ReleaseFactory(m_lazyEncryptor.Object);
                    m_lazyEncryptor.Reset();
                }

                if (m_lazyDecryptor.Loaded)
                {
                    Disposable.ReleaseFactory(m_lazyDecryptor.Object);
                    m_lazyDecryptor.Reset();
                }

                Disposable.ReleaseFactory(m_algorithm);
            }
        }
    }
}
