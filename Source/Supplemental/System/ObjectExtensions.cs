using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class ObjectExtensions
    {
        [DebuggerStepThrough]
        public static bool In(this object obj, IEnumerable list)
        {
            foreach (var item in list)
            {
                if ((item != null && item.Equals(obj))
                    || (item == null && obj == null))
                {
                    return true;
                }
            }

            return false;
        }

        [DebuggerStepThrough]
        public static NameValueCollection PropertiesToNameValueCollection(this object model)
        {
            return ObjectHelper.PropertiesToNameValueCollection(model);
        }
    }
}
