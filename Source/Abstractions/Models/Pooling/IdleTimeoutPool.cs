using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    public class IdleTimeoutPool<T> : DecoratedPool<T>
        where T : class, IIdleStateProvider
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("IdleTimeoutPool"));

        private readonly object m_syncRoot;
        private readonly Action<T> m_releasefactory;
        private readonly long m_idleTimeout;
        private readonly long m_leaseTimeout;
        private readonly int m_idleLockTimeout;
        private readonly Timer m_idleTimer;

        public IdleTimeoutPool(object syncRoot, IPool<T> inner, Action<T> releasefactory, int idleTimeout, int leaseTimeout)
            : base(inner)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            if (releasefactory == null)
            {
                throw new ArgumentNullException("releasefactory");
            }

            m_syncRoot = syncRoot;
            m_releasefactory = releasefactory;
            m_idleTimeout = TimeSpan.FromMilliseconds(idleTimeout).Ticks;
            m_leaseTimeout = TimeSpan.FromMilliseconds(leaseTimeout).Ticks;
            m_idleLockTimeout = idleTimeout;
            m_idleTimer = new Timer(idleTimeout);
            m_idleTimer.Elapsed += new ElapsedEventHandler(OnIdleElapsed);
            m_idleTimer.AutoReset = false;
            m_idleTimer.Start();
        }

        #region DecoratedPool members

        public override T Take(object state)
        {
            return TakeCurrent(state, DateTime.UtcNow);
        }

        public override bool Return(T item)
        {
            if (CheckExpired(item.IdleState, DateTime.UtcNow))
            {
                m_releasefactory(item);
                if (g_traceInfo.IsVerboseEnabled)
                {
                    TraceHelper.TraceVerbose(g_traceInfo, "{0} - Idled out on Return", Inner.Name);
                }

                return true;
            }

            item.IdleState.UsedOn = DateTime.UtcNow;
            return Inner.Return(item);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                m_idleTimer.Stop();
                m_idleTimer.Dispose();
            }
        }

        #endregion

        protected T TakeCurrent(object state, DateTime current)
        {
            T item;
            while ((item = Inner.Take(state)) != null)
            {
                if (!CheckExpired(item.IdleState, current))
                {
                    break;
                }

                m_releasefactory(item);
                if (g_traceInfo.IsVerboseEnabled)
                {
                    TraceHelper.TraceVerbose(g_traceInfo, "{0} - Idled out on TakeCurrent", Inner.Name);
                }
            }

            return item;
        }

        protected int RecycleIdled(DateTime current)
        {
            var count = Inner.Count;
            if (count == 0)
            {
                return 0;
            }

            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Recycling Idled, Count = {1}", Inner.Name, count);
            }

            // grab all items in the pool and check if they are expired            
            var index = 0;
            var items = new T[count];
            T item;
            while ((item = Inner.Take(null)) != null)
            {
                if (CheckExpired(item.IdleState, current))
                {
                    m_releasefactory(item);
                    continue;
                }

                items[index++] = item;
            }

            count -= index;

            // return back only items that are not expired
            while (index > 0)
            {
                Inner.Return(items[--index]);
            }

            return count;
        }

        protected bool CheckExpired(IdleState state, DateTime current)
        {
            return (current >= state.UsedOn.AddTicks(m_idleTimeout))
                    || (current >= state.CreatedOn.AddTicks(m_leaseTimeout));
        }

        private void OnIdleElapsed(object sender, ElapsedEventArgs e)
        {
            var current = DateTime.UtcNow;
            int idle = 0;
            using (var @lock = new MonitorLockScope(m_syncRoot, m_idleLockTimeout))
            {
                if (@lock.Aquired)
                {
                    idle = RecycleIdled(current);
                }
                else
                {
                    if (g_traceInfo.IsInfoEnabled)
                    {
                        TraceHelper.TraceInfo(g_traceInfo, "{0} - Failed to obtain lock on idle elapsed", Inner.Name);
                    }
                }
            }

            if (idle > 0 && g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "{0} - Recycled {1}", Inner.Name, idle);
            }

            m_idleTimer.Start();
        }
    }
}
