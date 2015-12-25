using System;
using System.Web;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public static class HttpRequestBaseExtensions
    {
        public static string[] UserHosts(this HttpRequestBase request)
        {
            return UserHostsHelper.UserHosts(request.ServerVariables);
        }

        public static string Scheme(this HttpRequestBase request)
        {
            return SchemeHelper.Scheme(request.ServerVariables);
        }

        public static bool IsSchemeHttps(this HttpRequestBase request)
        {
            return Uri.UriSchemeHttps.Equals(Scheme(request), StringComparison.Ordinal);
        }

        public static string Host(this HttpRequestBase request)
        {
            var host = request.Headers["HOST"];
            if (!string.IsNullOrEmpty(host))
            {
                return host.Split(':')[0];
            }

            return request.Url.Host;
        }

        public static bool IsHttpVerbGetOrHead(this HttpRequestBase request)
        {
            var method = request.HttpMethod;
            return ((method.Length == 3 && method == "GET")
                || (method.Length == 4 && method == "HEAD"));
        }

        public static bool TryUniqueToken(this HttpRequestBase request, out string uniqueToken)
        {
            return UniqueTokenHelper.TryUniqueToken(request.Form, out uniqueToken);
        }

        public static string UniqueToken(this HttpRequestBase request)
        {
            return UniqueTokenHelper.UniqueToken(request.Form);
        }
    }
}
