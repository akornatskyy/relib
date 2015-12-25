using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public class BinaryCommandWriter : ICommandWriter
    {
        private readonly IPacketBuilder m_builder;

        public BinaryCommandWriter(IPacketBuilder builder)
        {
            m_builder = builder;
        }

        #region ICommandWriter Members

        public void Store(StorePacket packet, bool noreply)
        {
            m_builder
                .Reset()
                .WriteOperation((RequestOperation)packet.Operation)
                .WriteVersion(packet.Version)
                .WriteFlags(packet.Flags)
                .WriteExpires(packet.Expires)                
                .WriteKey(packet.Key)
                .WriteValue(packet.Value);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }
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
            throw new NotImplementedException();
        }

        public void Delete(byte[] key, bool noreply)
        {
            m_builder
                .Reset()
                .WriteOperation(RequestOperation.Delete);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }
            
            m_builder.WriteKey(key);                
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
                .WriteOperation(RequestOperation.Flush);
            
            if (noreply)
            {
                m_builder.WriteNoReply();
            }
            
            m_builder.WriteDelay(delay);
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
                .WriteDelta(delta, packet.InitialValue)
                .WriteExpires(packet.Expires)
                .WriteKey(packet.Key);

            if (noreply)
            {
                m_builder.WriteNoReply();
            }
        }

        #endregion
    }
}
