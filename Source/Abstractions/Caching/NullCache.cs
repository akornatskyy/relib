using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class NullCache : ICache
    {
        #region ICache Members

        public bool Clear()
        {
            return true;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool Get<T>(DataKey<T> datakey)
        {
            return true;
        }

        public bool Store<T>(string key, T value)
        {
            return true;
        }

        public bool Store<T>(string key, T value, DateTime expiresAt)
        {
            return true;
        }

        public bool Store<T>(string key, T value, TimeSpan validFor)
        {
            return true;
        }

        public bool Store<T>(DataKey<T> datakey)
        {
            return true;
        }

        public bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            return true;
        }

        public bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            return true;
        }

        public bool Remove(string key)
        {
            return true;
        }

        public long Increment(string key)
        {
            return 0L;
        }

        public long Increment(string key, DateTime expiresAt)
        {
            return 0L;
        }

        public long Increment(string key, TimeSpan validFor)
        {
            return 0L;
        }

        public long Increment(string key, long delta)
        {
            return 0L;
        }

        public long Increment(string key, long delta, DateTime expiresAt)
        {
            return 0L;
        }

        public long Increment(string key, long delta, TimeSpan validFor)
        {
            return 0L;
        }
        
        public long Increment(string key, long delta, long initialValue)
        {
            return 0L;
        }

        public long Increment(string key, long delta, long initialValue, DateTime expiresAt)
        {
            return 0L;
        }

        public long Increment(string key, long delta, long initialValue, TimeSpan validFor)
        {
            return 0L;
        }

        public bool Increment(DataKey<long> datakey)
        {
            return true;
        }

        public bool Increment(DataKey<long> datakey, DateTime expiresAt)
        {
            return true;
        }

        public bool Increment(DataKey<long> datakey, TimeSpan validFor)
        {
            return true;
        }

        public bool Increment(DataKey<long> datakey, long delta)
        {
            return true;
        }

        public bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt)
        {
            return true;
        }

        public bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor)
        {
            return true;
        }

        #endregion
    }
}
