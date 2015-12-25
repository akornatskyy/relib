using System;
using System.Web;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public static class HttpRequestExtensions
    {
        public static string[] UserHosts(this HttpRequest request)
        {
            return UserHostsHelper.UserHosts(request.ServerVariables);
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return AjaxRedirectHelper.IsAjaxRequest(request);
        }

        public static string Scheme(this HttpRequest request)
        {
            return SchemeHelper.Scheme(request.ServerVariables);
        }

        public static bool IsSchemeHttps(this HttpRequest request)
        {
            return Uri.UriSchemeHttps.Equals(Scheme(request), StringComparison.Ordinal);
        }

        public static string Host(this HttpRequest request)
        {
            var host = request.Headers["HOST"];
            if (!string.IsNullOrEmpty(host))
            {
                return host.Split(':')[0];
            }

            return request.Url.Host;
        }

        public static bool IsHttpVerbGetOrHead(this HttpContext context)
        {
            var method = context.Request.HttpMethod;
            return ((method.Length == 3 && method == "GET")
                || (method.Length == 4 && method == "HEAD"));
        }

        public static bool TryUniqueToken(this HttpRequest request, out string uniqueToken)
        {
            return UniqueTokenHelper.TryUniqueToken(request.Form, out uniqueToken);
        }

        public static string UniqueToken(this HttpRequest request)
        {
            return UniqueTokenHelper.UniqueToken(request.Form);
        }
    }
}
