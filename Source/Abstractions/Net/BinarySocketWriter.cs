using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ReusableLibrary.Abstractions.IO;

namespace ReusableLibrary.Abstractions.Net
{
    public sealed class BinarySocketWriter : IBinaryWriter
    {
        private readonly Socket m_socket;

        public BinarySocketWriter(Socket socket)
        {
            m_socket = socket;
        }

        #region IBinaryWriter Members

        public int Write(byte[] bytes, int offset, int count)
        {
            return m_socket.Send(bytes, offset, count, SocketFlags.None);
        }

        public int Write(IList<ArraySegment<byte>> buffers)
        {
            return m_socket.Send(buffers, SocketFlags.None);
        }

        #endregion
    }
}
