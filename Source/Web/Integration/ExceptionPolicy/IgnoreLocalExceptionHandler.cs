using System;
using System.Web;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Web.Integration
{
    public sealed class IgnoreLocalExceptionHandler : IExceptionHandler
    {
        public IgnoreLocalExceptionHandler()
        {
            IgnoreLocal = true;
        }

        public bool IgnoreLocal { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            return IgnoreLocal
                && HttpContext.Current != null 
                && HttpContext.Current.Request.IsLocal;
        }

        #endregion
    }
}
