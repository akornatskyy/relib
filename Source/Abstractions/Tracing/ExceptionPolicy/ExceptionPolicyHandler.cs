using System;
using System.Reflection;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class ExceptionPolicyHandler : IExceptionHandler
    {
        public IExceptionHandler[] Chain { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (ex == null || Chain == null)
            {
                return false;
            }
            
            if (ex is TargetInvocationException)
            {
                return HandleException(ex.InnerException);
            }

            foreach (var handler in Chain)
            {
                if (handler.HandleException(ex))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
