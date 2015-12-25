using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Caching
{
    public static class DefaultCache
    {
        private static ICache g_cache;

        public static ICache Instance
        {
            [DebuggerStepThrough]
            get { return g_cache; }
        }

        [DebuggerStepThrough]
        public static void InitializeWith(ICache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            if (g_cache != null)
            {
                throw new InvalidOperationException(Properties.Resources.ErrorCacheInitializedAlready);
            }

            g_cache = cache;
        }

        [DebuggerStepThrough]
        public static void Clear()
        {
            g_cache.Clear();
        }

        [DebuggerStepThrough]
        public static T Get<T>(string key)
        {
            return g_cache.Get<T>(key);
        }

        [DebuggerStepThrough]
        public static void Get<T>(DataKey<T> datakey)
        {
            g_cache.Get(datakey);
        }

        [DebuggerStepThrough]
        public static void Store<T>(string key, T value)
        {
            g_cache.Store(key, value);
        }

        [DebuggerStepThrough]
        public static void Store<T>(string key, T value, DateTime expiresAt)
        {
            g_cache.Store(key, value, expiresAt);
        }

        [DebuggerStepThrough]
        public static void Store<T>(string key, T value, TimeSpan validFor)
        {
            g_cache.Store(key, value, validFor);
        }

        [DebuggerStepThrough]
        public static void Store<T>(DataKey<T> datakey)
        {
            g_cache.Store(datakey);
        }

        [DebuggerStepThrough]
        public static void Store<T>(DataKey<T> datakey, DateTime expiresAt)
        {
            g_cache.Store(datakey, expiresAt);
        }

        [DebuggerStepThrough]
        public static void Store<T>(DataKey<T> datakey, TimeSpan validFor)
        {
            g_cache.Store(datakey, validFor);
        }

        [DebuggerStepThrough]
        public static void Remove(string key)
        {
            g_cache.Remove(key);
        }

        [DebuggerStepThrough]
        public static void Reset()
        {
            if (g_cache != null)
            {
                Disposable.ReleaseFactory(g_cache);
                g_cache = null;
            }
        }
    }
}
