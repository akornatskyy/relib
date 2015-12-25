using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.Models
{
    public static class RangeExtensions
    {
        public static Range<T> ExchangeIfFromGreaterTo<T>(this Range<T> range)
            where T : IComparable<T>
        {
            return RangeHelper.ExchangeIfFromGreaterTo<T>(range, (IComparer<T>)null);
        }

        public static Range<T> ExchangeIfFromGreaterTo<T>(this Range<T> range, IComparer<T> comparer)
            where T : IComparable<T>
        {
            return RangeHelper.ExchangeIfFromGreaterTo<T>(range, comparer);
        }

        public static Range<T> ExchangeIfFromGreaterTo<T>(this Range<T> range, Comparison<T> compare)
        {
            return RangeHelper.ExchangeIfFromGreaterTo<T>(range, compare);
        }
    }
}
