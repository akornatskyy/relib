using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class LinkedCacheDependency
    {
        public const string DependencyKey = "d:";

        private readonly ICache m_cache;
        private readonly string m_headKey;
        private readonly DateTime m_expiresAt;

        public LinkedCacheDependency(ICache cache, string headKey)
            : this(cache, headKey, DateTime.MinValue)
        {
        }

        public LinkedCacheDependency(ICache cache, string headKey, DateTime expiresAt)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            if (headKey == null)
            {
                throw new ArgumentNullException("headKey");
            }

            m_cache = cache;
            m_headKey = headKey;
            m_expiresAt = expiresAt;
        }

        public bool Add(string key)
        {
            var head = m_cache.Get<Pair<string>>(m_headKey);
            if (head.First == null)
            {
                head = new Pair<string>(key, null);
                return m_cache.Store(m_headKey, head, m_expiresAt);
            }

            var dependentKey = string.Concat(DependencyKey, key);
            var succeed = m_cache.Store(dependentKey, head, m_expiresAt);
            
            head = new Pair<string>(key, dependentKey);
            return succeed && m_cache.Store(m_headKey, head, m_expiresAt);            
        }

        public bool Remove()
        {
            var succeed = true;
            Pair<string> dependent;
            var key = m_headKey;
            do
            {
                dependent = m_cache.Get<Pair<string>>(key);
                if (dependent.First == null)
                {
                    break;
                }

                succeed = succeed && m_cache.Remove(dependent.First);
                succeed = succeed && m_cache.Remove(key);
                key = dependent.Second;
            }
            while (key != null);

            return succeed;
        }
    }
}
