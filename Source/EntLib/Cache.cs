using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using ReusableLibrary.Abstractions.Caching;

namespace ReusableLibrary.EntLib
{
    public sealed class Cache : AbstractCache
    {
        private readonly ICacheManager m_cacheManager;

        public Cache(string cacheManagerName)
            : this(CacheFactory.GetCacheManager(cacheManagerName))
        {
        }

        public Cache(ICacheManager cacheManager)
        {
            m_cacheManager = cacheManager;
        }

        public override bool Clear()
        {
            // Removes all items from the cache.
            m_cacheManager.Flush();
            return true;
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            var data = m_cacheManager.GetData(datakey.Key);
            if (data == null)
            {
                return true;
            }

            datakey.Value = (T)data;
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            // Adds new CacheItem to cache. If another item already exists with 
            // the same key, that item is removed before the new item is added.
            m_cacheManager.Add(datakey.Key, datakey.Value);
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            m_cacheManager.Add(datakey.Key, datakey.Value, CacheItemPriority.Normal, null, 
                new AbsoluteTime(expiresAt.ToLocalTime()));
            return true;
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            m_cacheManager.Add(datakey.Key, datakey.Value, CacheItemPriority.Normal, null,
                new AbsoluteTime(validFor));
            return true;
        }

        public override bool Remove(string key)
        {
            // Removes the given item from the cache. If no item exists with that key, 
            // this method does nothing. 
            m_cacheManager.Remove(key);
            return true;
        }
    }
}
