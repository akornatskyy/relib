using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
        {
            ListHelper.AddRange(list, collection);
        }
    }
}
