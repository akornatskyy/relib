using System;
using System.Threading;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Caching
{
    public abstract class AbstractCache : Disposable, ICache
    {
        private readonly object m_sync = new object();

        #region ICache Members

        public abstract bool Clear();

        public T Get<T>(string key)
        {
            var datakey = new LazyDataKey<T>(key);
            Get(datakey);
            return datakey.Value;
        }

        public bool Store<T>(string key, T value)
        {
            return Store(new DataKey<T>(key)
            {
                Value = value
            });
        }

        public bool Store<T>(string key, T value, DateTime expiresAt)
        {
            return Store(new DataKey<T>(key)
            {
                Value = value
            }, expiresAt);
        }

        public bool Store<T>(string key, T value, TimeSpan validFor)
        {
            return Store(new DataKey<T>(key)
            {
                Value = value
            }, validFor);
        }

        public long Increment(string key)
        {
            return Increment(key, 1L, 0L);
        }

        public long Increment(string key, DateTime expiresAt)
        {
            return Increment(key, 1L, 0L, expiresAt);
        }

        public long Increment(string key, TimeSpan validFor)
        {
            return Increment(key, 1L, 0L, validFor);
        }

        public long Increment(string key, long delta)
        {
            return Increment(key, delta, 0L);
        }

        public long Increment(string key, long delta, DateTime expiresAt)
        {
            return Increment(key, delta, 0L, expiresAt);
        }

        public long Increment(string key, long delta, TimeSpan validFor)
        {
            return Increment(key, delta, 0L, validFor);
        }

        public long Increment(string key, long delta, long initialValue)
        {
            var datakey = new DataKey<long>(key) 
            { 
                Value = initialValue
            };
            if (!Increment(datakey, delta))
            {
                return -1L;
            }

            return datakey.Value;
        }

        public long Increment(string key, long delta, long initialValue, DateTime expiresAt)
        {
            var datakey = new DataKey<long>(key)
            {
                Value = initialValue
            };
            if (!Increment(datakey, delta, expiresAt))
            {
                return -1L;
            }

            return datakey.Value;
        }

        public long Increment(string key, long delta, long initialValue, TimeSpan validFor)
        {
            var datakey = new DataKey<long>(key) 
            { 
                Value = initialValue
            };
            if (!Increment(datakey, delta, validFor))
            {
                return -1L;
            }

            return datakey.Value;
        }

        public bool Increment(DataKey<long> datakey)
        {
            return Increment(datakey, 1L);
        }

        public bool Increment(DataKey<long> datakey, DateTime expiresAt)
        {
            return Increment(datakey, 1L, expiresAt);
        }
        
        public bool Increment(DataKey<long> datakey, TimeSpan validFor)
        {
            return Increment(datakey, 1L, validFor);
        }

        public virtual bool Increment(DataKey<long> datakey, long delta)
        {
            return Increment(datakey, delta, s => Store(s));
        }

        public virtual bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt)
        {
            return Increment(datakey, delta, s => Store(s, expiresAt));
        }

        public virtual bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor)
        {
            return Increment(datakey, delta, s => Store(s, validFor));
        }

        public abstract bool Get<T>(DataKey<T> datakey);

        public abstract bool Store<T>(DataKey<T> datakey);

        public abstract bool Store<T>(DataKey<T> datakey, DateTime expiresAt);

        public abstract bool Store<T>(DataKey<T> datakey, TimeSpan validFor);

        public abstract bool Remove(string key);

        #endregion

        protected override void Dispose(bool disposing)
        {
        }

        private bool Increment(DataKey<long> datakey, long delta, 
            Func2<DataKey<IncrementDataSlot>, bool> storeStrategy)
        {
            var slot = new DataKey<IncrementDataSlot>(datakey.Key);
            if (!Get(slot))
            {
                return false;
            }

            if (!slot.HasValue)
            {
                lock (m_sync)
                {
                    if (!Get(slot))
                    {
                        return false;
                    }

                    if (!slot.HasValue)
                    {
                        slot.Value = new IncrementDataSlot(datakey.Value);
                        return storeStrategy(slot);
                    }
                }
            }

            datakey.Value = slot.Value.Add(delta);
            return true;
        }

        private class IncrementDataSlot
        {
            private long m_value;

            public IncrementDataSlot(long value)
            {
                m_value = value;
            }

            public long Add(long delta)
            {
                return Interlocked.Add(ref m_value, delta);
            }
        }
    }
}
