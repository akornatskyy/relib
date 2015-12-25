using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Memcached.Protocol
{
    public class ProtocolOptions
    {
        public ProtocolOptions()
            : this(string.Empty)
        {
        }

        public ProtocolOptions(string options)
        {
            var items = StringHelper.ParseOptions(options);
            BufferSize = NameValueCollectionHelper.ConvertToInt32(items, "buffer size", 1024);
            NoReply = NameValueCollectionHelper.ConvertToBoolean(items, "no reply", false);
            PoolAccessTimeout = NameValueCollectionHelper.ConvertToInt32(items, "pool access timeout", 100);
            MaxPoolSize = NameValueCollectionHelper.ConvertToInt32(items, "max pool size", 16);
        }

        public int BufferSize { get; private set; }

        public bool NoReply { get; private set; }

        public int PoolAccessTimeout { get; private set; }

        public int MaxPoolSize { get; private set; }
    }
}
