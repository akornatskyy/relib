using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public class DecoratedCache : AbstractCache
    {
        private readonly ICache m_inner;

        public DecoratedCache(ICache inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            m_inner = inner;
        }

        public override bool Clear()
        {
            return m_inner.Clear();
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            return m_inner.Get(datakey);
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            return m_inner.Store(datakey);
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            return m_inner.Store(datakey, expiresAt);
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            return m_inner.Store(datakey, validFor);
        }

        public override bool Remove(string key)
        {
            return m_inner.Remove(key);
        }

        public override bool Increment(DataKey<long> datakey, long delta)
        {
            return m_inner.Increment(datakey, delta);
        }

        public override bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt)
        {
            return m_inner.Increment(datakey, delta, expiresAt);
        }

        public override bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor)
        {
            return m_inner.Increment(datakey, delta, validFor);
        }
    }
}
