using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public interface ICommandWriter
    {
        void Store(StorePacket packet, bool noreply);

        void Get(GetOperation operation, byte[] key);

        void GetMany(GetOperation operation, byte[][] keys);

        void Delete(byte[] key, bool noreply);

        void Version();

        void Flush(int delay, bool noreply);

        void Incr(IncrPacket packet, bool noreply);
    }
}
