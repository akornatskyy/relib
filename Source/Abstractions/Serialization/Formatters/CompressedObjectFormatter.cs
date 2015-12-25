using System;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public abstract class CompressedObjectFormatter : DecoratedObjectFormatter
    {
        public const int CompressedFlag = 0x100;

        protected CompressedObjectFormatter(IObjectFormatter inner)
            : base(inner)
        {
        }

        public int CompressionThreshold { get; set; }

        public override T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            if ((flags & CompressedFlag) != CompressedFlag)
            {
                return base.Deserialize<T>(data, flags);
            }

            return base.Deserialize<T>(Decompress(data), flags & ~CompressedFlag);
        }

        public override ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            var segment = base.Serialize<T>(value, out flags);
            if (segment.Count < CompressionThreshold)
            {
                return segment;
            }

            var compressed = Compress(segment);
            if (compressed.Count >= segment.Count)
            {
                return segment;
            }

            flags |= CompressedFlag;
            return compressed;
        }

        public abstract ArraySegment<byte> Compress(ArraySegment<byte> data);

        public abstract ArraySegment<byte> Decompress(ArraySegment<byte> data);
    }
}
