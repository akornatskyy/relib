using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public interface IPacketParser
    {
        ResponseStatus ReadStatus();

        byte[] ReadKey();

        int ReadFlags();

        int ReadLength();

        long ReadVersion();

        long ReadIncrement();

        ArraySegment<byte> ReadValue(int length);
    }
}
