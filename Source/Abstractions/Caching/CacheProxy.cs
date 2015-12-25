using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Caching
{
    /// <summary>
    /// The main purpose is to be a fast cache behind a "real" cache.
    /// The lifetime of this cache is intended to be short and controlled 
    /// externally (e.g. per web request). So it is reasonably safe to
    /// ignore expiration.
    /// It is not thread safe.
    /// </summary>
    [DebuggerDisplay("Count = {Count}, MissCount = {MissCount}")]
    public sealed class CacheProxy : AbstractCache
    {
        private readonly ICache m_inner;
        private readonly IDictionary m_items;
        private readonly IList<string> m_misses;

        public CacheProxy(ICache inner)
            : this(inner, 20)
        {
        }

        public CacheProxy(ICache inner, int capacity)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            m_inner = inner;
            m_items = new Hashtable(capacity, StringComparer.Ordinal);
            m_misses = new List<string>(capacity);
        }

        public int Count 
        { 
            get { return m_items.Count; } 
        }

        public int MissCount
        {
            get { return m_misses.Count; }
        }

        public override bool Clear()
        {
            m_items.Clear();
            m_misses.Clear();
            return m_inner.Clear();
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            var key = datakey.Key;
            var data = m_items[key];
            if (data == null)
            {
                if (m_misses.Contains(key))
                {
                    return true;
                }

                if (!m_inner.Get<T>(datakey))
                {
                    return false;
                }

                if (datakey.HasValue)
                {
                    m_items[key] = datakey.Value;
                }
                else
                {
                    m_misses.Add(key);
                }

                return true;
            }

            datakey.Value = (T)data;

            return true;
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            var succeed = m_inner.Store(datakey);
            if (succeed)
            {
                m_items[datakey.Key] = datakey.Value;
            }

            return succeed;
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            var succeed = m_inner.Store(datakey, expiresAt);
            if (succeed)
            {
                m_items[datakey.Key] = datakey.Value;
            }

            return succeed;
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            var succeed = m_inner.Store(datakey, validFor);
            if (succeed)
            {
                m_items[datakey.Key] = datakey.Value;
            }

            return succeed;
        }

        public override bool Remove(string key)
        {
            m_items.Remove(key);
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
