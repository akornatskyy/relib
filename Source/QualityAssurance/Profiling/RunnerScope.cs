using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public class RunnerScope<TPayload> : Disposable, IRunnerScope<TPayload>
    {
        private readonly Func<Action<TPayload>> m_runnerFactory;

        public RunnerScope(Func<Action<TPayload>> runnerFactory)
        {
            m_runnerFactory = runnerFactory;
        }

        #region IRunnerLifetime<T> Members

        public Action<TPayload> CreateRunner()
        {
            return m_runnerFactory();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
        }
    }
}
