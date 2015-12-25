using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Net
{
    public static class SocketHelper
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("Socket"));

        public static bool Connect(Socket socket, IPAddress[] addresses, int port, int timeout)
        {
            using (var waitAsyncResult = new WaitAsyncResult((w, r) =>
            {
                var s = (Socket)r.AsyncState;
                var succeed = false;
                try
                {
                    s.EndConnect(r);
                    succeed = !w.TimedOut;
                }
                catch (SystemException sex)
                {
                    if (g_traceInfo.IsWarningEnabled)
                    {
                        TraceHelper.TraceWarning(g_traceInfo, sex.Message);
                    }
                }

                if (!succeed)
                {
                    s.Close();
                }

                return succeed;
            }))
            {
                socket.BeginConnect(addresses, port, waitAsyncResult.Callback, socket);
                return waitAsyncResult.Wait(timeout);
            }
        }
    }
}
