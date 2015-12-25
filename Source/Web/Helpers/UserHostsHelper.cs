using System;
using System.Linq;
using System.Collections.Specialized;

namespace ReusableLibrary.Web.Helpers
{
    public static class UserHostsHelper
    {
        public static string[] UserHosts(NameValueCollection variables)
        {
            return (variables["HTTP_X_FORWARDED_FOR"]
                ?? variables["HTTP_X_CLUSTER_CLIENT_IP"]
                ?? variables["REMOTE_ADDR"] ?? string.Empty)
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToArray();
        }
    }
}
