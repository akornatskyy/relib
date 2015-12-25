using ReusableLibrary.Abstractions.Caching;

namespace ReusableLibrary.Memcached.Protocol
{
    public interface IProtocol
    {
        bool Store<T>(StoreOperation operation, T datakey, int expires) where T : DataKey;

        bool Get<T>(GetOperation operation, T datakey) where T : DataKey;

        bool GetMany<T>(GetOperation operation, T[] datakeys) where T : DataKey;

        bool Delete(string key);

        bool Increment(DataKey<long> datakey, long delta, int expires);
    }
}
