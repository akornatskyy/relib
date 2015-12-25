using System;
using System.Threading;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class SynchronizationContextDelegateInvokeStrategy : IDelegateInvokeStrategy
    {
        private delegate object InvokeStrategy(Delegate @delegate, object[] args);

        private readonly InvokeStrategy m_strategy;
        private readonly SynchronizationContext m_context;

        public SynchronizationContextDelegateInvokeStrategy(bool synchronous)
        {
            m_context = SynchronizationContext.Current;

            if (m_context == null)
            {
                m_strategy = Direct;
            }
            else
            {
                if (synchronous)
                {
                    m_strategy = Send;
                }
                else
                {
                    m_strategy = Post;
                }
            }
        }

        #region IDelegateInvokeStrategy Members

        public object Invoke(Delegate @delegate, params object[] args)
        {
            return m_strategy(@delegate, args);
        }

        #endregion

        public object Send(Delegate @delegate, object[] args)
        {
            m_context.Send(state =>
            {
                var p = (Pair<Delegate, object[]>)state;
                p.First.DynamicInvoke(p.Second);
            }, new Pair<Delegate, object[]>(@delegate, args));
            return null;
        }

        public object Post(Delegate @delegate, object[] args)
        {
            m_context.Post(state =>
            {
                var p = (Pair<Delegate, object[]>)state;
                p.First.DynamicInvoke(p.Second);
            }, new Pair<Delegate, object[]>(@delegate, args));
            return null;
        }

        public object Direct(Delegate @delegate, object[] args)
        {
            return @delegate.DynamicInvoke(args);
        }
    }
}
