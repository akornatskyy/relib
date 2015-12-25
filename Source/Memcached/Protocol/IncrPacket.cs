using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class IncrPacket
    {
        public byte[] Key { get; set; }

        public long Delta { get; set; }

        public long InitialValue { get; set; }

        public int Expires { get; set; }
    }
}
