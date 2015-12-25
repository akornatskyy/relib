using System.Diagnostics;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class PagedListHelper
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(IPagedList<T> pagedList)
        {
            return pagedList == null || !pagedList.State.HasItems;
        }
    }
}
