using System;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Threading
{
    public struct UpgradeLockScope : ILockScope
    {
        private readonly ReaderWriterLock m_lock;
        private readonly bool m_aquired;
        private LockCookie m_cookie;

        public UpgradeLockScope(ReaderWriterLock @lock, TimeSpan timeout)
            : this(@lock, TimeSpanHelper.Timeout(timeout))
        {
        }

        public UpgradeLockScope(ReaderWriterLock @lock, int timeout)
        {
            try
            {
                m_cookie = @lock.UpgradeToWriterLock(timeout);
                m_lock = @lock;
                m_aquired = true;
            }
            catch (ApplicationException)
            {
                m_cookie = new LockCookie();
                m_lock = null;
                m_aquired = false;
            }
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
                m_lock.DowngradeFromWriterLock(ref m_cookie);
            }
        }

        #endregion
    }
}
