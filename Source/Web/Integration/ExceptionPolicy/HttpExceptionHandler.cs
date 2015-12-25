using System;
using System.Linq;
using System.Web;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Web.Integration
{
    public sealed class HttpExceptionHandler : IExceptionHandler
    {
        public int[] Ignore { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var hex = ex as HttpException;
            if (hex == null || Ignore == null)
            {
                return false;
            }

            return Ignore.Contains(hex.GetHttpCode());
        }

        #endregion
    }
}
