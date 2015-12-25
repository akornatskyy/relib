using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Caching
{
    public static class CacheHelper
    {
        #region Cache function Func2<TResult>

        public static string CacheKey<TResult>(Func2<TResult> createFunc)
        {
            return TypeHelper.GetName(createFunc);
        }

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc)
        {
            return Get<TResult>(cache, key, createFunc, null);
        }

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, key, createFunc, 
                datakey => cache.Store(datakey), dependency);
        }

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc, DateTime expiresAt)
        {
            return Get<TResult>(cache, key, createFunc, expiresAt, null);
        }

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc, DateTime expiresAt, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, key, createFunc,
                datakey => cache.Store(datakey, expiresAt), dependency);
        }

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc, TimeSpan validFor)
        {
            return Get<TResult>(cache, key, createFunc, validFor, null);
        }

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc, TimeSpan validFor, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, key, createFunc,
                datakey => cache.Store(datakey, validFor), dependency);
        }

        #endregion

        #region Cache function Func2<T, TResult>

        public static string CacheKey<T, TResult>(Func2<T, TResult> createFunc, T arg)
        {
            return Key.From(createFunc, arg);
        }

        public static TResult Get<T, TResult>(ICache cache, Func2<T, TResult> createFunc, T arg)
        {
            return Get<T, TResult>(cache, createFunc, arg, null);
        }

        public static TResult Get<T, TResult>(ICache cache, Func2<T, TResult> createFunc, T arg, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, CacheKey(createFunc, arg), () => createFunc(arg),
                datakey => cache.Store(datakey), dependency);
        }

        public static TResult Get<T, TResult>(ICache cache, Func2<T, TResult> createFunc, T arg, DateTime expiresAt)
        {
            return Get<T, TResult>(cache, createFunc, arg, expiresAt, null);
        }

        public static TResult Get<T, TResult>(ICache cache, Func2<T, TResult> createFunc, T arg, DateTime expiresAt, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, CacheKey(createFunc, arg), () => createFunc(arg),
                datakey => cache.Store(datakey, expiresAt), dependency);
        }

        public static TResult Get<T, TResult>(ICache cache, Func2<T, TResult> createFunc, T arg, TimeSpan validFor)
        {
            return Get<T, TResult>(cache, createFunc, arg, validFor, null);
        }

        public static TResult Get<T, TResult>(ICache cache, Func2<T, TResult> createFunc, T arg, TimeSpan validFor, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, CacheKey(createFunc, arg), () => createFunc(arg),
                datakey => cache.Store(datakey, validFor), dependency);
        }

        #endregion

        #region Cache function Func2<TA, TB, TResult>

        public static string CacheKey<TA, TB, TResult>(Func2<TA, TB, TResult> createFunc, TA arga, TB argb)
        {
            return Key.From(createFunc, arga, argb);
        }

        public static TResult Get<TA, TB, TResult>(ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb)
        {
            return Get<TA, TB, TResult>(cache, createFunc, arga, argb, null);
        }

        public static TResult Get<TA, TB, TResult>(ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, CacheKey(createFunc, arga, argb), () => createFunc(arga, argb),
                datakey => cache.Store(datakey), dependency);
        }

        public static TResult Get<TA, TB, TResult>(ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, DateTime expiresAt)
        {
            return Get<TA, TB, TResult>(cache, createFunc, arga, argb, expiresAt, null);
        }

        public static TResult Get<TA, TB, TResult>(ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, DateTime expiresAt, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, CacheKey(createFunc, arga, argb), () => createFunc(arga, argb),
                datakey => cache.Store(datakey, expiresAt), dependency);
        }

        public static TResult Get<TA, TB, TResult>(ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, TimeSpan validFor)
        {
            return Get<TA, TB, TResult>(cache, createFunc, arga, argb, validFor, null);
        }

        public static TResult Get<TA, TB, TResult>(ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, TimeSpan validFor, LinkedCacheDependency dependency)
        {
            return Get<TResult>(cache, CacheKey(createFunc, arga, argb), () => createFunc(arga, argb),
                datakey => cache.Store(datakey, validFor), dependency);
        }

        #endregion

        public static TResult Get<TResult>(ICache cache, string key, Func2<TResult> createFunc, 
            Action<DataKey<TResult>> storeFunc, LinkedCacheDependency dependency)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (createFunc == null)
            {
                throw new ArgumentNullException("createFunc");
            }

            if (storeFunc == null)
            {
                throw new ArgumentNullException("storeFunc");
            }

            var datakey = new LazyDataKey<TResult>(key);
            cache.Get(datakey);
            if (!datakey.HasValue)
            {
                datakey.Value = createFunc();
                storeFunc(datakey);
                if (dependency != null)
                {
                    dependency.Add(key);
                }
            }

            return datakey.Value;
        }
    }
}
