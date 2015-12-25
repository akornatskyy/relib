using System;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Threading
{
    public struct ReaderLockScope : ILockScope
    {
        private readonly ReaderWriterLock m_lock;
        private readonly bool m_aquired;

        public ReaderLockScope(ReaderWriterLock @lock, TimeSpan timeout)
            : this(@lock, TimeSpanHelper.Timeout(timeout))
        {
        }

        public ReaderLockScope(ReaderWriterLock @lock, int timeout)
        {
            try
            {
                @lock.AcquireReaderLock(timeout);
                m_lock = @lock;
                m_aquired = true;
            }
            catch (ApplicationException)
            {
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
                m_lock.ReleaseReaderLock();
            }
        }

        #endregion
    }
}
