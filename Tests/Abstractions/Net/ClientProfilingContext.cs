using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class ClientProfilingContext : Disposable
    {
        public ClientProfilingContext()
        {
            Client = new Client<ClientConnection>(new ConnectionOptions("Port=11211"),
                ClientConnection.CreateFactory, ClientConnection.ReleaseFactory);
        }

        public IClient Client { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client.Dispose();
            }
        }
    }
}
