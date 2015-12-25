using System;
using System.Collections.Generic;
using System.Text;

namespace ReusableLibrary.Abstractions.Tracing
{
    public interface IPerformanceCounter
    {
        void Increment();

        void IncrementBy(long value);
    }
}
