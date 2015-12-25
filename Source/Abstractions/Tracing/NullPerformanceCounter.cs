using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Tracing
{
    public sealed class NullPerformanceCounter : IPerformanceCounter
    {
        #region IPerformanceCounter Members

        public void Increment()
        {
        }

        public void IncrementBy(long value)
        {
        }

        #endregion
    }
}
