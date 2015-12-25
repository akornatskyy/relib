using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class EnumExtensions
    {
        public static bool HasAll(this Enum source, Enum flags)
        {
            return EnumHelper.HasAll(source, flags);
        }

        public static bool HasAny(this Enum source, Enum flags)
        {
            return EnumHelper.HasAny(source, flags);
        }
    }
}
