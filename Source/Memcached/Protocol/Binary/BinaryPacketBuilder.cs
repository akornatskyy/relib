using System;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class BinaryPacketBuilder : IPacketBuilder
    {
        private static class Offset
        {
            internal const int StartOfHeader = 0;
            internal const int OperationCode = 1;
            internal const int KeyLength = 2;
            internal const int ExtrasLength = 4;
            internal const int DataType = 5;
            internal const int TotalBodyLength = 8;
            internal const int Version = 16;
            internal const int EndOfHeader = 24;
            internal const int Flags = 24;
            internal const int Expires = 28;
        }

        private readonly Buffer<byte> m_buffer;
        private int m_offset;
        private int m_position;

        public BinaryPacketBuilder(Buffer<byte> buffer)
        {
            buffer.EnsureCapacity(274);
            m_buffer = buffer;
        }

        #region IPacketBuilder Members

        public IPacketBuilder Reset()
        {
            m_position = Offset.StartOfHeader;
            return this;
        }

        public IPacketBuilder WriteOperation(RequestOperation operation)
        {
            var bytes = m_buffer.Array;
            m_offset = m_position;
            m_position += Offset.EndOfHeader;

            Array.Clear(bytes, m_offset + 2, Offset.EndOfHeader - 2);
            bytes[m_offset + Offset.StartOfHeader] = 0x80;

            byte opcode = 0x00;
            switch (operation)
            {
                case RequestOperation.Get:
                    break;
                case RequestOperation.Set:
                    opcode = 0x01;
                    break;
                case RequestOperation.Delete:
                    opcode = 0x04;
                    break;
                case RequestOperation.Increment:
                    opcode = 0x05;
                    break;
                case RequestOperation.Decrement:
                    opcode = 0x06;
                    break;
                case RequestOperation.Version:
                    opcode = 0x0B;
                    break;
                case RequestOperation.Flush:
                    opcode = 0x08;
                    break;
                default:
                    throw new NotImplementedException(operation.ToString());
            }

            bytes[m_offset + Offset.OperationCode] = opcode;
            return this;
        }

        public IPacketBuilder WriteDelta(long delta, long initial)
        {
            var buffer = m_buffer.Array;
            buffer[m_offset + Offset.ExtrasLength] = 20 /* delta + initial + expires */;

            BigEndianConverter.GetBytes(delta, buffer, m_position);
            BigEndianConverter.GetBytes(initial, buffer, m_position + 8);
            m_position += 16;
            return this;
        }

        public IPacketBuilder WriteNoReply()
        {
            unsafe
            {
                fixed (byte* b = &m_buffer.Array[m_offset + Offset.OperationCode])
                {
                    *b = (byte)(*b | 0x10);
                }
            }

            return this;
        }

        public IPacketBuilder WriteLength(int length)
        {
            return this;
        }

        public IPacketBuilder WriteVersion(long version)
        {
            if (version != 0)
            {
                var buffer = m_buffer.Array;
                BigEndianConverter.GetBytes(version, buffer, m_offset + Offset.Version);
            }

            return this;
        }

        public IPacketBuilder WriteFlags(int typeCode)
        {
            var buffer = m_buffer.Array;
            buffer[m_offset + Offset.ExtrasLength] = 8;

            BigEndianConverter.GetBytes(typeCode, buffer, m_position);
            m_position += 4;
            return this;
        }

        public IPacketBuilder WriteExpires(int expires)
        {
            var buffer = m_buffer.Array;

            BigEndianConverter.GetBytes(expires, buffer, m_position);
            m_position += 4;
            return this;
        }

        public IPacketBuilder WriteKey(byte[] key)
        {
            var length = key.Length;
            var buffer = m_buffer.Array;
            BigEndianConverter.GetBytes((short)length, buffer, m_offset + Offset.KeyLength);

            EnsureMore(length);

            WriteBytes(key, 0, length);
            return this;
        }

        public IPacketBuilder WriteDelay(int delay)
        {
            throw new NotImplementedException();
        }

        public IPacketBuilder WriteValue(ArraySegment<byte> bytes)
        {
            EnsureMore(bytes.Count);

            WriteBytes(bytes.Array, bytes.Offset, bytes.Count);
            return this;
        }

        public int WriteTo(IBinaryWriter writer)
        {
            var totalLength = m_position - m_offset - Offset.EndOfHeader;
            var buffer = m_buffer.Array;

            BigEndianConverter.GetBytes(totalLength, buffer, m_offset + Offset.TotalBodyLength);

            return writer.Write(m_buffer.Array, 0, m_position);
        }

        #endregion

        private unsafe void WriteBytes(byte[] bytes, int offset, int count)
        {
            if (count <= 8)
            {
                unsafe
                {
                    fixed (byte* b = &m_buffer.Array[m_position])
                    {
                        m_position += count;
                        while (--count >= 0)
                        {
                            b[count] = bytes[offset + count];
                        }
                    }
                }
            }
            else
            {
                Buffer.BlockCopy(bytes, offset, m_buffer.Array, m_position, count);
                m_position += count;
            }
        }

        private void EnsureMore(int count)
        {
            m_buffer.EnsureCapacity(m_position + count);
        }
    }
}
