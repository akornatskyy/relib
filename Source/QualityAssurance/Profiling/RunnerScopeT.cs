using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.QualityAssurance.Profiling
{
    public class RunnerScope<TContext, TPayload> : Disposable, IRunnerScope<TPayload>
        where TContext : IDisposable, new()
    {
        private readonly Func<TContext, Action<TPayload>> m_runnerFactory;
        private TContext m_context;

        public RunnerScope(Func<TContext, Action<TPayload>> runnerFactory)
        {
            m_runnerFactory = runnerFactory;
        }

        #region IRunnerLifetime<T> Members

        public Action<TPayload> CreateRunner()
        {
            m_context = new TContext();
            return m_runnerFactory(m_context);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_context.Dispose();
                m_context = default(TContext);
            }
        }
    }
}
