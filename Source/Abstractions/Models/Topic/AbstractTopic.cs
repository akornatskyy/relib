using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Threading;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Models
{
    public abstract class AbstractTopic : Disposable
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromMinutes(5);
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceTopic));
        
        private readonly List<Pair<Delegate, IDelegateInvokeStrategy>> m_subscribers;
        private readonly ReaderWriterLockScope m_lockScope = new ReaderWriterLockScope(new ReaderWriterLock());

        protected AbstractTopic(string name)
        {
            Name = name;
            m_subscribers = new List<Pair<Delegate, IDelegateInvokeStrategy>>();
            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Created", Name);
            }
        }

        public string Name { get; private set; }

        public int Length 
        { 
            get 
            {
                using (var @lock = m_lockScope.Reader(Timeout))
                {
                    if (!@lock.Aquired)
                    {
                        return -1;
                    }

                    return m_subscribers.Count;
                }
            }
        }

        public void Close()
        {
            using (var @lock = m_lockScope.Writer(Timeout))
            {
                if (!@lock.Aquired)
                {
                    throw new TimeoutException("Close has timed out");
                }

                m_subscribers.Clear();
            }

            if (g_traceInfo.IsVerboseEnabled)
            {
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Closed", Name);
            }
        }

        protected void InnerSubscribe(Delegate subscriber)
        {
            InnerSubscribe(subscriber, DelegateInvokeStrategy.Publisher);
        }

        protected void InnerSubscribe(Delegate subscriber, IDelegateInvokeStrategy strategy)
        {
            using (var @lock = m_lockScope.Writer(Timeout))
            {
                if (!@lock.Aquired)
                {
                    throw new TimeoutException("Subscribe has timed out");
                }

                m_subscribers.Add(new Pair<Delegate, IDelegateInvokeStrategy>(subscriber, strategy));
            }

            if (g_traceInfo.IsVerboseEnabled)
            {
                var subscriberName = GetSubscriberName(subscriber);
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Added '{1}' subscription for '{2}'",
                    Name, subscriberName.Second, subscriberName.First);
            }
        }

        protected void InnerUnsubscribe(Delegate subscriber)
        {
            using (var @lock = m_lockScope.Writer(Timeout))
            {
                if (!@lock.Aquired)
                {
                    throw new TimeoutException("Unsubscribe has timed out");
                }

                m_subscribers.RemoveAll(d => d.First == subscriber);
            }

            if (g_traceInfo.IsVerboseEnabled)
            {
                var subscriberName = GetSubscriberName(subscriber);
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Removed '{1}' subscription for '{2}'",
                    Name, subscriberName.Second, subscriberName.First);
            }
        }

        protected void InnerPublish(params object[] args)
        {
            if (g_traceInfo.IsVerboseEnabled)
            {
                var publisherName = GetPublisherName(GetType());
                TraceHelper.TraceVerbose(g_traceInfo, "{0} - Publishing by '{1}' of '{2}'",
                            Name, publisherName.Second, publisherName.First);
            }

            using (var @lock = m_lockScope.Reader(Timeout))
            {
                if (!@lock.Aquired)
                {
                    throw new TimeoutException("Unsubscribe has timed out");
                }

                foreach (var pair in m_subscribers)
                {
                    var subscriber = pair.First;
                    if (g_traceInfo.IsVerboseEnabled)
                    {
                        var subscriberName = GetSubscriberName(subscriber);
                        TraceHelper.TraceVerbose(g_traceInfo, "{0} - Invoking '{1}' for '{2}'",
                            Name, subscriberName.Second, subscriberName.First);
                    }
                    
                    var strategy = pair.Second;
                    strategy.Invoke(subscriber, args);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        private static Pair<string> GetSubscriberName(Delegate subscriber)
        {
            return new Pair<string>(
                subscriber.Target != null ? TypeHelper.GetName(subscriber.Target.GetType()) : "anonymous",
                subscriber.Method.Name);
        }

        private static Pair<string> GetPublisherName(Type type)
        {
            var result = new Pair<string>("Unknown", "Unknown");
            var trace = new StackTrace(false);
            for (int i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                var method = frame.GetMethod();
                var declaringType = method.DeclaringType;
                if (!declaringType.IsAssignableFrom(type))
                {
                    result = new Pair<string>(method.DeclaringType.Name, method.Name);
                    break;
                }
            }

            return result;
        }
    }
}
