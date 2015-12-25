using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class ValuePacket
    {
        public byte[] Key { get; set; }

        public int Flags { get; set; }

        public long Version { get; set; }

        public ArraySegment<byte> Value { get; set; }
    }
}
