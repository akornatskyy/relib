using System.Collections.Generic;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class CollectionHelper
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}
