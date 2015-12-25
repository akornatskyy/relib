using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class DictionaryExtensions
    {
        [DebuggerStepThrough]
        public static bool TryGetValue<TValue>(this IDictionary dictionary, object key, out TValue value)
        {
            return DictionaryHelper.TryGetValue<TValue>(dictionary, key, out value);
        }

        [DebuggerStepThrough]
        public static TValue GetValue<TValue>(this IDictionary dictionary, object key, TValue defaultValue)
        {
            return DictionaryHelper.GetValue<TValue>(dictionary, key, defaultValue);
        }

        [DebuggerStepThrough]
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return DictionaryHelper.GetValue<TKey, TValue>(dictionary, key, defaultValue);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TValue> UniqueueValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return DictionaryHelper.UniqueueValues<TKey, TValue>(dictionary);
        }
    }
}
