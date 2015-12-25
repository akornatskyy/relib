using System.Diagnostics;
using System.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class WaitPool<T> : DecoratedPool<T>
        where T : class
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("WaitPool"));

        private readonly Semaphore m_semaphore;

        public WaitPool(IPool<T> inner, int waitTimeout)
            : base(inner)
        {
            WaitTimeout = waitTimeout;
            m_semaphore = new Semaphore(inner.Size - inner.Count, inner.Size);
        }

        public override bool Return(T item)
        {
            var result = base.Return(item);
            try
            {
                m_semaphore.Release();
            }
            catch (SemaphoreFullException sfex)
            {
                if (g_traceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(g_traceInfo, sfex.Message);
                }

                result = false;
            }

            return result;
        }

        public override T Take(object state)
        {
            if (!m_semaphore.WaitOne(WaitTimeout))
            {
                if (g_traceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(g_traceInfo, "Take has timed out");
                }

                return default(T);
            }

            var result = base.Take(state);
            if (result == default(T))
            {
                m_semaphore.Release();
            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {                
                m_semaphore.Close();
            }
        }

        private int WaitTimeout { get; set; }
    }
}
