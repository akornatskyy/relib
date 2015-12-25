using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Net
{
    public class DistributedOptions
    {
        public DistributedOptions()
            : this(string.Empty)
        {
        }

        public DistributedOptions(string options)
        {
            var items = StringHelper.ParseOptions(options);
            ClientAccessTimeout = NameValueCollectionHelper.ConvertToInt32(items, "client access timeout", 100);
            IdlePoolSize = NameValueCollectionHelper.ConvertToInt32(items, "idle pool size", 16);
            IdleLeaseTime = NameValueCollectionHelper.ConvertToInt32(items, "idle lease time", 10 * 1000);
            IdleAccessTimeout = NameValueCollectionHelper.ConvertToInt32(items, "idle access timeout", 100);
        }

        public int ClientAccessTimeout { get; private set; }

        public int IdlePoolSize { get; private set; }

        public int IdleLeaseTime { get; private set; }

        public int IdleAccessTimeout { get; private set; }
    }
}
