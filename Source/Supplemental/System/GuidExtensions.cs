using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid target)
        {
            return GuidHelper.IsEmpty(target);
        }

        public static string Shrink(this Guid target)
        {
            return GuidHelper.Shrink(target);
        }
    }
}
