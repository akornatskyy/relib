using System;
using System.Collections;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Threading;

namespace ReusableLibrary.Abstractions.Services
{
    public sealed class TopicCatalog : Disposable, ITopicCatalog
    {
        private static readonly int Timeout = TimeSpanHelper.Timeout(TimeSpan.FromMinutes(5));

        private readonly IDictionary m_catalog = new Hashtable();
        private readonly ReaderWriterLockScope m_lockScope = new ReaderWriterLockScope(new ReaderWriterLock());

        #region ITopicCatalog Members

        public T Get<T>(string name)
             where T : AbstractTopic
        {
            return GetOrCreateTopic<T>(name);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (var @lock = m_lockScope.Writer(Timeout))
                {
                    if (!@lock.Aquired)
                    {
                        throw new TimeoutException();
                    }

                    foreach (AbstractTopic topic in m_catalog.Values)
                    {
                        topic.Dispose();
                    }

                    m_catalog.Clear();
                }
            }
        }

        private T GetOrCreateTopic<T>(string name)
        {
            using (var @rlock = m_lockScope.Reader(Timeout))
            {
                if (!@rlock.Aquired)
                {
                    throw new TimeoutException("Get topic has timed out");
                }

                T value;
                if (!DictionaryHelper.TryGetValue<T>(m_catalog, name, out value))
                {
                    using (var @wlock = m_lockScope.UpgradeToWriter(Timeout))
                    {
                        if (!@wlock.Aquired)
                        {
                            throw new TimeoutException("Create topic has timed out");
                        }

                        value = (T)Activator.CreateInstance(typeof(T), name);
                        m_catalog[name] = value;
                    }
                }

                return value;
            }
        }
    }
}
