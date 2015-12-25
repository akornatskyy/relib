using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class TraceExceptionHandler : IExceptionHandler
    {
        private readonly TraceInfo m_traceInfo;

        public TraceExceptionHandler()
            : this(TraceInfo.ErrorTraceInfo)
        {
        }

        public TraceExceptionHandler(string traceSourceName)
            : this(new TraceInfo(new TraceSource(traceSourceName ?? TraceInfo.ErrorTraceInfo.TraceSource.Name)))
        {
        }

        public TraceExceptionHandler(TraceInfo traceInfo)
        {
            m_traceInfo = traceInfo;
        }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (m_traceInfo.IsErrorEnabled)
            {
                TraceHelper.TraceError(m_traceInfo, ex);
            }

            return false;
        }

        #endregion
    }
}
