using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> items, Action2<int, T> action)
        {
            EnumerableHelper.ForEach<T>(items, action);
        }

        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            EnumerableHelper.ForEach<T>(items, action);
        }

        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> odd, Action<T> even)
        {
            EnumerableHelper.ForEach<T>(items, odd, even);
        }

        [DebuggerStepThrough]
        public static IDictionary<T, TValue> ToDictionary<T, TValue>(this IEnumerable<TValue> items)
            where TValue : ValueObject<T>
        {
            return EnumerableHelper.ToDictionary<T, TValue>(items);
        }

        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(this IEnumerable<T> items, Func2<T, KeyValuePair<TKey, TValue>> func)
        {
            return EnumerableHelper.ToDictionary<T, TKey, TValue>(items, func);
        }

        [DebuggerStepThrough]
        public static NameValueCollection ToNameValueCollection<T>(this IEnumerable<T> items, Func2<T, KeyValuePair<string, string>> func)
        {
            return EnumerableHelper.ToNameValueCollection<T>(items, func);
        }

        [DebuggerStepThrough]
        public static IEnumerable<TResult> Translate<T, TResult>(this IEnumerable<T> items, Func2<T, TResult> func)
        {
            return EnumerableHelper.Translate(items, func);
        }

        [DebuggerStepThrough]
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        [DebuggerStepThrough]
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize, bool hasMore)
        {
            return new PagedList<T>(source, index, pageSize, hasMore);
        }

        [DebuggerStepThrough]
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize, int pageIndexOffset, bool hasMore)
        {
            return new PagedOffsetList<T>(source, index, pageSize, pageIndexOffset, hasMore);
        }

        [DebuggerStepThrough]
        public static IEnumerable<object[]> ToPropertyData<T>(this IEnumerable<T> source)
        {
            return source.Select(s => new object[] { s });
        }
    }
}
