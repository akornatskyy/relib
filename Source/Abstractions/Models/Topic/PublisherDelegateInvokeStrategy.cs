using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class PublisherDelegateInvokeStrategy : IDelegateInvokeStrategy
    {
        #region IDelegateInvokeStrategy Members

        public object Invoke(Delegate @delegate, params object[] args)
        {
            return @delegate.DynamicInvoke(args);
        }

        #endregion
    }
}
