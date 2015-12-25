using System;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class NullExceptionHandler : IExceptionHandler
    {
        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            return true;
        }

        #endregion
    }
}
