using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Abstractions.Serialization.Formatters;

namespace ReusableLibrary.Memcached.Protocol
{
    public abstract class AbstractProtocolFactory : Disposable, IProtocolFactory
    {
        private readonly IClient m_client;
        private readonly ProtocolOptions m_options;
        private readonly object m_sync = new object();
        private readonly IPool<IProtocol> m_pool;

        protected AbstractProtocolFactory(string name, IClientFactory clientFactory, ProtocolOptions options)
        {
            m_client = clientFactory.Client();
            m_options = options;
            m_pool = new LazyPool<IProtocol>(
                new SynchronizedPool<IProtocol>(m_sync,
                    new StackPool<IProtocol>(name, options.MaxPoolSize),
                    options.PoolAccessTimeout),
                CreateFactory, ReleaseFactory);
        }

        #region IProtocolFactory Members

        public virtual Pooled<IProtocol> AquireProtocol()
        {
            return new Pooled<IProtocol>(m_pool, null);
        }

        public virtual IObjectFormatter CreateObjectFormatter()
        {
            return new NullObjectFormatter(new DeflateObjectFormatter(new ArrayObjectFormatter(
                new SimpleObjectFormatter(Encoding.UTF8,
                    new RuntimeObjectFormatter(new BinaryFormatter(), null)))));
        }

        public virtual Func2<Action2<IClientConnection, object>, bool> Context(object state)
        {
            return m_client.Context(state);
        }

        public abstract IEncoder CreateEncoder();

        public abstract IPacketBuilder CreatePacketBuilder(Buffer<byte> buffer);

        public abstract IPacketParser CreatePacketParser(IBinaryReader reader, Buffer<byte> buffer);

        public abstract ICommandWriter CreateCommandWriter(IPacketBuilder builder);

        public abstract ICommandReader CreateCommandReader(IPacketParser parser);

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_pool.Dispose();
            }
        }

        private IProtocol CreateFactory(object state)
        {
            return new DefaultProtocol(this, m_options);
        }

        private void ReleaseFactory(IProtocol protocol)
        {
        }
    }
}
