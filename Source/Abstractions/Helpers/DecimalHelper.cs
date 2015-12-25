using System;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class DecimalHelper
    {
        public static byte[] GetBytes(decimal value)
        {
            var bytes = new byte[16];
            var bits = decimal.GetBits(value);
            Buffer.BlockCopy(BitConverter.GetBytes(bits[0]), 0, bytes, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(bits[1]), 0, bytes, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(bits[2]), 0, bytes, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(bits[3]), 0, bytes, 12, 4);
            return bytes;
        }

        public static decimal ToDecimal(byte[] bytes, int startIndex)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (startIndex >= bytes.Length)
            {
                throw new ArgumentOutOfRangeException(Properties.Resources.DecimalHelperOutOfRange);
            }

            if (startIndex > bytes.Length - 16)
            {
                throw new ArgumentException(Properties.Resources.DecimalHelperTooSmall);
            }

            return new decimal(new[] 
            { 
                BitConverter.ToInt32(bytes, startIndex + 0),
                BitConverter.ToInt32(bytes, startIndex + 4),
                BitConverter.ToInt32(bytes, startIndex + 8),
                BitConverter.ToInt32(bytes, startIndex + 12)
            });
        }
    }
}
