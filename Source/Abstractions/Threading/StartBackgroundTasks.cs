using System.Diagnostics;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Threading
{
    public sealed class StartBackgroundTasks : IStartupTask
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceBootstrapper));

        private readonly IBackgroundTask[] m_tasks;

        public StartBackgroundTasks(IBackgroundTask[] tasks)
        {
            m_tasks = tasks;
        }

        public void Execute()
        {
            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "Starting background tasks...");
            }

            foreach (var t in m_tasks)
            {
                t.Start();
            }

            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "All background tasks has been started");
            }
        }
    }
}
