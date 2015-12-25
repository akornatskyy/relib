using System;
using System.Diagnostics;
using System.Net;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Net
{
    public static class DnsHelper
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("Dns"));

        public static IPAddress[] GetHostAddresses(string hostNameOrAddress, int timeout)
        {
            IPAddress[] addresses = null;
            using (var waitAsyncResult = new WaitAsyncResult((w, r) =>
            {
                try
                {
                    addresses = Dns.EndGetHostAddresses(r);
                    return true;
                }
                catch (SystemException sex)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, sex.Message);
                    }

                    return false;
                }
            }))
            {
                Dns.BeginGetHostAddresses(hostNameOrAddress, waitAsyncResult.Callback, null);
                waitAsyncResult.Wait(timeout);
                return addresses;
            }
        }
    }
}
