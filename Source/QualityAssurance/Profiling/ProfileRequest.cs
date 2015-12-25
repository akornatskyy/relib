using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public sealed class ProfileRequest<TPayload>
    {
        public ProfileRequest(Func<IRunnerScope<TPayload>> runnerScopeFactory)
        {
            RunnerFactory = runnerScopeFactory;
        }

        public Func<IRunnerScope<TPayload>> RunnerFactory { get; private set; }

        public TPayload[] Payloads { get; set; }

        public IRunnerScope<TPayload> CreateRunnerScope()
        {
            return RunnerFactory();
        }
    }
}
