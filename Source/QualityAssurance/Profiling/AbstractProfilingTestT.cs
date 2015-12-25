using System;
using System.Collections.Generic;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public abstract class AbstractProfilingTest<TContext>
        where TContext : IDisposable, new()
    {
        protected AbstractProfilingTest()
        {
            DefaultSamplesCount = 100;
        }

        public int DefaultSamplesCount { get; set; }

        protected static ProfileReport<T> Profile<T>(int threads, Func<TContext, Action<T>> testFactoryMethod, Func<IEnumerable<T>> payloadFactoryMethod, int samplesCount)
        {
            return ProfilerTest.Profile<T>(threads,
                () => new RunnerScope<TContext, T>(context => testFactoryMethod(context)),
                payloadFactoryMethod, samplesCount);
        }

        protected ProfileReport<T> Profile<T>(int threads, Func<TContext, Action<T>> testFactoryMethod, Func<IEnumerable<T>> payloadFactoryMethod)
        {
            return Profile<T>(threads, testFactoryMethod, payloadFactoryMethod, DefaultSamplesCount);
        }
    }
}
