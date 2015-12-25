using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class CacheInstrumentation : AbstractCache
    {
        private readonly ICache m_inner;
        private readonly ICachingInstrumentationProvider m_instrumentationProvider;

        public CacheInstrumentation(ICache inner, ICachingInstrumentationProvider instrumentationProvider)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            if (instrumentationProvider == null)
            {
                throw new ArgumentNullException("instrumentationProvider");
            }

            m_inner = inner;
            m_instrumentationProvider = instrumentationProvider;
        }

        public override bool Clear()
        {
            return m_inner.Clear();
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            if (!m_inner.Get<T>(datakey))
            {
                m_instrumentationProvider.FireFailed();
                return false;
            }

            m_instrumentationProvider.FireAccessed(datakey.HasValue);
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            var succeed = m_inner.Store(datakey);
            m_instrumentationProvider.FireStored(succeed);
            return succeed;
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            var succeed = m_inner.Store(datakey, expiresAt);
            m_instrumentationProvider.FireStored(succeed);
            return succeed;
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            var succeed = m_inner.Store(datakey, validFor);
            m_instrumentationProvider.FireStored(succeed);
            return succeed;
        }

        public override bool Remove(string key)
        {
            var succeed = m_inner.Remove(key);
            m_instrumentationProvider.FireRemoved(succeed);
            return succeed;
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
