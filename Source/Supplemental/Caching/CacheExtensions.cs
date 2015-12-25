using System;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.Caching
{
    public static class CacheExtensions
    {
        #region Cache function Func2<TResult>

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc);
        }

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc, dependency);
        }

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc, DateTime expiresAt)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc, expiresAt);
        }

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc, DateTime expiresAt, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc, expiresAt, dependency);
        }

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc, TimeSpan validFor)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc, validFor);
        }

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc, TimeSpan validFor, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc, validFor, dependency);
        }

        #endregion

        #region Cache function Func2<T, TResult>

        public static TResult Get<T, TResult>(this ICache cache, Func2<T, TResult> createFunc, T arg)
        {
            return CacheHelper.Get<T, TResult>(cache, createFunc, arg);
        }

        public static TResult Get<T, TResult>(this ICache cache, Func2<T, TResult> createFunc, T arg, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<T, TResult>(cache, createFunc, arg, dependency);
        }

        public static TResult Get<T, TResult>(this ICache cache, Func2<T, TResult> createFunc, T arg, DateTime expiresAt)
        {
            return CacheHelper.Get<T, TResult>(cache, createFunc, arg, expiresAt);
        }

        public static TResult Get<T, TResult>(this ICache cache, Func2<T, TResult> createFunc, T arg, DateTime expiresAt, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<T, TResult>(cache, createFunc, arg, expiresAt, dependency);
        }

        public static TResult Get<T, TResult>(this ICache cache, Func2<T, TResult> createFunc, T arg, TimeSpan validFor)
        {
            return CacheHelper.Get<T, TResult>(cache, createFunc, arg, validFor);
        }

        public static TResult Get<T, TResult>(this ICache cache, Func2<T, TResult> createFunc, T arg, TimeSpan validFor, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<T, TResult>(cache, createFunc, arg, validFor, dependency);
        }

        #endregion

        #region Cache function Func2<TA, TB, TResult>

        public static TResult Get<TA, TB, TResult>(this ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb)
        {
            return CacheHelper.Get<TA, TB, TResult>(cache, createFunc, arga, argb);
        }

        public static TResult Get<TA, TB, TResult>(this ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TA, TB, TResult>(cache, createFunc, arga, argb, dependency);
        }

        public static TResult Get<TA, TB, TResult>(this ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, DateTime expiresAt)
        {
            return CacheHelper.Get<TA, TB, TResult>(cache, createFunc, arga, argb, expiresAt);
        }

        public static TResult Get<TA, TB, TResult>(this ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, DateTime expiresAt, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TA, TB, TResult>(cache, createFunc, arga, argb, expiresAt, dependency);
        }

        public static TResult Get<TA, TB, TResult>(this ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, TimeSpan validFor)
        {
            return CacheHelper.Get<TA, TB, TResult>(cache, createFunc, arga, argb, validFor);
        }

        public static TResult Get<TA, TB, TResult>(this ICache cache, Func2<TA, TB, TResult> createFunc, TA arga, TB argb, TimeSpan validFor, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TA, TB, TResult>(cache, createFunc, arga, argb, validFor, dependency);
        }

        #endregion

        public static TResult Get<TResult>(this ICache cache, string key, Func2<TResult> createFunc, Action<DataKey<TResult>> storeFunc, LinkedCacheDependency dependency)
        {
            return CacheHelper.Get<TResult>(cache, key, createFunc, storeFunc, dependency);
        }
    }
}
