using System;
using System.Collections.Generic;
using System.IO;

namespace ReusableLibrary.Abstractions.IO
{
    public sealed class BinaryStreamWriter : IBinaryWriter
    {
        private readonly Stream m_stream;

        public BinaryStreamWriter(Stream stream)
        {
            m_stream = stream;
        }

        #region IBinaryWriter Members

        public int Write(byte[] bytes, int offset, int count)
        {
            m_stream.Write(bytes, offset, count);
            return count;
        }

        public int Write(IList<ArraySegment<byte>> buffers)
        {
            var count = 0;
            foreach (var segment in buffers)
            {
                m_stream.Write(segment.Array, segment.Offset, segment.Count);
                count += segment.Count;
            }

            return count;
        }

        #endregion
    }
}
