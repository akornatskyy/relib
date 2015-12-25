using System;
using System.Globalization;
using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Memcached.Protocol
{
    public class TextPacketParser : IPacketParser
    {
        private const int End = 2573;
        private const int Deleted = 21573;
        private const int Error = 21071;
        private const int ClientError = 20037;
        private const int ServerError = 17750;
        private const int Exists = 21587;
        private const int NotFound = 18015;
        private const int NotStored = 21343;
        private const int Stored = 17746;
        private const int Value = 17749;

        private static readonly Encoding g_encoding = Encoding.ASCII;

        private readonly IBinaryReader m_reader;
        private readonly Buffer<byte> m_buffer;
        private readonly byte[] m_statusBuffer = new byte[7];

        public TextPacketParser(IBinaryReader reader, Buffer<byte> buffer)
        {
            buffer.EnsureCapacity(260);
            m_reader = reader;
            m_buffer = buffer;
        }

        #region IPacketParser Members

        public ResponseStatus ReadStatus()
        {
            var status = ResponseStatus.Unknown;

            var buffer = m_statusBuffer;
            BinaryReaderHelper.ReadTo(m_reader, buffer, 0, 5);

            var remains = 0;
            var code = buffer[3] | (buffer[4] << 8);
            switch (code)
            {    
                case End:
                    status = ResponseStatus.NoError;
                    break;
                case Value:
                    status = ResponseStatus.Value;
                    remains = 1;
                    break;
                case Stored:
                    status = ResponseStatus.NoError;
                    remains = 3;
                    break;
                case Deleted:
                    status = ResponseStatus.NoError;
                    remains = 4;
                    break;
                case NotFound:
                    status = ResponseStatus.KeyNotFound;
                    remains = 6;
                    break;
                case Exists:
                    status = ResponseStatus.KeyExists;
                    remains = 3;
                    break;                
                case NotStored:
                    status = ResponseStatus.ItemNotStored;
                    remains = 7;
                    break;
                case ServerError:
                case ClientError:                
                    Read(8);
                    throw new InvalidOperationException(ReadError());
                case Error:
                    Read(2);
                    throw new InvalidOperationException();
                default:
                    throw new NotImplementedException();
            }

            if (remains > 0)
            {
                BinaryReaderHelper.ReadTo(m_reader, buffer, 0, remains);
            }

            return status;
        }

        public byte[] ReadKey()
        {
            var position = ReadToken();
            var key = new byte[position];
            Buffer.BlockCopy(m_buffer.Array, 0, key, 0, position);
            return key;
        }

        public int ReadFlags()
        {
            var position = ReadToken();
            return Int32.Parse(g_encoding.GetString(m_buffer.Array, 0, position), CultureInfo.InvariantCulture);
        }

        public int ReadLength()
        {
            var position = ReadToken();
            var str = g_encoding.GetString(m_buffer.Array, 0, position);
            return Int32.Parse(str, CultureInfo.InvariantCulture);
        }

        public long ReadVersion()
        {
            var position = ReadToken();
            var str = g_encoding.GetString(m_buffer.Array, 0, position);
            return Int64.Parse(str, CultureInfo.InvariantCulture);
        }

        public long ReadIncrement()
        {
            var position = ReadLine();
            var str = g_encoding.GetString(m_buffer.Array, 0, position);
            long incr;
            if (!Int64.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out incr))
            {
                //// NOT_FOUND
                //// CLIENT_ERROR invalid numeric delta argument
                return -1L;
            }

            return incr;
        }

        public ArraySegment<byte> ReadValue(int length)
        {
            m_buffer.EnsureCapacity(length + 2);
            BinaryReaderHelper.ReadTo(m_reader, m_buffer.Array, 0, length + 2);
            return new ArraySegment<byte>(m_buffer.Array, 0, length);
        }

        #endregion

        private string ReadError()
        {
            var position = ReadLine();
            return position > 0 ? g_encoding.GetString(m_buffer.Array, 0, position) : string.Empty;
        }

        private void Read(int count)
        {
            BinaryReaderHelper.ReadTo(m_reader, m_buffer.Array, 0, count);
        }

        private int ReadLine()
        {
            var position = BinaryReaderHelper.ReadToken(m_reader, m_buffer.Array, 0, m_buffer.Capacity, 
                token => token == 0x0A);
            if (position == -1)
            {
                throw new InvalidOperationException("End of line not found");
            }

            return --position;
        }

        private int ReadToken()
        {
            var position = BinaryReaderHelper.ReadToken(m_reader, m_buffer.Array, 0, m_buffer.Capacity, 
                token => token == 0x20 || token == 0x0A);
            if (position == -1)
            {
                throw new InvalidOperationException("Token not found");
            }

            if (m_buffer.Array[position] == 0x0A)
            {
                position--;
            }

            return position;
        }
    }
}
