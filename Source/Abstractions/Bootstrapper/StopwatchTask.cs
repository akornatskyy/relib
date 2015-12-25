using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Bootstrapper
{
    public sealed class StopwatchTask : IStartupTask, IShutdownTask
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceBootstrapper));
        private static DateTime g_startTime = DateTime.Now;

        #region IStartupTask Members

        void IStartupTask.Execute()
        {
            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "Starting...");
            }

            g_startTime = DateTime.Now;
        }

        #endregion

        #region IShutdownTask Members

        void IShutdownTask.Execute()
        {
            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "Completed within {0}", TimeSpanHelper.ToLongTimeString(DateTime.Now.Subtract(g_startTime)).ToLowerInvariant());
                TraceHelper.TraceInfo(g_traceInfo, "All Done");
            }
        }

        #endregion
    }
}
