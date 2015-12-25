using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public sealed class Profiler : IProfiler
    {
        public Profiler()
            : this(null)
        {
        }

        public Profiler(string name)
        {
            Name = name ?? "Default";
        }

        #region IBenchmarkRunner Members

        public string Name { get; private set; }

        public TimeSpan Elapsed { get; private set; }

        public IEnumerable<ProfileResult<TPayload>> Go<TPayload>(ProfileRequest<TPayload> request)
        {
            var payloads = request.Payloads;
            var results = new List<ProfileResult<TPayload>>(payloads.Length);
            
            Elapsed = TimeSpan.MinValue;            
            using (var scope = request.CreateRunnerScope())
            {
                var runner = scope.CreateRunner();
                var overall = Stopwatch.StartNew();
                
                Stopwatch stopwatch;
                TPayload payload;
                Exception error;
                for (var sample = 0; sample < payloads.Length; sample++)
                {
                    payload = payloads[sample];
                    stopwatch = Stopwatch.StartNew();
                    
                    try
                    {
                        runner(payload);
                        error = null;
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }

                    stopwatch.Stop();
                    var result = new ProfileResult<TPayload>()
                    {
                        Elapsed = stopwatch.Elapsed,
                        Error = error,
                        Payload = payload
                    };
                    results.Add(result);
                }

                overall.Stop();
                Elapsed = overall.Elapsed;
            }

            return results.AsReadOnly();
        }

        #endregion
    }
}
