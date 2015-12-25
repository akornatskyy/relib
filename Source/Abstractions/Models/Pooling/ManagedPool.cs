using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    [DebuggerDisplay("Disposed = {m_disposed}, Count = {Count}")]
    public abstract class ManagedPool<T> : DecoratedPool<T>
        where T : class
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("ManagedPool"));

        private readonly Action<T> m_releasefactory;
        private bool m_disposed;

        protected ManagedPool(IPool<T> inner, Action<T> releasefactory)
            : base(inner)
        {
            m_releasefactory = releasefactory;
        }

        #region DecoratedPool<T> Members

        public override bool Return(T item)
        {
            if (!m_disposed && Inner.Return(item))
            {
                return true;
            }

            if (g_traceInfo.IsWarningEnabled)
            {
                TraceHelper.TraceWarning(g_traceInfo, "{0} - Releasing an item failed to return", Name);
            }

            m_releasefactory(item);
            return false;
        }

        public override bool Clear()
        {
            T t;
            while ((t = Inner.Take(null)) != default(T))
            {
                m_releasefactory(t);
            }

            if (Count != 0)
            {
                throw new InvalidOperationException("Count != 0");
            }

            return true;
        }

        #endregion

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear();
                Inner.Dispose();
                m_disposed = true;
            }
        }

        #endregion
    }
}
