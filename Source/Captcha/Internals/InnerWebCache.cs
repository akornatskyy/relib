using System;
using System.Web;
using System.Web.Caching;
using ReusableLibrary.Abstractions.Caching;

namespace ReusableLibrary.Captcha.Internals
{
    public sealed class InnerWebCache : AbstractCache
    {
        private Cache m_cache;

        public InnerWebCache()
            : this(HttpRuntime.Cache)
        {
        }

        public InnerWebCache(Cache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            m_cache = cache;
        }

        public override bool Clear()
        {
            return true;
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            var data = m_cache.Get(datakey.Key);
            if (data == null)
            {
                return true;
            }

            datakey.Value = (T)data;
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            m_cache.Insert(datakey.Key, datakey.Value);
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            m_cache.Insert(datakey.Key, datakey.Value, null, expiresAt, Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal, null);
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            m_cache.Insert(datakey.Key, datakey.Value, null, DateTime.Now.Add(validFor), Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal, null);
            return true;
        }

        public override bool Remove(string key)
        {
            m_cache.Remove(key);
            return true;
        }
    }
}
