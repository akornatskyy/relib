using System;
using System.Threading;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Threading
{
    public class WaitAsyncResult : Disposable
    {
        private readonly ManualResetEvent m_waitHandle = new ManualResetEvent(false);
        private readonly Func2<WaitAsyncResult, IAsyncResult, bool> m_callback;

        private int m_status;

        public WaitAsyncResult(Func2<WaitAsyncResult, IAsyncResult, bool> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            m_callback = callback;
        }

        public bool Succeed { get; private set; }

        public bool TimedOut
        {
            get
            {
                return 1 == Interlocked.CompareExchange(ref m_status, 1, 1);
            }
        }

        public void Callback(IAsyncResult asyncResult)
        {
            try
            {
                Succeed = m_callback(this, asyncResult);
            }
            finally
            {
                try
                {
                    if (0 == Interlocked.CompareExchange(ref m_status, 2, 0))
                    {
                        m_waitHandle.Set();
                        Interlocked.Exchange(ref m_status, 3);
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        public bool Wait(int timeout)
        {
            if (!m_waitHandle.WaitOne(timeout)
                && 0 == Interlocked.CompareExchange(ref m_status, 1, 0))
            {
                return false;
            }

            // m_waitHandle.Close() must not happen before 
            // m_waitHandle.Set() above completes (else m_waitHandle.Set()
            // might fail)
            while (3 != Interlocked.CompareExchange(ref m_status, 3, 3))
            {
                Thread.SpinWait(1);
            }

            return Succeed;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_waitHandle.Close();
            }
        }
    }
}
