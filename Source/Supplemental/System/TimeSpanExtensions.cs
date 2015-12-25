using System;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Supplemental.System
{
    public static class TimeSpanExtensions
    {
        public static string ToLongTimeString(this TimeSpan ts)
        {
            return TimeSpanHelper.ToLongTimeString(ts);
        }

        public static string ToShortTimeString(this TimeSpan ts)
        {
            return TimeSpanHelper.ToShortTimeString(ts);
        }

        public static int Timeout(this TimeSpan interval)
        {
            return TimeSpanHelper.Timeout(interval);
        }
    }
}
