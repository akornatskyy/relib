using System;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class ListHelper
    {
        public static void AddRange<T>(IList<T> list, IEnumerable<T> collection)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            List<T> list2 = list as List<T>;
            if (list2 == null)
            {
                throw new NotImplementedException();
            }

            list2.AddRange(collection);
        }
    }
}
