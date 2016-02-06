using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public enum StoreOperation
    {
        None = 0,

        Set = 20,

        Add,

        Replace,

        CheckAndSet
    }
}
