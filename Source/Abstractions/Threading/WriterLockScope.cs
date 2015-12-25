using System;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Threading
{
    public struct WriterLockScope : ILockScope
    {
        private readonly ReaderWriterLock m_lock;
        private readonly bool m_aquired;

        public WriterLockScope(ReaderWriterLock @lock, TimeSpan timeout)
            : this(@lock, TimeSpanHelper.Timeout(timeout))
        {
        }

        public WriterLockScope(ReaderWriterLock @lock, int timeout)
        {
            try
            {
                @lock.AcquireWriterLock(timeout);
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
                m_lock.ReleaseWriterLock();
            }
        }

        #endregion
    }
}
