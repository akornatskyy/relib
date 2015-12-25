using System;
using System.Collections.Generic;
using System.Linq;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    internal static class ProfilerTest
    {
        public static ProfileReport<T> Profile<T>(int threads, 
            Func<IRunnerScope<T>> runnerScopeFactory, 
            Func<IEnumerable<T>> payloadFactoryMethod, 
            int samplesCount)
        {
            // Arrange
            var request = new ProfileRequest<T>(runnerScopeFactory)
            {
                Payloads = EnsureSamples(payloadFactoryMethod, samplesCount)
            };
            var profiler = new ProfilerPool(threads);

            // Act
            var results = profiler.Go(request);

            // Assert
            var report = new ProfileReport<T>(profiler.Elapsed, results);
            return report;
        }

        private static T[] EnsureSamples<T>(Func<IEnumerable<T>> payloadFactoryMethod, int samplesCount)
        {
            var samples = payloadFactoryMethod().Take(samplesCount).ToArray();
            if (samples.Length == 0)
            {
                throw new InvalidOperationException("You need supply at least one sample");
            }

            if (samples.Length == samplesCount)
            {
                return samples;
            }

            var list = new List<T>(samplesCount);
            for (int i = 0; i < samplesCount / samples.Length; i++)
            {
                list.AddRange(samples);
            }

            list.AddRange(samples.Take(samplesCount % samples.Length));
            return list.ToArray();
        }
    }
}
