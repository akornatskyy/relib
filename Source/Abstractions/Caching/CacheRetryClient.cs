using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Caching
{
    public class CacheRetryClient : AbstractCache
    {
        public CacheRetryClient(ICache inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            Inner = inner;
            Retry = new RetryOptions();
        }

        public RetryOptions Retry { get; set; }

        protected ICache Inner { get; private set; }

        public override bool Clear()
        {
            return RetryHelper.Execute(Retry, () => Inner.Clear()) == 0;
        }

        public override bool Get<T>(DataKey<T> datakey)
        {
            return RetryHelper.Execute(Retry, () => Inner.Get(datakey)) == 0;
        }

        public override bool Store<T>(DataKey<T> datakey)
        {
            return RetryHelper.Execute(Retry, () => Inner.Store(datakey)) == 0;
        }

        public override bool Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            return RetryHelper.Execute(Retry, () => Inner.Store(datakey, expiresAt)) == 0;
        }

        public override bool Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            return RetryHelper.Execute(Retry, () => Inner.Store(datakey, validFor)) == 0;
        }

        public override bool Remove(string key)
        {
            return RetryHelper.Execute(Retry, () => Inner.Remove(key)) == 0;
        }

        public override bool Increment(DataKey<long> datakey, long delta)
        {
            return RetryHelper.Execute(Retry, () => Inner.Increment(datakey, delta)) == 0;
        }

        public override bool Increment(DataKey<long> datakey, long delta, DateTime expiresAt)
        {
            return RetryHelper.Execute(Retry, () => Inner.Increment(datakey, delta, expiresAt)) == 0;
        }

        public override bool Increment(DataKey<long> datakey, long delta, TimeSpan validFor)
        {
            return RetryHelper.Execute(Retry, () => Inner.Increment(datakey, delta, validFor)) == 0;
        }
    }
}
