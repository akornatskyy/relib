using System;
using System.Collections.Specialized;

namespace ReusableLibrary.Web.Helpers
{
    public static class SchemeHelper
    {
        private const string HttpsOn = "on";
        private const string ServerPortSecure = "1";

        public static string Scheme(NameValueCollection variables)
        {
            return (ServerPortSecure.Equals(variables["SERVER_PORT_SECURE"], StringComparison.Ordinal) ? Uri.UriSchemeHttps : null)
                ?? variables["HTTP_X_FORWARDED_PROTO"]
                ?? variables["X_FORWARDED_PROTO"]
                ?? (HttpsOn.Equals(variables["HTTPS"], StringComparison.Ordinal) ? Uri.UriSchemeHttps : null)
                ?? Uri.UriSchemeHttp;
        }
    }
}
