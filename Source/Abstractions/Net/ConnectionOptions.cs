using System;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Net
{
    public class ConnectionOptions
    {
        public ConnectionOptions(string options)
        {
            var items = StringHelper.ParseOptions(options);
            Name = items["name"] ?? "Default";
            Server = items["server"] ?? IpNumberHelper.Localhost;
            Port = NameValueCollectionHelper.ConvertToInt32(items, "port");
            DnsTimeout = NameValueCollectionHelper.ConvertToInt32(items, "dns timeout", 250);
            ConnectTimeout = NameValueCollectionHelper.ConvertToInt32(items, "connect timeout", 250);
            SendTimeout = NameValueCollectionHelper.ConvertToInt32(items, "send timeout", 250);
            SendBufferSize = NameValueCollectionHelper.ConvertToInt32(items, "send buffer size", 8 * 1024);
            ReceiveTimeout = NameValueCollectionHelper.ConvertToInt32(items, "receive timeout", 250);
            ReceiveBufferSize = NameValueCollectionHelper.ConvertToInt32(items, "receive buffer size", 8 * 1024);
            MaxPoolSize = NameValueCollectionHelper.ConvertToInt32(items, "max pool size", 16);
            PoolAccessTimeout = NameValueCollectionHelper.ConvertToInt32(items, "pool access timeout", 100);
            PoolWaitTimeout = NameValueCollectionHelper.ConvertToInt32(items, "pool wait timeout", 250);
            IdleTimeout = NameValueCollectionHelper.ConvertToInt32(items, "idle timeout", 30 * 1000);
            LeaseTimeout = NameValueCollectionHelper.ConvertToInt32(items, "lease timeout", 15 * 60 * 1000);

            FullName = String.Format(CultureInfo.InvariantCulture, "Name={0}, Server={1}:{2}", 
                Name, Server, Port);
        }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public string Server { get; private set; }

        public int Port { get; private set; }

        public int DnsTimeout { get; private set; }

        public int ConnectTimeout { get; private set; }

        public int SendTimeout { get; private set; }

        public int SendBufferSize { get; private set; }

        public int ReceiveTimeout { get; private set; }

        public int ReceiveBufferSize { get; private set; }

        public int MaxPoolSize { get; private set; }

        public int PoolAccessTimeout { get; private set; }

        public int PoolWaitTimeout { get; private set; }

        public int IdleTimeout { get; private set; }

        public int LeaseTimeout { get; private set; }
    }
}
