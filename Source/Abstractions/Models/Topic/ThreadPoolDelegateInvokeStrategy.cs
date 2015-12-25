using System;
using ReusableLibrary.Abstractions.Threading;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class ThreadPoolDelegateInvokeStrategy : IDelegateInvokeStrategy
    {
        #region IDelegateInvokeStrategy Members

        public object Invoke(Delegate @delegate, params object[] args)
        {
            AsyncHelper.FireAndForget(@delegate, args);
            return null;
        }

        #endregion
    }
}
