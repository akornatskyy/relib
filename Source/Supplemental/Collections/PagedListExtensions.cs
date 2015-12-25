using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class PagedListExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this IPagedList<T> pagedList)
        {
            return PagedListHelper.IsNullOrEmpty(pagedList);
        }
    }
}
