using System;
using System.IO;
using System.Runtime.Serialization;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public sealed class RuntimeObjectFormatter : DecoratedObjectFormatter
    {
        public const int RuntimeObjectFlag = 0x200;

        private IFormatter m_formatter;

        public RuntimeObjectFormatter(IFormatter formatter, IObjectFormatter inner)
            : base(inner)
        {
            m_formatter = formatter;
        }

        public override T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            if ((flags & RuntimeObjectFlag) != RuntimeObjectFlag)
            {
                return base.Deserialize<T>(data, flags);
            }

            using (var source = new MemoryStream(data.Array, data.Offset, data.Count, false, false))
            {
                return (T)m_formatter.Deserialize(source);
            }
        }

        public override ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            using (var destination = new MemoryStream())
            {
                m_formatter.Serialize(destination, value);
                flags = RuntimeObjectFlag | (int)Type.GetTypeCode(typeof(T));
                return new ArraySegment<byte>(destination.GetBuffer(), 0, (int)destination.Position);
            }
        }
    }
}
