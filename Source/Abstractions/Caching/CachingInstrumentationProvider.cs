using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class CachingInstrumentationProvider : ICachingInstrumentationProvider
    {
        private readonly IPerformanceCounter m_failedCounter;
        private readonly IPerformanceCounter m_hitsCounter;
        private readonly IPerformanceCounter m_missesCounter;
        private readonly IPerformanceCounter m_storedCounter;
        private readonly IPerformanceCounter m_removedCounter;

        public CachingInstrumentationProvider(string category, string instanceNameSuffix, bool enabled)
        {
            var factory = new PerformanceCounterFactory(category, instanceNameSuffix, enabled);

            m_failedCounter = factory.Create("Total Cache Failed Requests", "Cache Failed Requests/sec");
            m_hitsCounter = factory.Create("Total Cache Hits", "Cache Hits/sec");
            m_missesCounter = factory.Create("Total Cache Misses", "Cache Misses/sec");
            m_storedCounter = factory.Create("Total Cache Store Requests", "Cache Store Requests/sec");
            m_removedCounter = factory.Create("Total Cache Remove Requests", "Cache Remove Requests/sec");
        }

        #region ICachingInstrumentationProvider Members

        public void FireFailed()
        {
            m_failedCounter.Increment();
        }

        public void FireAccessed(bool hit)
        {
            if (hit)
            {
                m_hitsCounter.Increment();
            }
            else
            {
                m_missesCounter.Increment();
            }
        }

        public void FireStored(bool succeed)
        {
            if (succeed)
            {
                m_storedCounter.Increment();
            }
            else
            {
                m_failedCounter.Increment();
            }
        }

        public void FireRemoved(bool succeed)
        {
            if (succeed)
            {
                m_removedCounter.Increment();
            }
            else
            {
                m_failedCounter.Increment();
            }
        }

        #endregion
    }
}
