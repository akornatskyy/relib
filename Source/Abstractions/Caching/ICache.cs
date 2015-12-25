using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public interface ICache
    {
        bool Clear();

        T Get<T>(string key);

        bool Get<T>(DataKey<T> datakey);

        bool Store<T>(string key, T value);

        bool Store<T>(string key, T value, DateTime expiresAt);

        bool Store<T>(string key, T value, TimeSpan validFor);

        bool Store<T>(DataKey<T> datakey);

        bool Store<T>(DataKey<T> datakey, DateTime expiresAt);

        bool Store<T>(DataKey<T> datakey, TimeSpan validFor);

        bool Remove(string key);

        long Increment(string key);

        long Increment(string key, DateTime expiresAt);

        long Increment(string key, TimeSpan validFor);

        long Increment(string key, long delta);

        long Increment(string key, long delta, DateTime expiresAt);

        long Increment(string key, long delta, TimeSpan validFor);

        long Increment(string key, long delta, long initialValue);

        long Increment(string key, long delta, long initialValue, DateTime expiresAt);

        long Increment(string key, long delta, long initialValue, TimeSpan validFor);

        bool Increment(DataKey<long> datakey);

        bool Increment(DataKey<long> datakey, DateTime expiresAt);

        bool Increment(DataKey<long> datakey, TimeSpan validFor);

        bool Increment(DataKey<long> datakey, long delta);

        bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt);

        bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor);
    }
}
