using System.Diagnostics;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class Pooled<T> : Disposable
        where T : class
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("Pooled"));

        private readonly IPool<T> m_pool;

        public Pooled(IPool<T> pool)
            : this(pool, null)
        {
        }

        public Pooled(IPool<T> pool, object state)
        {
            m_pool = pool;
            Item = m_pool.Take(state);
        }

        public T Item { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Item != default(T))
            {
                if (!m_pool.Return(Item))
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, "Failed to return an item to the {0} pool", m_pool.Name);
                    }
                }

                Item = default(T);
            }
        }
    }
}
