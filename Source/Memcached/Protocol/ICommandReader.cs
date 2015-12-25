using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public interface ICommandReader
    {
        void ReadSucceed();

        ValuePacket ReadValue(bool includeVersion);
        
        bool ReadStored();

        bool ReadDeleted();

        long ReadIncrement();
    }
}
