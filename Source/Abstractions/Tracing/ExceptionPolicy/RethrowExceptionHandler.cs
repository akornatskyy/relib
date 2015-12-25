using System;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class RethrowExceptionHandler<T> : IExceptionHandler
        where T : Exception
    {
        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (ex is T)
            {
                throw ex;
            }

            return false;
        }

        #endregion
    }
}
