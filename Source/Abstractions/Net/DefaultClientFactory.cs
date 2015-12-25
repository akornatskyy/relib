using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Cryptography;

namespace ReusableLibrary.Abstractions.Net
{
    public sealed class DefaultClientFactory : Disposable, IClientFactory
    {
        private readonly IClient m_master;
        private readonly IClient[] m_slaves;

        public DefaultClientFactory(ConnectionOptions connectionOptions)
        {
            m_master = CreateClient(connectionOptions);
        }

        public DefaultClientFactory(ConnectionOptions[] connectionOptions,
            DistributedOptions distributedOptions, 
            IHashAlgorithmProvider hashAlgorithmProvider)
        {
            if (connectionOptions.Length > 1)
            {
                var distributed = new Distributed(distributedOptions, hashAlgorithmProvider);
                var clients = new IClient[connectionOptions.Length];
                for (int i = 0; i < connectionOptions.Length; i++)
                {
                    clients[i] = CreateClient(connectionOptions[i]);
                }

                distributed.AddRange(clients);
                m_slaves = clients;
                m_master = distributed;
            }
            else
            {
                m_master = CreateClient(connectionOptions[0]);
            }
        }

        #region ICacheFactory Members
        
        public IClient Client()
        {
            return m_master;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_master.Dispose();
                if (m_slaves != null)
                {
                    EnumerableHelper.ForEach(m_slaves, slave => Disposable.ReleaseFactory(slave));
                }
            }
        }

        private static IClient CreateClient(ConnectionOptions options)
        {
            return new Client<TcpClientConnection>(options, 
                TcpClientConnection.CreateFactory, 
                TcpClientConnection.ReleaseFactory);
        }
    }
}
