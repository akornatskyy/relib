using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Threading
{
    public sealed class WaitBackgroundTasks : IShutdownTask
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceBootstrapper));

        private readonly IBackgroundTask[] m_tasks;
        private readonly TimeSpan m_waitDuration;

        public WaitBackgroundTasks(IBackgroundTask[] tasks, int waitDurationInSeconds)
            : this(tasks, TimeSpan.FromSeconds(waitDurationInSeconds))
        {
        }

        public WaitBackgroundTasks(IBackgroundTask[] tasks, TimeSpan waitDuration)
        {
            m_tasks = tasks;
            m_waitDuration = waitDuration;
        }

        public void Execute()
        {
            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "Waiting background tasks to complete");
            }

            // Signal all running tasks to stop work.
            foreach (var t in m_tasks)
            {
                if (t.IsRunning)
                {
                    t.Stop(false);
                }
            }

            // Still running? Force shutdown.
            foreach (var t in m_tasks)
            {
                if (t.IsRunning && !t.Wait(m_waitDuration))
                {
                    t.Stop(true);
                }
            }

            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "All background tasks has been completed");
            }
        }
    }
}
