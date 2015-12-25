using System;
using System.IO;
using System.IO.Compression;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public sealed class DeflateObjectFormatter : CompressedObjectFormatter
    {
        public DeflateObjectFormatter(IObjectFormatter inner)
            : base(inner)
        {
            CompressionThreshold = 64 * 1024;
        }

        public override ArraySegment<byte> Compress(ArraySegment<byte> data)
        {
            using (var source = new MemoryStream(data.Count))
            {
                using (var compressor = new DeflateStream(source, CompressionMode.Compress, true))
                {
                    compressor.Write(data.Array, data.Offset, data.Count);
                }

                return new ArraySegment<byte>(source.GetBuffer(), 0, (int)source.Position);
            }            
        }

        public override ArraySegment<byte> Decompress(ArraySegment<byte> data)
        {
            using (var source = new MemoryStream(data.Array, data.Offset, data.Count, false, true))
            {
                using (var decompressor = new DeflateStream(source, CompressionMode.Decompress, true))
                {
                    using (var destination = new MemoryStream(data.Count))
                    {
                        var tmp = new byte[data.Count];
                        int read;
                        while ((read = decompressor.Read(tmp, 0, tmp.Length)) != 0)
                        {
                            destination.Write(tmp, 0, read);
                        }
                        
                        return new ArraySegment<byte>(destination.GetBuffer(), 0, (int)destination.Position);
                    }
                }
            }
        }
    }
}
