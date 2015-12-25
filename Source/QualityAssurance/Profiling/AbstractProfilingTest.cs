using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public abstract class AbstractProfilingTest : Disposable
    {
        protected AbstractProfilingTest()
        {
            DefaultSamplesCount = 100;
        }

        public int DefaultSamplesCount { get; set; }

        protected static ProfileReport<T> Profile<T>(int threads, Func<Action<T>> testFactoryMethod, Func<IEnumerable<T>> payloadFactoryMethod, int samplesCount)
        {
            return ProfilerTest.Profile<T>(threads,
                () => new RunnerScope<T>(testFactoryMethod),
                payloadFactoryMethod, samplesCount);
        }

        protected static ProfileReport<T> Profile<TContext, T>(int threads, Func<TContext, Action<T>> testFactoryMethod, Func<IEnumerable<T>> payloadFactoryMethod, int samplesCount)
            where TContext : IDisposable, new()
        {
            return ProfilerTest.Profile<T>(threads,
                () => new RunnerScope<TContext, T>(context => testFactoryMethod(context)),
                payloadFactoryMethod, samplesCount);
        }

        protected ProfileReport<T> Profile<T>(int threads, Func<Action<T>> testFactoryMethod, Func<IEnumerable<T>> payloadFactoryMethod)
        {
            return Profile<T>(threads, testFactoryMethod, payloadFactoryMethod, DefaultSamplesCount);
        }

        protected ProfileReport<T> Profile<TContext, T>(int threads, Func<TContext, Action<T>> testFactoryMethod, Func<IEnumerable<T>> payloadFactoryMethod)
            where TContext : IDisposable, new()
        {
            return Profile<TContext, T>(threads, testFactoryMethod, payloadFactoryMethod, DefaultSamplesCount);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
