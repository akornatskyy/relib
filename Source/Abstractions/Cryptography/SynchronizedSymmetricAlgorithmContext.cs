using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class SynchronizedSymmetricAlgorithmContext : Disposable, ISymmetricAlgorithmContext
    {
        private readonly SymmetricAlgorithm m_algorithm;
        private readonly LazyObject<ICryptoTransform> m_encryptor;
        private readonly LazyObject<ICryptoTransform> m_decryptor;

        public SynchronizedSymmetricAlgorithmContext(ISymmetricAlgorithmProvider provider)
            : this(provider.Create())
        {
        }

        public SynchronizedSymmetricAlgorithmContext(SymmetricAlgorithm algorithm)
        {
            m_algorithm = algorithm;
            m_encryptor = new LazyObject<ICryptoTransform>(() =>
            {
                lock (m_algorithm)
                {
                    return m_algorithm.CreateEncryptor();
                }
            });
            m_decryptor = new LazyObject<ICryptoTransform>(() =>
            {
                lock (m_algorithm)
                {
                    return m_algorithm.CreateDecryptor();
                }
            });
        }

        #region ISymmetricAlgorithmContext Members

        public bool EncryptorContext(Action<ICryptoTransform> action)
        {
            lock (m_encryptor)
            {
                action(m_encryptor.Object);
            }

            return true;
        }

        public bool DecryptorContext(Action<ICryptoTransform> action)
        {
            lock (m_decryptor)
            {
                action(m_decryptor.Object);
            }

            return true;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_encryptor.Loaded)
                {
                    Disposable.ReleaseFactory(m_encryptor.Object);
                    m_encryptor.Reset();
                }

                if (m_decryptor.Loaded)
                {
                    Disposable.ReleaseFactory(m_decryptor.Object);
                    m_decryptor.Reset();
                }

                Disposable.ReleaseFactory(m_algorithm);
            }
        }
    }
}
