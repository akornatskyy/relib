using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class TraceInfo
    {
        public static readonly TraceInfo ErrorTraceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceError));

        public TraceInfo(TraceSource traceSource)
        {
            TraceSource = traceSource;
            IsVerboseEnabled = TraceSource.Switch.ShouldTrace(TraceEventType.Verbose);
            IsInfoEnabled = TraceSource.Switch.ShouldTrace(TraceEventType.Information);
            IsWarningEnabled = TraceSource.Switch.ShouldTrace(TraceEventType.Warning);
            IsErrorEnabled = TraceSource.Switch.ShouldTrace(TraceEventType.Error);
        }

        public TraceSource TraceSource
        {
            [DebuggerStepThrough]
            get;
            private set;
        }

        public bool IsVerboseEnabled { get; private set; }

        public bool IsInfoEnabled { get; private set; }

        public bool IsWarningEnabled { get; private set; }

        public bool IsErrorEnabled { get; private set; }
    }
}
