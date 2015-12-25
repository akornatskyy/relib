using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.WorkItem
{
    public abstract class SequencialWorkItem<TStateBag> : IWorkItem, IParameterizedWorkItem<TStateBag>
        where TStateBag : new()
    {
        protected SequencialWorkItem()
            : this(null)
        {
        }

        protected SequencialWorkItem(string name)
        {
            StateBag = new TStateBag();
            TraceInfo = new TraceInfo(new TraceSource(name ?? GetType().Name));
        }

        public TStateBag StateBag { get; set; }

        public IExceptionHandler ExceptionHandler { get; set; }

        #region IParameterizedWorkItem<TStateBag> Members

        public bool DoWork(TStateBag payload)
        {
            return DoWork(payload, () => false);
        }

        public bool DoWork(TStateBag payload, Func2<bool> shutdownCallback)
        {
            StateBag = payload;
            return DoWork(shutdownCallback);
        }

        #endregion

        #region IWorkItem Members

        public bool DoWork()
        {
            return DoWork(() => false);
        }

        public virtual bool DoWork(Func2<bool> shutdownCallback)
        {
            try
            {
                if (Rules == null || SatisfyAllRules(shutdownCallback))
                {
                    OnAllRulesSatisfied();
                    if (TraceInfo.IsInfoEnabled)
                    {
                        TraceHelper.TraceInfo(TraceInfo, "Succeed");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                if (!HandleException(ex))
                {
                    // Rethrow exception and preserve the full call stack trace
                    throw;
                }
            }

            return false;
        }

        #endregion

        public abstract void OnAllRulesSatisfied();

        public TraceInfo TraceInfo { get; private set; }

        public virtual void ReportError(string message)
        {
            if (TraceInfo.IsErrorEnabled)
            {
                TraceHelper.TraceError(TraceInfo, message);
            }
        }

        protected Func2<bool>[] Rules { get; set; }

        protected virtual bool HandleException(Exception ex)
        {
            return ExceptionHandler != null && ExceptionHandler.HandleException(ex);
        }

        private bool SatisfyAllRules(Func2<bool> shutdownCallback)
        {
            var satisfyAllRules = true;
            foreach (var satisfyRule in Rules)
            {
                if (TraceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(TraceInfo, satisfyRule.Method.Name);
                }

                satisfyAllRules &= satisfyRule() && !shutdownCallback();
                if (!satisfyAllRules)
                {
                    break;
                }
            }

            return satisfyAllRules;
        }
    }
}
