using System;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public sealed class ArrayObjectFormatter : DecoratedObjectFormatter
    {
        public const int ArrayFlag = 0x80;

        public ArrayObjectFormatter(IObjectFormatter inner)
            : base(inner)
        {
        }

        public override T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            if ((flags & ArrayFlag) != ArrayFlag)
            {
                return base.Deserialize<T>(data, flags);
            }

            var array = new byte[data.Count];
            Buffer.BlockCopy(data.Array, data.Offset, array, 0, data.Count);
            return (T)(object)array;
        }

        public override ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            byte[] data = value as byte[];
            if (data == null)
            {
                return base.Serialize<T>(value, out flags);
            }

            flags = ArrayFlag;
            return new ArraySegment<byte>(data, 0, data.Length);
        }
    }
}
