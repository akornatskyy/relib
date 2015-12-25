using System;
using ReusableLibrary.Supplemental.System;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace ReusableLibrary.WatiN
{
    public static class FindHelper
    {
        public static AttributeConstraint ByPartialUrl(string partial)
        {
            return Find.ByUrl(url => !String.IsNullOrEmpty(url) && url.Contains(partial, StringComparison.OrdinalIgnoreCase));
        }
    }
}
