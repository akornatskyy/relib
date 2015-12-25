using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Net
{
    public class TcpClientConnection : Disposable, IClientConnection, IIdleStateProvider
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource("TcpClientConnection"));

        private Socket m_socket;

        public TcpClientConnection()
        {
            IdleState = new IdleState();
        }

        public static TcpClientConnection CreateFactory(object state)
        {
            var options = (ConnectionOptions)state;
            var connection = new TcpClientConnection();
            connection.Options = options;
            if (!connection.TryConnect())
            {
                connection = null;
            }

            return connection;
        }

        public static void ReleaseFactory(TcpClientConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (SystemException sex)
            {
                if (g_traceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(g_traceInfo, sex.Message);
                }
            }
        }

        #region IClientConnection Members

        public ConnectionOptions Options { get; set; }

        public bool TryConnect()
        {
            var socket = m_socket;
            if (socket != null)
            {
                if (socket.Connected)
                {
                    return true;
                }
            }
            else
            {
                socket = CreateSocket(Options);
            }

            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Connecting...", Options.FullName);
            }

            var addresses = DnsHelper.GetHostAddresses(Options.Server, Options.DnsTimeout);
            if (addresses == null)
            {
                if (g_traceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(g_traceInfo, "{0} - Unable resolve host name...", Options.FullName);
                }

                return false;
            }

            if (!SocketHelper.Connect(socket, addresses, Options.Port, Options.ConnectTimeout))
            {
                if (g_traceInfo.IsWarningEnabled)
                {
                    TraceHelper.TraceWarning(g_traceInfo, "{0} - Can not connect within {1}", Options.FullName, TimeSpanHelper.ToShortTimeString(TimeSpan.FromMilliseconds(Options.ConnectTimeout)));
                }

                return false;
            }

            Reader = new BinarySocketReader(socket);
            Writer = new BinarySocketWriter(socket);
            m_socket = socket;

            if (g_traceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(g_traceInfo, "{0} - Connected", Options.FullName);
            }

            return true;
        }

        public IBinaryReader Reader { get; private set; }

        public IBinaryWriter Writer { get; private set; }

        public void Close()
        {
            if (m_socket == null)
            {
                return;
            }

            Reader = null;
            Writer = null;
            try
            {
                if (m_socket.Connected)
                {
                    if (g_traceInfo.IsVerboseEnabled)
                    {
                        TraceHelper.TraceVerbose(g_traceInfo, "{0} - Shutting down...", Options.FullName);
                    }

                    m_socket.Shutdown(SocketShutdown.Both);
                }
            }
            finally
            {
                m_socket.Close();
                m_socket = null;

                if (g_traceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(g_traceInfo, "{0} - Closed", Options.FullName);
                }
            }
        }

        #endregion

        #region IIdleStateProvider Members

        public IdleState IdleState { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "TcpClientConnection [{0}]", Options.FullName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        private static Socket CreateSocket(ConnectionOptions options)
        {
            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Creating...", options.FullName);
            }

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Disable the Nagle Algorithm for this socket.
            socket.NoDelay = true;

            socket.SendTimeout = options.SendTimeout;
            socket.SendBufferSize = options.SendBufferSize;

            socket.ReceiveTimeout = options.ReceiveTimeout;
            socket.ReceiveBufferSize = options.ReceiveBufferSize;

            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            return socket;
        }
    }
}
