using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class IgnoreDebugExceptionHandler : IExceptionHandler
    {
        public IgnoreDebugExceptionHandler()
        {
            IgnoreDebug = true;
        }

        public bool IgnoreDebug { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            return IgnoreDebug && Debugger.IsAttached;
        }

        #endregion
    }
}
