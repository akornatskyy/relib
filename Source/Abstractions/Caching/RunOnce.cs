using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class RunOnce
    {
        private readonly ICache m_cache;
        private readonly TimeSpan m_period;

        public RunOnce(ICache cache, TimeSpan period)
        {
            m_cache = cache;
            m_period = period;
        }

        public bool TryEnter(string key)
        {
            return m_cache.Increment(key, 1L, 1L, m_period) == 1L;
        }
    }
}
