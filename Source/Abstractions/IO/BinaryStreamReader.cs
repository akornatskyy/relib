using System.IO;

namespace ReusableLibrary.Abstractions.IO
{
    public sealed class BinaryStreamReader : IBinaryReader
    {
        private readonly Stream m_stream;

        public BinaryStreamReader(Stream stream)
        {
            m_stream = stream;
        }

        #region IBinaryReader Members

        public int Read(byte[] buffer, int offset, int count)
        {
            return m_stream.Read(buffer, offset, count);
        }

        #endregion
    }
}
