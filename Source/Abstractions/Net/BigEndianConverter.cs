using System;

namespace ReusableLibrary.Abstractions.Net
{
    public static class BigEndianConverter
    {
        public static byte[] GetBytes(short value)
        {
            var buffer = new byte[2];
            GetBytes(value, buffer, 0);
            return buffer;
        }

        public static unsafe void GetBytes(short value, byte[] buffer, int index)
        {
            fixed (byte* pbuffer = &buffer[index])
            {
                pbuffer[0] = (byte)((value >> 8) & 0xFF);
                pbuffer[1] = (byte)(value & 0xFF);
            }
        }

        public static byte[] GetBytes(int value)
        {
            var buffer = new byte[4];
            GetBytes(value, buffer, 0);
            return buffer;
        }

        public static unsafe void GetBytes(int value, byte[] buffer, int index)
        {
            fixed (byte* pbuffer = &buffer[index])
            {
                pbuffer[0] = (byte)((value >> 24) & 0xFF);
                pbuffer[1] = (byte)((value >> 16) & 0xFF);
                pbuffer[2] = (byte)((value >> 8) & 0xFF);
                pbuffer[3] = (byte)(value & 0xFF);
            }
        }

        public static byte[] GetBytes(long value)
        {
            var buffer = new byte[8];
            GetBytes(value, buffer, 0);
            return buffer;
        }

        public static unsafe byte[] GetBytes(long value, byte[] buffer, int index)
        {
            var high = (int)(value >> 32);
            var low = (int)(value & 0xFFFFFFFFL);

            fixed (byte* pbuffer = &buffer[index])
            {
                pbuffer[0] = (byte)((high >> 24) & 0xFF);
                pbuffer[1] = (byte)((high >> 16) & 0xFF);
                pbuffer[2] = (byte)((high >> 8) & 0xFF);
                pbuffer[3] = (byte)(high & 0xFF);

                pbuffer[4] = (byte)((low >> 24) & 0xFF);
                pbuffer[5] = (byte)((low >> 16) & 0xFF);
                pbuffer[6] = (byte)((low >> 8) & 0xFF);
                pbuffer[7] = (byte)(low & 0xFF);
            }

            return buffer;
        }

        public static short GetInt16(byte[] buffer, int index)
        {
            if (index > Int32.MaxValue - 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return (short)((buffer[index] << 8) | buffer[index + 1]);
        }

        public static int GetInt32(byte[] buffer, int index)
        {
            if (index > Int32.MaxValue - 3)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return (buffer[index] << 24) | (buffer[index + 1] << 16) |
                (buffer[index + 2] << 8) | buffer[index + 3];
        }

        public static long GetInt64(byte[] buffer, int index)
        {
            if (index > Int32.MaxValue - 7)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            var high = (uint)((buffer[index] << 24) | (buffer[index + 1] << 16) |
                (buffer[index + 2] << 8) | buffer[index + 3]);
            var low = (uint)((buffer[index + 4] << 24) | (buffer[index + 5] << 16) |
                (buffer[index + 6] << 8) | buffer[index + 7]);
            return (long)high << 32 | low;
        }
    }
}
