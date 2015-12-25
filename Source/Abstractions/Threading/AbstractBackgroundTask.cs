using System;
using System.Diagnostics;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Threading
{
    public abstract class AbstractBackgroundTask : IBackgroundTask
    {
        private readonly Thread m_workerThread;
        private volatile bool m_isRunning;

        protected AbstractBackgroundTask()
            : this(null)
        {
        }

        protected AbstractBackgroundTask(string taskName)
        {
            var name = taskName ?? GetType().Name;
            m_workerThread = new Thread(new ThreadStart(WorkerThreadWrapper));
            m_workerThread.Name = name + "Thread";
            m_workerThread.IsBackground = true;
            WaitHandle = new ManualResetEvent(false);
            TraceInfo = new TraceInfo(new TraceSource(name));
        }

        public TimeSpan WorkItemTimeBreak { get; set; }

        public TimeSpan WorkItemStartDelay { get; set; }

        #region IBackgroundTask Members

        public EventWaitHandle WaitHandle { get; private set; }

        public bool IsRunning
        {
            get { return m_isRunning; }
            private set { m_isRunning = value; }
        }

        public void Start()
        {
            if (m_isRunning)
            {
                throw new InvalidOperationException(Properties.Resources.AbstractBackgroundTaskAlreadyRunning);
            }

            if (TraceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(TraceInfo, "Starting...");
            }

            try
            {
                m_workerThread.Start();
            }
            catch (ThreadStateException tsex)
            {
                throw new InvalidOperationException(tsex.Message, tsex);
            }

            while (!IsRunning)
            {
                Thread.Sleep(0);
            }

            WaitHandle.Set();
        }

        public bool Wait(TimeSpan timeout)
        {
            if (!m_isRunning)
            {
                return true;
            }

            if (!WaitHandle.Set())
            {
                throw new InvalidOperationException();
            }

            return m_workerThread.Join(timeout);
        }

        public void Stop(bool forceShutdown)
        {
            if (!m_isRunning)
            {
                return;
            }

            if (TraceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(TraceInfo, "Stopping...");
            }

            if (!WaitHandle.Set())
            {
                throw new InvalidOperationException();
            }

            if (forceShutdown)
            {
                if (TraceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(TraceInfo, "Forcing shutdown...");
                }

                try
                {
                    m_workerThread.Abort();
                }
                catch (ThreadStateException)
                {
                    // Just ignore it
                }
                finally
                {
                    m_isRunning = false;
                }
            }
        }

        #endregion

        public abstract bool DoWork(Func2<bool> shutdownCallback);

        protected TraceInfo TraceInfo { get; private set; }

        private void WorkerThreadWrapper()
        {
            IsRunning = true;

            try
            {
                WaitHandle.WaitOne();
                WaitHandle.Reset();
                if (TraceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(TraceInfo, "Working...");
                }

                if (WorkItemStartDelay != TimeSpan.Zero && WaitHandle.WaitOne(WorkItemStartDelay))
                {
                    return;
                }

                int tasksServed = 0;
                while (true)
                {
                    if (!DoWork(() => WaitHandle.WaitOne(0)))
                    {
                        if (TraceInfo.IsInfoEnabled)
                        {
                            TraceHelper.TraceInfo(TraceInfo, "No more work");
                        }

                        break;
                    }

                    tasksServed++;

                    if (WaitHandle.WaitOne(0))
                    {
                        break;
                    }

                    if (TraceInfo.IsInfoEnabled)
                    {
                        TraceHelper.TraceInfo(TraceInfo, "Sleeping for {0}", TimeSpanHelper.ToLongTimeString(WorkItemTimeBreak).ToLowerInvariant());
                    }

                    if (WaitHandle.WaitOne(WorkItemTimeBreak))
                    {
                        break;
                    }
                }

                if (TraceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(TraceInfo, "Served {0} tasks", tasksServed);
                }
            }
            catch (ThreadAbortException)
            {
                // Just ignore it
            }
            finally
            {
                WaitHandle.Set();
                IsRunning = false;
                if (TraceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(TraceInfo, "Finished");
                }
            }
        }
    }
}
