using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class EnumerableHelper
    {
        [DebuggerStepThrough]
        public static int Count<T>(IEnumerable<T> items)
        {
            ICollection<T> c = items as ICollection<T>;
            if (c != null)
            {
                return c.Count;
            }

            int count = 0;
            ForEach<T>(items, (ignore) => count++);
            return count;
        }

        [DebuggerStepThrough]
        public static void ForEach<T>(IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            using (IEnumerator<T> enumerator = items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }

        [DebuggerStepThrough]
        public static void ForEach<T>(IEnumerable<T> items, Action2<int, T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            using (IEnumerator<T> enumerator = items.GetEnumerator())
            {
                int index = 0;
                while (enumerator.MoveNext())
                {
                    action(index++, enumerator.Current);
                }
            }
        }

        [DebuggerStepThrough]
        public static void ForEach<T>(IEnumerable<T> items, Action<T> odd, Action<T> even)
        {
            bool isOdd = true;
            ForEach(items, item =>
            {
                if (isOdd)
                {
                    odd(item);
                }
                else
                {
                    even(item);
                }

                isOdd = !isOdd;
            });
        }

        [DebuggerStepThrough]
        public static bool Contains<T>(IEnumerable<T> items, T value)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            ICollection<T> c = items as ICollection<T>;
            if (c != null)
            {
                return c.Contains(value);
            }

            throw new NotImplementedException();
        }

        [DebuggerStepThrough]
        public static IDictionary<T, TValue> ToDictionary<T, TValue>(IEnumerable<TValue> items)
            where TValue : ValueObject<T>
        {
            return ToDictionary(items, vo => new KeyValuePair<T, TValue>(vo.Key, vo));
        }

        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(IEnumerable<T> items, Func2<T, KeyValuePair<TKey, TValue>> func)
        {
            var result = new Dictionary<TKey, TValue>(Count(items));
            ForEach(items, (item) =>
            {
                var pair = func(item);
                result.Add(pair.Key, pair.Value);
            });
            return result;
        }

        [DebuggerStepThrough]
        public static NameValueCollection ToNameValueCollection<T>(IEnumerable<T> items, Func2<T, KeyValuePair<string, string>> func)
        {
            var result = new NameValueCollection();
            ForEach(items, (item) =>
            {
                var pair = func(item);
                result.Add(pair.Key, pair.Value);
            });
            return result;
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Translate<T, TResult>(IEnumerable<T> items, Func2<T, TResult> func)
        {
            var result = new List<TResult>(Count(items));
            ForEach(items, (item) => result.Add(func(item)));
            return result;
        }
    }
}
