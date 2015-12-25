using System;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public sealed class NullObjectFormatter : DecoratedObjectFormatter
    {
        private static readonly ArraySegment<byte> Empty = new ArraySegment<byte>(new byte[] { });

        public NullObjectFormatter(IObjectFormatter inner)
            : base(inner)
        {
        }

        public override T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            TypeCode typeCode = (TypeCode)(flags & TypeCodeMask);
            if (typeCode != TypeCode.Empty)
            {
                return base.Deserialize<T>(data, flags);
            }

            return default(T);
        }

        public override ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            if (value != null)
            {
                return base.Serialize<T>(value, out flags);
            }

            flags = (int)TypeCode.Empty;
            return Empty;
        }
    }
}
