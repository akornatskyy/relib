using System;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Memcached.Protocol;

namespace ReusableLibrary.Memcached.Tests.Infrastructure
{
    public class CacheClientContext : Disposable
    {
        private readonly IClientFactory m_clientFactory;
        private IProtocolFactory m_protocolFactory;
        private ICache m_client;

        public CacheClientContext()
        {
            m_clientFactory = new DefaultClientFactory(new[] 
                {  
                    new ConnectionOptions("Name = B; Server = 127.0.0.1; Port = 11311;Max Pool Size = 20; Pool Access Timeout = 150; Connect Timeout = 750; Idle Timeout = 19000"),
                    new ConnectionOptions("Name = A; Server = 127.0.0.1; Port = 11211;Max Pool Size = 20; Pool Access Timeout = 150; Connect Timeout = 750; Idle Timeout = 21000")
                },
                new DistributedOptions(),
                new HashAlgorithmProvider<FNV32ModifiedHashAlgorithm>());
        }

        public ICache CacheClientText()
        {
            return CacheClientText(string.Empty);
        }

        public ICache CacheClientText(string options)
        {
            if (m_client != null)
            {
                return m_client;
            }

            m_protocolFactory = new TextProtocolFactory(m_clientFactory,
                new ProtocolOptions(options));
            m_client = new CacheClient(m_protocolFactory);
            return m_client;
        }

        public ICache CacheClientBinary()
        {
            return CacheClientBinary(string.Empty);
        }

        public ICache CacheClientBinary(string options)
        {
            if (m_client != null)
            {
                return m_client;
            }

            m_protocolFactory = new BinaryProtocolFactory(m_clientFactory,
                new ProtocolOptions(options));
            m_client = new CacheClient(m_protocolFactory);
            return m_client;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Disposable.ReleaseFactory(m_client);
            if (m_protocolFactory != null)
            {
                m_protocolFactory.Dispose();
            }

            m_clientFactory.Dispose();
        }
    }
}
