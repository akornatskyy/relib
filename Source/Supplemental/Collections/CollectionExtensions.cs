using System.Collections.Generic;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return CollectionHelper.IsNullOrEmpty(collection);
        }
    }
}
