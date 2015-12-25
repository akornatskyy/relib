using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public sealed class ProfilerPool : IProfiler
    {
        public ProfilerPool(int size)
            : this(null, size)
        {
        }

        public ProfilerPool(string name, int size)
        {
            Name = name ?? "Default";
            PoolSize = size;
        }

        public int PoolSize { get; private set; }

        #region IBenchmarkRunner Members

        public string Name { get; private set; }

        public TimeSpan Elapsed { get; private set; }

        public IEnumerable<ProfileResult<TPayload>> Go<TPayload>(ProfileRequest<TPayload> request)
        {
            var results = new List<ProfileResult<TPayload>>(request.Payloads.Length * PoolSize);
            var workerThreads = new WorkerThread<TPayload>[PoolSize];
            var random = new Random(RandomHelper.Seed());
            for (int i = 0; i < PoolSize; i++)
            {
                var runner = new Profiler(string.Format(CultureInfo.InvariantCulture, "{0}{1}", Name, i));
                var workerRequest = new ProfileRequest<TPayload>(request.RunnerFactory)
                {
                    Payloads = RandomHelper.Shuffle(random, request.Payloads).ToArray()
                };
                workerThreads[i] = new WorkerThread<TPayload>(runner, workerRequest);
            }

            Elapsed = TimeSpan.MinValue;
            var overall = Stopwatch.StartNew();

            EnumerableHelper.ForEach(workerThreads, t => t.Start());
            EnumerableHelper.ForEach(workerThreads, t =>
            {
                if (t.Join(Timeout.Infinite))
                {
                    lock (results)
                    {
                        results.AddRange(t.Results);
                    }
                }
            });

            overall.Stop();
            Elapsed = overall.Elapsed;

            return results.AsReadOnly();
        }

        #endregion

        private class WorkerThread<TPayload>
        {
            private readonly IProfiler m_runner;
            private readonly Thread m_thread;
            private readonly ProfileRequest<TPayload> m_request;

            public WorkerThread(IProfiler runner, ProfileRequest<TPayload> request)
            {
                m_runner = runner;
                m_request = request;
                m_thread = new Thread(new ParameterizedThreadStart(state => DoWork((ProfileRequest<TPayload>)state)))
                {
                    Name = runner.Name
                };
            }

            public IEnumerable<ProfileResult<TPayload>> Results { get; private set; }

            public void Start()
            {
                m_thread.Start(m_request);
            }

            public bool Join(int timeout)
            {
                return m_thread.Join(timeout);
            }

            private void DoWork(ProfileRequest<TPayload> request)
            {
                Results = m_runner.Go(request);
            }
        }
    }
}
