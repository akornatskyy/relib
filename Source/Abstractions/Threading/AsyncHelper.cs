using System;
using System.Threading;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Threading
{
    public static class AsyncHelper
    {
        public static void FireAndForget<T>(Action<T> action, T arg)
        {
            FireAndForget((Delegate)action, arg);
        }

        public static void FireAndForget<TA, TB>(Action2<TA, TB> action, TA a, TB b)
        {
            FireAndForget((Delegate)action, a, b);
        }

        public static void FireAndForget(Delegate d, params object[] args)
        {
            ThreadPool.QueueUserWorkItem(state => 
            { 
                var targetInfo = (Pair<Delegate, object[]>)state;
                targetInfo.First.DynamicInvoke(targetInfo.Second);
            }, new Pair<Delegate, object[]>(d, args));
        }
    }
}
