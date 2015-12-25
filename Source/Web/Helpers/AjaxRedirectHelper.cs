using System;
using System.Web;

namespace ReusableLibrary.Web.Helpers
{
    public static class AjaxRedirectHelper
    {
        public const int AjaxRedirectStatusCode = 207;

        public static void AjaxRedirect(HttpContext context, string url)
        {
            AjaxRedirect(context, url, true);
        }

        public static void AjaxRedirect(HttpContext context, string url, bool endResponse)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.Response;
            if (IsAjaxRequest(context.Request))
            {
                response.StatusCode = AjaxRedirectStatusCode;
                response.AddHeader("Location", url);
                if (endResponse)
                {
                    response.End();
                }
            }
            else
            {
                response.Redirect(url, endResponse);
            }
        }

        public static void AjaxRedirect(HttpContextBase context, string url)
        {
            AjaxRedirect(context, url, true);
        }

        public static void AjaxRedirect(HttpContextBase context, string url, bool endResponse)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.Response;
            if (IsAjaxRequest(context.Request))
            {
                response.StatusCode = AjaxRedirectStatusCode;
                response.AddHeader("Location", url);
                if (endResponse)
                {
                    response.End();
                }
            }
            else
            {
                response.Redirect(url, endResponse);
            }
        }

        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return ((request["X-Requested-With"] == "XMLHttpRequest")
                || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }

        public static bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return ((request["X-Requested-With"] == "XMLHttpRequest")
                || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }
    }
}
