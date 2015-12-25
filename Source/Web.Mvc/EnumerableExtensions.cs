using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Web.Mvc
{
    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static SelectList ToSelectList<TValueObject, TKey>(this IEnumerable<TValueObject> enumerable, TKey selectedKey)
            where TValueObject : ValueObject<TKey>
        {
            return new SelectList(enumerable, "Key", "DisplayName", selectedKey);
        }
    }
}
