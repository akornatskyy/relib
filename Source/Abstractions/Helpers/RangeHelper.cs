using System;
using System.Collections.Generic;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class RangeHelper
    {
        [DebuggerStepThrough]
        public static Range<T> ExchangeIfFromGreaterTo<T>(Range<T> range)
            where T : IComparable<T>
        {
            return ExchangeIfFromGreaterTo<T>(range, (IComparer<T>)null);
        }

        [DebuggerStepThrough]
        public static Range<T> ExchangeIfFromGreaterTo<T>(Range<T> range, IComparer<T> comparer)
            where T : IComparable<T>
        {
            return ExchangeIfFromGreaterTo<T>(range, (from, to) => (comparer ?? Comparer<T>.Default).Compare(from, to));
        }

        [DebuggerStepThrough]
        public static Range<T> ExchangeIfFromGreaterTo<T>(Range<T> range, Comparison<T> compare)
        {
            if (compare(range.From, range.To) > 0)
            {
                return new Range<T>(range.To, range.From);
            }

            return range;
        }
    }
}
