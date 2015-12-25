using System;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Threading
{
    public struct MonitorLockScope : ILockScope
    {
        private readonly object m_sync;
        private readonly bool m_aquired;

        public MonitorLockScope(object sync, TimeSpan timeout)
            : this(sync, TimeSpanHelper.Timeout(timeout))
        {
        }

        public MonitorLockScope(object sync, int timeout)
        {
            m_sync = sync;
            m_aquired = Monitor.TryEnter(sync, timeout);
        }

        #region ILockScope Members

        public bool Aquired
        {
            get { return m_aquired; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_aquired)
            {
                Monitor.Exit(m_sync);
            }
        }

        #endregion
    }
}
