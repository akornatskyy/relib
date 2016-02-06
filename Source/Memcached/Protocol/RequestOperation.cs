using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public enum RequestOperation
    {
        None = 0,

        Get = 10,

        Gets,
        
        Set = 20,
        
        Add,
        
        Replace,
        
        CheckAndSet,

        Delete = 30,
        
        Flush,

        Version = 40,

        Increment = 50,

        Decrement
    }
}
