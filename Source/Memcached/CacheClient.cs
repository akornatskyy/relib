using System;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Memcached.Protocol;

namespace ReusableLibrary.Memcached
{
    public sealed class CacheClient : AbstractCache
    {
        private const int MaxValidFor = 60 * 60 * 24 * 30;

        private readonly Pooled<IProtocol> m_pooled;
        private readonly IProtocol m_protocol;

        public CacheClient(IProtocolFactory factory)
        {
            m_pooled = factory.AquireProtocol();
            m_protocol = m_pooled.Item;
        }

        public static int GetExpires(TimeSpan validFor)
        {
            var expires = validFor.Ticks / TimeSpan.TicksPerSecond;
            if (expires > MaxValidFor || expires <= 0)
            {
                throw new ArgumentOutOfRangeException("validFor");
            }

            return (int)expires;
        }

        public static int GetExpires(DateTime expiresAt)
        {
            return DateTimeHelper.ToUnix(expiresAt);
        }

        public override bool Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            return m_protocol.Get(GetOperation.Get, datakey);
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            return m_protocol.Store(StoreOperation.Set, datakey, 0);
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            return m_protocol.Store(StoreOperation.Set, datakey, GetExpires(expiresAt));
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            return m_protocol.Store(StoreOperation.Set, datakey, GetExpires(validFor));
        }

        public override bool Remove(string key)
        {
            return m_protocol.Delete(key);
        }

        public override bool Increment(DataKey<long> datakey, long delta)
        {
            return m_protocol.Increment(datakey, delta, 0);
        }

        public override bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt)
        {
            return m_protocol.Increment(datakey, delta, GetExpires(expiresAt));
        }

        public override bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor)
        {
            return m_protocol.Increment(datakey, delta, GetExpires(validFor));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_pooled.Dispose();
            }
        }
    }
}
