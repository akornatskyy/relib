using System;
using System.Web;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Tracing;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web.Integration
{
    public sealed class RedirectExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        public string RedirectUrl { get; set; }

        public bool CheckInner { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (string.IsNullOrEmpty(RedirectUrl) || ex == null)
            {
                return false;
            }

            if (typeof(TException).IsAssignableFrom(ex.GetType()) || 
                (CheckInner && ExceptionHelper.Find<TException>(ex.InnerException) != null))
            {
                var context = HttpContext.Current;
                context.Server.ClearError();
                AjaxRedirectHelper.AjaxRedirect(context, BuildAspxErrorPath(context.Request.Url));
                return true;    
            }
            
            return false;
        }

        #endregion

        public string BuildAspxErrorPath(Uri errorUri)
        {
            var builder = new UriBuilder(errorUri);
            builder.Query = String.Concat("aspxerrorpath=", builder.Path);
            builder.Path = RedirectUrl;
            return builder.Uri.AbsoluteUri;
        }
    }
}
