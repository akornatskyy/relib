using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public enum ResponseStatus
    {
        NoError,

        KeyNotFound,

        KeyExists,

        ValueTooLarge,

        InvalidArguments,

        ItemNotStored,

        UnknownCommand = 0x81,

        OutOfMemory,

        Value = 0xF0,

        Unknown
    }
}
