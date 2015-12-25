using System;
using System.Web;
using ReusableLibrary.Abstractions.Caching;

namespace ReusableLibrary.Web.Integration
{
    public sealed class SessionCache : AbstractCache
    {
        private HttpSessionStateBase m_state;

        public SessionCache()
            : this(new HttpContextWrapper(HttpContext.Current).Session)
        {
        }

        public SessionCache(HttpSessionStateBase cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            m_state = cache;
        }

        public override bool Clear()
        {
            return true;
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            var data = m_state[datakey.Key];
            if (data == null)
            {
                return true;
            }

            datakey.Value = (T)data;
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            m_state[datakey.Key] = datakey.Value;
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            m_state[datakey.Key] = datakey.Value;
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            m_state[datakey.Key] = datakey.Value;
            return true;
        }

        public override bool Remove(string key)
        {
            m_state.Remove(key);
            return true;
        }

        public override bool Increment(DataKey<long> datakey, long delta)
        {
            throw new InvalidOperationException(Properties.Resources.SessionCacheIncrementNotValid);
        }

        public override bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt)
        {
            throw new InvalidOperationException(Properties.Resources.SessionCacheIncrementNotValid);
        }

        public override bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor)
        {
            throw new InvalidOperationException(Properties.Resources.SessionCacheIncrementNotValid);
        }
    }
}
