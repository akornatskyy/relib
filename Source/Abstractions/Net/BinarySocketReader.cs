using System.Net.Sockets;
using ReusableLibrary.Abstractions.IO;

namespace ReusableLibrary.Abstractions.Net
{
    public sealed class BinarySocketReader : IBinaryReader
    {
        private readonly Socket m_socket;

        public BinarySocketReader(Socket socket)
        {
            m_socket = socket;
        }

        #region IBinaryReader Members

        public int Read(byte[] buffer, int offset, int count)
        {
            return m_socket.Receive(buffer, offset, count, SocketFlags.None);
        }

        #endregion
    }
}
