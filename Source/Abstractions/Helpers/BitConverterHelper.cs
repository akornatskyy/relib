using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class BitConverterHelper
    {
        public static Guid ToGuid(int target)
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(target).CopyTo(bytes, 12);
            return new Guid(bytes);
        }

        public static Guid ToGuid(long target)
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(target).CopyTo(bytes, 8);
            return new Guid(bytes);
        }

        public static byte[] GetBytes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new byte[] { };
            }

            var step = input.Length > 2 && input[2] == '-' ? 3 : 2;
            if (input.Length % step == 1)
            {
                ThrowError();
            }

            var result = new byte[(input.Length + 1) / step];
            for (int i = 0, j = 0; i < result.Length; i++, j += step)
            {
                result[i] = (byte)((ToHex(input[j]) << 4) + ToHex(input[j + 1]));
            }

            return result;
        }

        private static int ToHex(char c)
        {
            return c - ((c >= '0' && c <= '9')
                ? 48 : (c >= 'A' && c <= 'F')
                ? 55 : (c >= 'a' && c <= 'f') 
                ? 87 : ThrowError());
        }

        private static int ThrowError()
        {
            throw new FormatException();
        }
    }
}
