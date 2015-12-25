using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Net
{
    public class Distributed : AbstractDistributed
    {
        private readonly object m_sync = new object();

        public Distributed(DistributedOptions options, IHashAlgorithmProvider hashAlgorithmProvider)
        {
            Options = options;
            Pool = new SynchronizedKetamaPool<IClient>("Distributed", hashAlgorithmProvider, options.ClientAccessTimeout);
            Idle = new SynchronizedPool<IClient>(m_sync, new IdleTimeoutPool<IClient>(m_sync,
                        new StackPool<IClient>("Distributed Idle", options.IdlePoolSize),
                        RetryIdled, options.IdleLeaseTime, options.IdleLeaseTime), options.IdleAccessTimeout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Idle.Dispose();
                Pool.Dispose();
            }
        }
    }
}
