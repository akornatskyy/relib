using System;
using System.Globalization;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class TimeSpanHelper
    {
        public static int Timeout(TimeSpan interval)
        {
            var ms = (long)interval.TotalMilliseconds;
            if ((ms < -1L) || (ms > 0x7fffffffL))
            {
                throw new ArgumentOutOfRangeException("interval", Properties.Resources.TimeSpanTimeoutOutOfRange);
            }

            return (int)ms;
        }

        public static string ToLongTimeString(TimeSpan ts)
        {
            if (ts.Days > 0)
            {
                return String.Format(CultureInfo.InvariantCulture, "{0} Days{1}", ts.Days,
                    FormatIfNotZero(" Hours", ts.Hours));
            }
            else
            {
                if (ts.Hours > 0)
                {
                    return String.Format(CultureInfo.InvariantCulture, "{0} Hours{1}", ts.Hours,
                        FormatIfNotZero(" Minutes", ts.Minutes));
                }
                else
                {
                    if (ts.Minutes > 0)
                    {
                        return String.Format(CultureInfo.InvariantCulture, "{0} Minutes{1}", ts.Minutes,
                            FormatIfNotZero(" Seconds", ts.Seconds));
                    }
                    else
                    {
                        if (ts.Seconds > 0)
                        {
                            return String.Format(CultureInfo.InvariantCulture, "{0} Seconds{1}", ts.Seconds,
                                FormatIfNotZero(" Milliseconds", ts.Milliseconds));
                        }
                        else
                        {
                            return String.Concat(ts.Milliseconds.ToString(CultureInfo.InvariantCulture), " Milliseconds");
                        }
                    }
                }
            }
        }

        public static string ToShortTimeString(TimeSpan ts)
        {
            if (Math.Truncate(ts.TotalMinutes) > 0.0)
            {
                return String.Format(
                    CultureInfo.InvariantCulture, "{0}m{1}", Convert.ToInt32(ts.TotalMinutes),
                    Convert.ToInt32(ts.TotalHours) > 0 ? string.Empty : FormatIfNotZero("s", ts.Seconds));
            }
            else
            {
                var totalSeconds = Math.Truncate(ts.TotalSeconds);
                if (totalSeconds > 0.0)
                {
                    return String.Format(CultureInfo.InvariantCulture, "{0}s{1}", totalSeconds,
                        FormatIfNotZero("ms", ts.Milliseconds));
                }
                else
                {
                    var totalMilliseconds = ts.Milliseconds;
                    if (totalMilliseconds > 0)
                    {
                        return String.Concat(ts.Milliseconds.ToString(CultureInfo.InvariantCulture), "ms");
                    }
                    else
                    {
                        return String.Concat(((int)(ts.TotalMilliseconds * 1000.0)).ToString(CultureInfo.InvariantCulture), "mcs");
                    }
                }
            }
        }

        private static string FormatIfNotZero(string format, int value)
        {
            if (value == 0)
            {
                return string.Empty;
            }

            return String.Concat(" ", value.ToString(CultureInfo.InvariantCulture), format);
        }
    }
}
