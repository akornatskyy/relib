using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class DistributedProfilingContext : Disposable
    {
        private readonly IClient[] m_clients;

        public DistributedProfilingContext()
        {
            m_clients = new IClient[] 
                { 
                    new Client<ClientConnection>(new ConnectionOptions("Port=11211"),
                        ClientConnection.CreateFactory, ClientConnection.ReleaseFactory),
                    new Client<ClientConnection>(new ConnectionOptions("Port=11311"),
                        ClientConnection.CreateFactory, ClientConnection.ReleaseFactory)
                };
            var distributed = new Distributed(new DistributedOptions(),
                new HashAlgorithmProvider<FNV32ModifiedHashAlgorithm>());
            distributed.AddRange(m_clients);
            Distributed = distributed;
        }

        public IClient Distributed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Distributed.Dispose();
                EnumerableHelper.ForEach(m_clients, client => Disposable.ReleaseFactory(client));
            }
        }
    }
}
