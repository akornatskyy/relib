using System;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class EnumHelper
    {
        public static bool HasAll(Enum source, Enum flags)
        {
            var sourceValue = Convert.ToInt64(source, CultureInfo.InvariantCulture);
            var flagValue = Convert.ToInt64(flags, CultureInfo.InvariantCulture);

            return (sourceValue & flagValue) == flagValue;
        }

        public static bool HasAny(Enum source, Enum flags)
        {
            var sourceValue = Convert.ToInt64(source, CultureInfo.InvariantCulture);
            var flagValue = Convert.ToInt64(flags, CultureInfo.InvariantCulture);

            return (sourceValue & flagValue) != 0;
        }
    }
}
