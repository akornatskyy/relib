using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Abstractions.Services;
using ReusableLibrary.Abstractions.Tracing;
using ReusableLibrary.Abstractions.WorkItem;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.HistoryLog.Repository;

namespace ReusableLibrary.HistoryLog.WorkItem
{
    public sealed class HistoryLogWorkItem : IWorkItem
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("HistoryLogWorkItem"));

        private readonly string m_unitOfWorkName;
        private readonly IHistoryLogRepository m_historyLogRepository;
        private readonly HistoryLogQueue m_historyLogQueue;

        public HistoryLogWorkItem(string unitOfWorkName,
            IHistoryLogRepository historyLogRepository,
            HistoryLogQueue historyLogQueue)
        {
            m_unitOfWorkName = unitOfWorkName;
            m_historyLogRepository = historyLogRepository;
            m_historyLogQueue = historyLogQueue;
        }

        public IExceptionHandler ExceptionHandler { get; set; }

        #region IWorkItem Members

        public bool DoWork()
        {
            return DoWork(() => false);
        }

        public bool DoWork(Func2<bool> shutdownCallback)
        {
            try
            {
                Flush();
            }
            catch (RepositoryException rex)
            {
                return HandleException(rex);
            }

            return !shutdownCallback();
        }

        #endregion

        private bool HandleException(Exception ex)
        {
            return ExceptionHandler != null && ExceptionHandler.HandleException(ex);
        }

        private bool Flush()
        {
            if (m_historyLogQueue.IsEmpty())
            {
                if (g_traceInfo.IsVerboseEnabled)
                {
                    TraceHelper.TraceVerbose(g_traceInfo, "Queue is empty");
                }

                return false;
            }

            using (var unitOfWork = UnitOfWork.Begin(m_unitOfWorkName))
            {
                m_historyLogQueue.Process(item => m_historyLogRepository.Add(item));

                unitOfWork.Commit();
            }

            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "Queue flushed");
            }

            return true;
        }
    }
}
