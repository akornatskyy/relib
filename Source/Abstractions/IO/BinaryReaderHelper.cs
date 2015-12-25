using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.IO
{
    public static class BinaryReaderHelper
    {
        public static bool ReadTo(IBinaryReader reader, byte[] buffer, int offset, int count, out int read)
        {
            int start = offset;
            while ((read = reader.Read(buffer, offset, count)) > 0)
            {
                if (read == count)
                {
                    read = count;
                    return true;
                }

                offset += read;
                count -= read;
            }

            read = offset - start;
            return false;
        }

        public static void ReadTo(IBinaryReader reader, byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return;
            }

            int read = 0;
            while (!ReadTo(reader, buffer, offset, count, out read))
            {
                offset += read;
                count -= read;
            }            
        }

        public static int ReadToken(IBinaryReader reader, byte[] buffer, Predicate<byte> predicate)
        {
            return ReadToken(reader, buffer, 0, buffer.Length, predicate);
        }

        public static int ReadToken(IBinaryReader reader, byte[] buffer, int offset, int count, Predicate<byte> predicate)
        {
            while (count-- > 0)
            {
                ReadTo(reader, buffer, offset, 1);
                if (predicate(buffer[offset]))
                {
                    return offset;
                }

                checked
                {
                    offset++;
                }
            } 

            return -1;
        }
    }
}
