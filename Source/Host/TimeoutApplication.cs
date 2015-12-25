using System;
using System.Collections.Generic;
using System.Threading;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Host
{
    public sealed class TimeoutApplication : SingletonApplication
    {
        private readonly TimeSpan m_upTime;

        public TimeoutApplication(string name, int upTimeInSeconds)
            : this(name, TimeSpan.FromSeconds(upTimeInSeconds))
        {
            m_upTime = TimeSpan.FromSeconds(upTimeInSeconds);
        }

        public TimeoutApplication(string name, TimeSpan upTime)
            : base(name)
        {
            m_upTime = upTime;
        }

        #region SingletonApplication overrides

        protected override void RunCore()
        {
            base.RunCore();

            var waitHandles = new List<EventWaitHandle>();
            foreach (var task in DependencyResolver.ResolveAll<IBackgroundTask>())
            {
                waitHandles.Add(task.WaitHandle);
            }

            var timedOut = waitHandles.Count == 0 || EventWaitHandle.WaitAll(waitHandles.ToArray(), m_upTime);
            if (!timedOut && TraceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(TraceInfo, "Run out of the permitted time");
            }
        }

        #endregion
    }
}
