using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public enum StoreOperation
    {
        Set = 20,

        Add,

        Replace,

        CheckAndSet
    }
}
