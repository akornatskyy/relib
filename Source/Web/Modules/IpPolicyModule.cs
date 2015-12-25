using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Web.Integration;

namespace ReusableLibrary.Web
{
    public sealed class IpPolicyModule : IHttpModule
    {
        private static IpNetwork[] g_allowedNetworks = null;
        private static IpNetwork[] g_deniedNetworks = null;

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PostMapRequestHandler += new EventHandler(OnEnter);
        }

        #endregion

        public static bool SecureOnly { get; set; }

        public static Regex NoExceptionPathRegex { get; set; }

        public static void Allowed(string[] networks)
        {
            g_allowedNetworks = Parse(networks);
        }

        public static void Denied(string[] networks)
        {
            g_deniedNetworks = Parse(networks);
        }

        private static IpNetwork[] Parse(string[] networks)
        {
            var result = new List<IpNetwork>();
            foreach (var net in networks)
            {
                var parts = net.Split('/');
                if (parts.Length != 2)
                {
                    throw new InvalidOperationException("Invalid network");
                }

                result.Add(new IpNetwork()
                {
                    Network = IpNumberHelper.ToIpNumber(parts[0]),
                    Mask = IpNumberHelper.Netmask(Int32.Parse(parts[1]))
                });
            }

            return result.ToArray();
        }

        private static void OnEnter(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if ((g_allowedNetworks == null && g_deniedNetworks == null)
                || context.Error != null
                || context.CurrentHandler == null
                || context.CurrentHandler is DefaultHttpHandler)
            {
                return;
            }

            var request = context.Request;
            if (SecureOnly && request.Scheme() != Uri.UriSchemeHttps)
            {
                return;
            }

            foreach (var ip in request.UserHosts())
            {
                var ipnum = IpNumberHelper.ToIpNumber(ip);
                if (g_deniedNetworks != null)
                {
                    var deniedNetwork = FindMatch(ipnum, g_deniedNetworks);
                    if (deniedNetwork != null
                        && (NoExceptionPathRegex == null
                        || !NoExceptionPathRegex.IsMatch(request.RawUrl)))
                    {
                        throw new IpPolicyException(string.Format(CultureInfo.InvariantCulture,
                            "Matched '{0}' in denied network {1}/{2}.", ip, 
                            IpNumberHelper.ToIpString(deniedNetwork.Network), 
                            IpNumberHelper.ToIpString(deniedNetwork.Mask)));
                    }
                }

                if (g_allowedNetworks != null)
                {
                    var allowedNetwork = FindMatch(ipnum, g_allowedNetworks);
                    if (allowedNetwork == null
                        && (NoExceptionPathRegex == null
                        || !NoExceptionPathRegex.IsMatch(request.RawUrl)))
                    {
                        throw new IpPolicyException(string.Format(CultureInfo.InvariantCulture,
                            "Missmatch '{0}' in allowed networks.", ip));
                    }
                }
            }
        }

        private static IpNetwork FindMatch(int ipnum, IpNetwork[] networks)
        {
            foreach (var network in networks)
            {
                if (IpNumberHelper.Contains(network.Network, network.Mask, ipnum))
                {
                    return network;
                }
            }

            return null;
        }

        private class IpNetwork
        {
            public int Network { get; set; }

            public int Mask { get; set; }
        }
    }
}
