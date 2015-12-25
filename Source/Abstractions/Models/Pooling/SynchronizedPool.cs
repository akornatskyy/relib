using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    public class SynchronizedPool<T> : DecoratedPool<T>
        where T : class
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("SynchronizedPool"));

        public SynchronizedPool(object syncRoot, IPool<T> inner)
            : this(syncRoot, inner, System.Threading.Timeout.Infinite)
        {
        }

        public SynchronizedPool(object syncRoot, IPool<T> inner, TimeSpan accessTimeout)
            : this(syncRoot, inner, TimeSpanHelper.Timeout(accessTimeout))
        {
        }

        public SynchronizedPool(object syncRoot, IPool<T> inner, int timeout)
            : base(inner)
        {
            SyncRoot = syncRoot;
            AccessTimeout = timeout;
        }

        public override bool Clear()
        {
            using (var @lock = new MonitorLockScope(SyncRoot, AccessTimeout))
            {
                if (!@lock.Aquired)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, "{0} - Clear timed out", Name);
                    }

                    return false;
                }

                return Inner.Clear();
            }
        }

        public override bool Return(T item)
        {
            using (var @lock = new MonitorLockScope(SyncRoot, AccessTimeout))
            {
                if (!@lock.Aquired)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, "{0} - Return timed out", Name);
                    }

                    return false;
                }

                return Inner.Return(item);
            }
        }

        public override T Take(object state)
        {
            using (var @lock = new MonitorLockScope(SyncRoot, AccessTimeout))
            {
                if (!@lock.Aquired)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, "{0} - Take timed out", Name);
                    }

                    return default(T);
                }

                return Inner.Take(state);
            }
        }

        public override int Count
        {
            get
            {
                using (var @lock = new MonitorLockScope(SyncRoot, AccessTimeout))
                {
                    if (!@lock.Aquired)
                    {
                        if (g_traceInfo.IsWarningEnabled)
                        {
                            TraceHelper.TraceWarning(g_traceInfo, "{0} - Count timed out", Name);
                        }

                        return -1;
                    }

                    return Inner.Count;
                }
            }
        }

        protected int AccessTimeout { get; private set; }

        protected object SyncRoot { get; private set; }
    }
}
