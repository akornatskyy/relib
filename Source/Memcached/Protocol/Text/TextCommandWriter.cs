using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public class TextCommandWriter : ICommandWriter
    {
        private readonly IPacketBuilder m_builder;

        public TextCommandWriter(IPacketBuilder builder)
        {
            m_builder = builder;
        }

        #region ICommandWriter Members

        public void Store(StorePacket packet, bool noreply)
        {
            m_builder
                .Reset()
                .WriteOperation((RequestOperation)packet.Operation)
                .WriteKey(packet.Key)
                .WriteFlags(packet.Flags)
                .WriteExpires(packet.Expires)
                .WriteLength(packet.Value.Count)
                .WriteVersion(packet.Version);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }

            m_builder.WriteValue(packet.Value);
        }

        public void Get(GetOperation operation, byte[] key)
        {
            m_builder
                .Reset()
                .WriteOperation((RequestOperation)operation)
                .WriteKey(key);
        }

        public void GetMany(GetOperation operation, byte[][] keys)
        {
            m_builder
                .Reset()
                .WriteOperation((RequestOperation)operation);
            foreach (var key in keys)
            {
                m_builder.WriteKey(key);
            }
        }

        public void Delete(byte[] key, bool noreply)
        {
            m_builder
                .Reset()
                .WriteOperation(RequestOperation.Delete)
                .WriteKey(key);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }
        }

        public void Version()
        {
            m_builder
                .Reset()
                .WriteOperation(RequestOperation.Version);
        }

        public void Flush(int delay, bool noreply)
        {
            m_builder
                .Reset()
                .WriteOperation(RequestOperation.Flush)
                .WriteDelay(delay);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }
        }

        public void Incr(IncrPacket packet, bool noreply)
        {
            RequestOperation op;
            long delta;
            if (packet.Delta >= 0L)
            {
                op = RequestOperation.Increment;
                delta = packet.Delta;
            }
            else
            {
                op = RequestOperation.Decrement;
                delta = -packet.Delta;
            }

            m_builder
                .Reset()
                .WriteOperation(op)
                .WriteKey(packet.Key)
                .WriteDelta(delta, 0L);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }
        }

        #endregion
    }
}
