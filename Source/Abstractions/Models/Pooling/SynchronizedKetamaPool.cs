using System.Diagnostics;
using System.Threading;
using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class SynchronizedKetamaPool<T> : KetamaPool<T>
        where T : class
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("SynchronizedKetamaPool"));

        private readonly int m_timeout;
        private readonly ReaderWriterLockScope m_lockScope;

        public SynchronizedKetamaPool(string name, IHashAlgorithmProvider hashAlgorithmProvider, int timeout)
            : base(name, hashAlgorithmProvider)
        {
            m_timeout = timeout;
            m_lockScope = new ReaderWriterLockScope(new ReaderWriterLock());
        }

        public override T Take(object state)
        {
            T item = state as T;
            using (var @rlock = m_lockScope.Reader(m_timeout))
            {
                if (!@rlock.Aquired)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, "{0} - Take timed out", Name);
                    }

                    return default(T);
                }

                if (item != null)
                {
                    using (var @wlock = m_lockScope.UpgradeToWriter(m_timeout))
                    {
                        if (!@wlock.Aquired)
                        {
                            if (g_traceInfo.IsWarningEnabled)
                            {
                                TraceHelper.TraceWarning(g_traceInfo, "{0} - Take timed out on remove", Name);
                            }

                            return default(T);
                        }

                        if (!Remove(item))
                        {
                            if (g_traceInfo.IsWarningEnabled)
                            {
                                TraceHelper.TraceWarning(g_traceInfo, "{0} - Failed to remove", Name);
                            }

                            return default(T);
                        }
                    }
                }
                else
                {
                    item = Lookup(state);
                }
            }

            return item;
        }

        public override bool Return(T item)
        {
            using (var @lock = m_lockScope.Writer(m_timeout))
            {
                if (!@lock.Aquired)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, "{0} - Return timed out", Name);
                    }

                    return false;
                }

                return base.Return(item);
            }
        }
    }
}
