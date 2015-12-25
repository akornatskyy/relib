using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Tracing;
using ReusableLibrary.Abstractions.WorkItem;

namespace ReusableLibrary.HistoryLog.WorkItem
{
    public sealed class HistoryLogShutdownTask : IShutdownTask
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("HistoryLogShutdownTask"));

        private readonly string m_workItemName;

        public HistoryLogShutdownTask(string workItemName)
        {
            m_workItemName = workItemName;
        }

        #region IShutdownTask Members

        public void Execute()
        {
            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "Shutdown '{0}'", m_workItemName);
            }

            using (var context = WorkItemContext.Current)
            {
                IWorkItem workItem;

                try
                {
                    workItem = DependencyResolver.Resolve<IWorkItem>(m_workItemName);
                    workItem.DoWork();
                }
                catch (NullReferenceException)
                {
                    /* Container has been reloaded */
                }                
            }
        }

        #endregion
    }
}
