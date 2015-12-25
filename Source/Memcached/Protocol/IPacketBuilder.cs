using System;
using ReusableLibrary.Abstractions.IO;

namespace ReusableLibrary.Memcached.Protocol
{
    public interface IPacketBuilder
    {
        IPacketBuilder Reset();

        IPacketBuilder WriteOperation(RequestOperation operation);

        IPacketBuilder WriteKey(byte[] key);

        IPacketBuilder WriteFlags(int typeCode);

        IPacketBuilder WriteExpires(int expires);

        IPacketBuilder WriteLength(int length);

        IPacketBuilder WriteVersion(long version);

        IPacketBuilder WriteDelta(long delta, long initial);

        IPacketBuilder WriteNoReply();

        IPacketBuilder WriteDelay(int delay);

        IPacketBuilder WriteValue(ArraySegment<byte> bytes);

        int WriteTo(IBinaryWriter writer);
    }
}
