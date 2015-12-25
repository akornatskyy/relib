using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class StorePacket
    {
        public StoreOperation Operation { get; set; }

        public byte[] Key { get; set; }

        public int Flags { get; set; }

        public int Expires { get; set; }

        public long Version { get; set; }

        public ArraySegment<byte> Value { get; set; }
    }
}
