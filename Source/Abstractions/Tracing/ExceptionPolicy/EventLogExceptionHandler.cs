using System;
using System.Diagnostics;
using System.Security;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Tracing
{
    public class EventLogExceptionHandler : Disposable, IExceptionHandler
    {
        private readonly EventLog m_eventLog;
        private readonly int m_eventId;

        public EventLogExceptionHandler()
            : this(null, null, 5000)
        {
        }

        public EventLogExceptionHandler(string sourceName, int eventId)
            : this(sourceName, null, eventId)
        {
        }

        public EventLogExceptionHandler(string sourceName, string logName, int eventId)
        {
            logName = logName ?? "Application";
            sourceName = sourceName ?? ".NET Runtime 2.0 Error Reporting";
            m_eventId = eventId;

            try
            {
                if (EventLog.Exists(logName)
                    && EventLog.SourceExists(sourceName))
                {
                    new EventLogPermission(EventLogPermissionAccess.Write, ".").Demand();
                    m_eventLog = new EventLog(logName, ".", sourceName);
                }
            }
            catch (SecurityException sex)
            {
                if (TraceInfo.ErrorTraceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(TraceInfo.ErrorTraceInfo, sex.Message);
                }
            }
        }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (m_eventLog != null)
            {
                var buffer = new StringBuilder(0x1000);
                buffer
                    .AppendLine("Error Report")
                    .AppendLine();
                ErrorFormatter.Append(buffer, ex);
                m_eventLog.WriteEntry(buffer.ToString(), EventLogEntryType.Error, m_eventId);
            }

            return false;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_eventLog != null)
                {
                    m_eventLog.Dispose();
                }
            }
        }
    }
}
