using System;
using System.Threading;

namespace ReusableLibrary.Abstractions.Threading
{
    public struct ReaderWriterLockScope
    {
        private readonly ReaderWriterLock m_lock;

        public ReaderWriterLockScope(ReaderWriterLock @lock)
        {
            m_lock = @lock;
        }

        public ILockScope Reader(int timeout)
        {
            return new ReaderLockScope(m_lock, timeout);
        }

        public ILockScope Reader(TimeSpan timeout)
        {
            return new ReaderLockScope(m_lock, timeout);
        }

        public ILockScope UpgradeToWriter(int timeout)
        {
            return new UpgradeLockScope(m_lock, timeout);
        }

        public ILockScope UpgradeToWriter(TimeSpan timeout)
        {
            return new UpgradeLockScope(m_lock, timeout);
        }

        public ILockScope Writer(int timeout)
        {
            return new WriterLockScope(m_lock, timeout);
        }

        public ILockScope Writer(TimeSpan timeout)
        {
            return new WriterLockScope(m_lock, timeout);
        }
    }
}
