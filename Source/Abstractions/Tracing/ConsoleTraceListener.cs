using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class ConsoleTraceListener : TraceListener
    {
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            TraceEvent(eventCache, source, eventType, id, message, null);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
            {
                ConsoleColor savedColor = Console.ForegroundColor;
                ConsoleColor color = GetConsoleColor(eventType);
                if (savedColor != color)
                {
                    Console.ForegroundColor = GetConsoleColor(eventType);
                }

                WriteHeader(eventCache, source);
                if (args != null)
                {
                    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
                }
                else
                {
                    Console.WriteLine(format);
                }

                if (savedColor != color)
                {
                    Console.ForegroundColor = savedColor;
                }
            }
        }

        public override void Write(string message)
        {
            Console.Out.Write(message);
        }

        public override void WriteLine(string message)
        {
            Console.Out.WriteLine(message);
        }

        [DebuggerStepThrough]
        private static ConsoleColor GetConsoleColor(TraceEventType eventType)
        {
            ConsoleColor color;
            switch (eventType)
            {
                case TraceEventType.Error:
                    color = ConsoleColor.Red;
                    break;
                case TraceEventType.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }

            return color;
        }

        [DebuggerStepThrough]
        private bool IsEnabled(TraceOptions opts)
        {
            return (opts & this.TraceOutputOptions) != TraceOptions.None;
        }

        private void WriteHeader(TraceEventCache eventCache, string source)
        {
            //// {timestamp} [{managedThreadId}] {source} - {message}
            if (IsEnabled(TraceOptions.DateTime))
            {
                Write(eventCache.DateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss,fff ", CultureInfo.InvariantCulture));
            }

            if (IsEnabled(TraceOptions.ThreadId))
            {
                Write("[");
                string threadName = Thread.CurrentThread.Name;
                if (!String.IsNullOrEmpty(threadName))
                {
                    Write(threadName + ":");
                }

                Write(eventCache.ThreadId + "] ");
            }

            Write(source + " - ");
        }
    }
}
