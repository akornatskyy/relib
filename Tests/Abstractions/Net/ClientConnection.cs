using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Abstractions.Tracing;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class ClientConnection : IClientConnection
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("ClientConnection"));
        private static int g_id;

        public ClientConnection()
        {
            var id = Interlocked.Increment(ref g_id);
            Name = string.Format(CultureInfo.InvariantCulture, "#{0:00}", id);
            IdleState = new IdleState();

            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "{0} - Created", Name);
            }
        }

        public static ClientConnection CreateFactory(object state)
        {
            return new ClientConnection();
        }

        public static void ReleaseFactory(ClientConnection connection)
        {
            connection.Close();
        }

        public string Name { get; private set; }

        #region IClientConnection Members

        public ConnectionOptions Options { get; set; }

        public bool TryConnect()
        {
            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Connecting", Name);
            }

            return true;
        }

        public void Close()
        {
            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "{0} - Closed", Name);
            }
        }

        public IBinaryReader Reader
        {
            get { return null; }
        }

        public IBinaryWriter Writer
        {
            get { return null; }
        }

        #endregion

        #region IIdleStateProvider Members

        public IdleState IdleState { get; set; }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
