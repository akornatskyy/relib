using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class CompositePerformanceCounter : IPerformanceCounter
    {
        private readonly PerformanceCounter[] m_counters; 

        public CompositePerformanceCounter(PerformanceCounter[] counters)
        {
            m_counters = counters;
        }

        #region IPerformanceCounter Members

        public void Increment()
        {
            foreach (var counter in m_counters)
            {
                counter.Increment();
            }
        }

        public void IncrementBy(long value)
        {
            foreach (var counter in m_counters)
            {
                counter.IncrementBy(value);
            }
        }

        #endregion
    }
}
