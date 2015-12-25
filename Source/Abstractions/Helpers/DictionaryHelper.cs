using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class DictionaryHelper
    {
        [DebuggerStepThrough]
        public static bool TryGetValue<TValue>(IDictionary dictionary, object key, out TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            if (!dictionary.Contains(key))
            {
                value = default(TValue);
                return false;
            }

            var res = dictionary[key];
            if (!(res is TValue))
            {
                value = default(TValue);
                return false;
            }

            value = (TValue)res;
            return true;
        }

        [DebuggerStepThrough]
        public static TValue GetValue<TValue>(IDictionary dictionary, object key, TValue defaultValue)
        {
            TValue value;
            if (!TryGetValue<TValue>(dictionary, key, out value))
            {
                return defaultValue;
            }

            return value;
        }

        [DebuggerStepThrough]
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return defaultValue;
            }

            return value;
        }

        [DebuggerStepThrough]
        public static IEnumerable<TValue> UniqueueValues<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            var uniqueue = new List<TValue>();
            foreach (var item in dictionary.Values)
            {
                if (!uniqueue.Contains(item))
                {
                    uniqueue.Add(item);
                }
            }

            return uniqueue;
        }
    }
}
