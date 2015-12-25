using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class OnePass
    {
        private readonly ICache m_cache;
        private readonly TimeSpan m_period;

        public OnePass(ICache cache, TimeSpan period)
        {
            m_cache = cache;
            m_period = period;
        }

        public bool TryEnter(string key)
        {
            return m_cache.Increment(key, 1L, 1L, m_period) == 1L;
        }

        public bool TryLeave(string key)
        {
            return m_cache.Remove(key);
        }
    }
}
