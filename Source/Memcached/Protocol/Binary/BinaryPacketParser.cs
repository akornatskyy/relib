using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class BinaryPacketParser : IPacketParser
    {
        private class Offset
        {
            internal const int OperationCode = 1;
            internal const int KeyLength = 2;
            internal const int ExtrasLength = 4;
            internal const int DataType = 5;
            internal const int Status = 7;
            internal const int TotalBodyLength = 8;
            internal const int Version = 16;
            internal const int EndOfHeader = 24;
            internal const int Flags = 24;
        }

        private readonly IBinaryReader m_reader;
        private readonly Buffer<byte> m_buffer;

        public BinaryPacketParser(IBinaryReader reader, Buffer<byte> buffer)
        {
            buffer.EnsureCapacity(260);
            m_reader = reader;
            m_buffer = buffer;
        }

        #region IPacketParser Members

        public ResponseStatus ReadStatus()
        {
            var buffer = m_buffer.Array;
            BinaryReaderHelper.ReadTo(m_reader, buffer, 0, Offset.EndOfHeader);
            
            var totalLength = BigEndianConverter.GetInt32(buffer, Offset.TotalBodyLength);

            m_buffer.EnsureCapacity(Offset.EndOfHeader + totalLength);
            BinaryReaderHelper.ReadTo(m_reader, buffer, Offset.EndOfHeader, totalLength);
            
            var status = (ResponseStatus)m_buffer.Array[Offset.Status];
            if (status == ResponseStatus.NoError)
            {
                if (buffer[Offset.OperationCode] == 0x00)
                {
                    status = ResponseStatus.Value;
                }
            }

            return status;
        }

        public byte[] ReadKey()
        {
            var buffer = m_buffer.Array;
            var length = BigEndianConverter.GetInt16(buffer, Offset.KeyLength);            
            
            var key = new byte[length];
            Buffer.BlockCopy(buffer, Offset.EndOfHeader, key, 0, length);
            
            return key;
        }

        public int ReadFlags()
        {
            var buffer = m_buffer.Array;
            var length = buffer[Offset.ExtrasLength];
            Debug.Assert(length == 4, "Flags.length == 4");

            return BigEndianConverter.GetInt32(buffer, Offset.Flags);
        }

        public int ReadLength()
        {
            var buffer = m_buffer.Array;
            var extrasLength = buffer[Offset.ExtrasLength];
            Debug.Assert(extrasLength == 4, "Flags.length == 4");
            return BigEndianConverter.GetInt32(buffer, Offset.TotalBodyLength) - extrasLength;
        }

        public long ReadVersion()
        {
            var buffer = m_buffer.Array;
            return BigEndianConverter.GetInt64(buffer, Offset.Version);
        }

        public long ReadIncrement()
        {
            var buffer = m_buffer.Array;
            return BigEndianConverter.GetInt64(buffer, Offset.EndOfHeader);
        }

        public ArraySegment<byte> ReadValue(int length)
        {
            var buffer = m_buffer.Array;
            return new ArraySegment<byte>(buffer, Offset.EndOfHeader + buffer[Offset.ExtrasLength], length);
        }

        #endregion
    }
}
