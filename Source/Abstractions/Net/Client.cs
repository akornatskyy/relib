using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Net
{
    public sealed class Client<T> : AbstractClient<T>, IClient
        where T : class, IClientConnection
    {
        private readonly object m_sync = new object();

        public Client(ConnectionOptions options, Func2<object, T> createfactory, Action<T> releasefactory)
        {
            Options = options;
            Pool = new WaitPool<T>(
                        new LazyPool<T>(
                            new SynchronizedPool<T>(m_sync,
                                new IdleTimeoutPool<T>(m_sync,
                                    new StackPool<T>(String.Concat("Client ", options.Name), options.MaxPoolSize),
                                    releasefactory, options.IdleTimeout, options.LeaseTimeout),
                                options.PoolAccessTimeout),
                            createfactory, releasefactory),
                    options.PoolWaitTimeout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Pool.Dispose();
            }
        }
    }
}
