using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Memcached.Protocol
{
    public class TextPacketBuilder : IPacketBuilder
    {
        private const byte Lf = 0x0a;
        private const byte Cr = 0x0d;
        private const byte Separator = 0x20;

        private static readonly Encoding g_encoding = Encoding.ASCII;
        private static readonly byte[] g_get = g_encoding.GetBytes("get");
        private static readonly byte[] g_gets = g_encoding.GetBytes("gets");
        private static readonly byte[] g_set = g_encoding.GetBytes("set");
        private static readonly byte[] g_cas = g_encoding.GetBytes("cas");
        private static readonly byte[] g_delete = g_encoding.GetBytes("delete");
        private static readonly byte[] g_incr = g_encoding.GetBytes("incr");
        private static readonly byte[] g_decr = g_encoding.GetBytes("decr");
        private static readonly byte[] g_flush_all = g_encoding.GetBytes("flush_all");
        private static readonly byte[] g_version = g_encoding.GetBytes("version");
        private static readonly byte[] g_noreply = g_encoding.GetBytes(" noreply");

        private readonly Buffer<byte> m_buffer;
        private int m_position;

        public TextPacketBuilder(Buffer<byte> buffer)
        {
            buffer.EnsureCapacity(260);
            m_buffer = buffer;
        }

        public static bool CheckKey(byte[] key)
        {
            return Array.FindIndex(key, 0, key.Length, k => k == 32 || k == 13 || k == 10) == -1;
        }

        #region IPackerBuilder Members

        public IPacketBuilder Reset()
        {
            m_position = 0;
            return this;
        }

        public IPacketBuilder WriteOperation(RequestOperation operation)
        {
            Debug.Assert(m_position == 0, "position != 0");
            switch (operation)
            {
                case RequestOperation.Get:
                    WriteBytes(g_get, 0, g_get.Length);
                    break;
                case RequestOperation.Set:
                    WriteBytes(g_set, 0, g_set.Length);
                    break;
                case RequestOperation.Delete:
                    WriteBytes(g_delete, 0, g_delete.Length);
                    break;
                case RequestOperation.Increment:
                    WriteBytes(g_incr, 0, g_incr.Length);
                    break;
                case RequestOperation.Decrement:
                    WriteBytes(g_decr, 0, g_decr.Length);
                    break;
                case RequestOperation.Gets:
                    WriteBytes(g_gets, 0, g_gets.Length);
                    break;
                case RequestOperation.CheckAndSet:
                    WriteBytes(g_cas, 0, g_cas.Length);
                    break;
                case RequestOperation.Version:
                    WriteBytes(g_version, 0, g_version.Length);
                    break;
                case RequestOperation.Flush:
                    WriteBytes(g_flush_all, 0, g_flush_all.Length);
                    break;
                default:
                    throw new NotImplementedException(operation.ToString());
            }

            return this;
        }

        public IPacketBuilder WriteKey(byte[] key)
        {
            var count = key.Length;
            if (count > 250)
            {
                throw new ArgumentException("Key length can not be greater 250", "key");
            }

            if (!CheckKey(key))
            {
                throw new ArgumentException("Key must not have space, carriage return or line feed characters", "key");
            }

            EnsureMore(key.Length + 1);

            m_buffer.Array[m_position++] = Separator;
            WriteBytes(key, 0, key.Length);
            return this;
        }

        public IPacketBuilder WriteFlags(int flags)
        {
            var bytes = g_encoding.GetBytes(flags.ToString(CultureInfo.InvariantCulture));

            EnsureMore(bytes.Length + 1);

            m_buffer.Array[m_position++] = Separator;
            WriteBytes(bytes, 0, bytes.Length);
            return this;
        }

        public IPacketBuilder WriteExpires(int expires)
        {
            var bytes = g_encoding.GetBytes(expires.ToString(CultureInfo.InvariantCulture));

            EnsureMore(bytes.Length + 1);

            m_buffer.Array[m_position++] = Separator;
            WriteBytes(bytes, 0, bytes.Length);
            return this;
        }

        public IPacketBuilder WriteLength(int length)
        {
            var bytes = g_encoding.GetBytes(length.ToString(CultureInfo.InvariantCulture));

            EnsureMore(bytes.Length + 1);

            m_buffer.Array[m_position++] = Separator;
            WriteBytes(bytes, 0, bytes.Length);
            return this;
        }

        public IPacketBuilder WriteVersion(long version)
        {
            if (version == 0)
            {
                return this;
            }

            var bytes = g_encoding.GetBytes(version.ToString(CultureInfo.InvariantCulture));

            EnsureMore(bytes.Length + 1);

            m_buffer.Array[m_position++] = Separator;
            WriteBytes(bytes, 0, bytes.Length);
            return this;
        }

        public IPacketBuilder WriteDelta(long delta, long initial)
        {
            var bytes = g_encoding.GetBytes(delta.ToString(CultureInfo.InvariantCulture));

            EnsureMore(bytes.Length + 1);

            m_buffer.Array[m_position++] = Separator;
            WriteBytes(bytes, 0, bytes.Length);
            return this;
        }

        public IPacketBuilder WriteNoReply()
        {
            EnsureMore(g_noreply.Length);
            WriteBytes(g_noreply, 0, g_noreply.Length);
            return this;
        }

        public IPacketBuilder WriteDelay(int delay)
        {
            if (delay > 0)
            {
                var bytes = g_encoding.GetBytes(delay.ToString(CultureInfo.InvariantCulture));

                EnsureMore(bytes.Length + 1);

                m_buffer.Array[m_position++] = Separator;
                WriteBytes(bytes, 0, bytes.Length);
            }

            return this;
        }

        public IPacketBuilder WriteValue(ArraySegment<byte> bytes)
        {
            EnsureMore(bytes.Count + 2);

            m_buffer.Array[m_position++] = Cr;
            m_buffer.Array[m_position++] = Lf;

            WriteBytes(bytes.Array, bytes.Offset, bytes.Count);
            return this;
        }

        public int WriteTo(IBinaryWriter writer)
        {
            EnsureMore(2);
            var array = m_buffer.Array;
            array[m_position++] = Cr;
            array[m_position++] = Lf;
            return writer.Write(array, 0, m_position);
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
