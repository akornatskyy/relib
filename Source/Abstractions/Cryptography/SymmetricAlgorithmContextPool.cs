using System;
using System.Diagnostics;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public class SymmetricAlgorithmContextPool : Disposable, ISymmetricAlgorithmContext
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("SymmetricAlgorithmPool"));

        private readonly object m_sync = new object();
        private readonly SymmetricAlgorithmOptions m_options;
        private readonly ISymmetricAlgorithmProvider m_provider;

        public SymmetricAlgorithmContextPool(SymmetricAlgorithmOptions options, ISymmetricAlgorithmProvider provider)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            m_options = options;
            m_provider = provider;
            Pool = new WaitPool<ISymmetricAlgorithmContext>(
                        new LazyPool<ISymmetricAlgorithmContext>(
                            new SynchronizedPool<ISymmetricAlgorithmContext>(m_sync,
                                new StackPool<ISymmetricAlgorithmContext>(String.Concat("SymmetricAlgorithm ", options.Name), options.MaxPoolSize),
                                options.PoolAccessTimeout),
                            CreateFactory, ReleaseFactory),
                    options.PoolWaitTimeout);
        }

        public IPool<ISymmetricAlgorithmContext> Pool { get; set; }

        public bool EncryptorContext(Action<ICryptoTransform> action)
        {
            using (var pooled = new Pooled<ISymmetricAlgorithmContext>(Pool))
            {
                var item = pooled.Item;
                if (item == null)
                {
                    if (g_traceInfo.IsVerboseEnabled)
                    {
                        TraceHelper.TraceVerbose(g_traceInfo, "{0} - There are no free encryptors available", m_options.Name);
                    }

                    return false;
                }

                return item.EncryptorContext(action);
            }
        }

        public bool DecryptorContext(Action<ICryptoTransform> action)
        {
            using (var pooled = new Pooled<ISymmetricAlgorithmContext>(Pool))
            {
                var item = pooled.Item;
                if (item == null)
                {
                    if (g_traceInfo.IsVerboseEnabled)
                    {
                        TraceHelper.TraceVerbose(g_traceInfo, "{0} - There are no free decryptors available", m_options.Name);
                    }

                    return false;
                }

                return item.DecryptorContext(action);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Pool.Dispose();
            }
        }

        private ISymmetricAlgorithmContext CreateFactory(object state)
        {
            return new SymmetricAlgorithmContext(m_provider);
        }

        private void ReleaseFactory(ISymmetricAlgorithmContext item)
        {
            Disposable.ReleaseFactory(item);
        }
    }
}
