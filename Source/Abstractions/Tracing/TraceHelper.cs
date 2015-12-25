using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using System.Collections;

namespace ReusableLibrary.Abstractions.Tracing
{
    public static class TraceHelper
    {
        public const int DefaultEventId = 5000;

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceVerbose(TraceInfo info, string message)
        {
            info.TraceSource.TraceEvent(TraceEventType.Verbose, DefaultEventId, message);
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceVerbose(TraceInfo info, string format, params object[] args)
        {
            TraceVerbose(info, String.Format(CultureInfo.CurrentCulture, format, args));
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceInfo(TraceInfo info, string message)
        {
            info.TraceSource.TraceEvent(TraceEventType.Information, DefaultEventId, message);
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceInfo(TraceInfo info, string format, params object[] args)
        {
            TraceInfo(info, String.Format(CultureInfo.CurrentCulture, format, args));
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceWarning(TraceInfo info, string message)
        {
            info.TraceSource.TraceEvent(TraceEventType.Warning, DefaultEventId, message);
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceWarning(TraceInfo info, string format, params object[] args)
        {
            TraceWarning(info, String.Format(CultureInfo.CurrentCulture, format, args));
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceError(TraceInfo info, string message)
        {
            info.TraceSource.TraceEvent(TraceEventType.Error, DefaultEventId, message);
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceError(TraceInfo info, string format, params object[] args)
        {
            TraceError(info, String.Format(CultureInfo.CurrentCulture, format, args));
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceError(TraceInfo info, Exception ex, string format, params object[] args)
        {
            info.TraceSource.TraceEvent(TraceEventType.Error, DefaultEventId, format, args);
            foreach (DictionaryEntry entry in ex.Data)
            {
                info.TraceSource.TraceData(TraceEventType.Error, DefaultEventId, String.Format(CultureInfo.CurrentCulture, "{0}: {1}", entry.Key, entry.Value));
            }

            info.TraceSource.TraceData(TraceEventType.Error, DefaultEventId, ex);
        }

        [Conditional("TRACE")]
        [DebuggerStepThrough]
        public static void TraceError(TraceInfo info, Exception ex)
        {
            TraceError(info, ex, ex.Message);
        }
    }
}
