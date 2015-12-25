using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class IgnoreExceptionHandler<TIgnore> : IExceptionHandler
        where TIgnore : Exception
    {
        public bool CheckInner { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            if (typeof(TIgnore).IsAssignableFrom(ex.GetType()))
            {
                return true;
            }

            return CheckInner && ExceptionHelper.Find<TIgnore>(ex.InnerException) != null;
        }

        #endregion
    }
}
