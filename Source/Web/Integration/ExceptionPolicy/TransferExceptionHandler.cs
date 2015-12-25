using System;
using System.Web;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Web.Integration
{
    public sealed class TransferExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        public string TransferUrl { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (ex == null
                || String.IsNullOrEmpty(TransferUrl)
                || !typeof(TException).IsAssignableFrom(ex.GetType()))
            {
                return false;
            }

            var context = HttpContext.Current;
            context.Server.Transfer(BuildAspxErrorPath(context.Request.Url));
            context.Server.ClearError();
            return true;
        }

        #endregion

        public string BuildAspxErrorPath(Uri errorUri)
        {
            var builder = new UriBuilder(errorUri);
            builder.Query = String.Concat("aspxerrorpath=", builder.Path);
            builder.Path = TransferUrl;
            return builder.Uri.PathAndQuery;
        }
    }
}
