using System;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public interface IRunnerScope<T> : IDisposable
    {
        Action<T> CreateRunner();
    }
}
