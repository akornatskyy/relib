using System;
using System.Diagnostics;
using System.Globalization;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class DateTimeHelper
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [DebuggerStepThrough]
        public static DateTime FirstDayOfMonth(DateTime current)
        {
            return new DateTime(current.Year, current.Month, 1);
        }

        [DebuggerStepThrough]
        public static DateTime LastDayOfMonth(DateTime current)
        {
            return FirstDayOfMonth(current.AddMonths(1)).AddDays(-1).Date;
        }

        [DebuggerStepThrough]
        public static int DaysOfMonth(DateTime current)
        {
            return LastDayOfMonth(current).Day;
        }

        [DebuggerStepThrough]
        public static DateTime FirstDayOfWeek(DateTime current)
        {
            return FirstDayOfWeek(current, null);
        }

        [DebuggerStepThrough]
        public static DateTime FirstDayOfWeek(DateTime current, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);
            return FirstDayOfWeek(current, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        }

        [DebuggerStepThrough]
        public static DateTime FirstDayOfWeek(DateTime current, DayOfWeek firstDay)
        {
            return current.AddDays(firstDay - current.DayOfWeek).Date;
        }

        [DebuggerStepThrough]
        public static DateTime LastDayOfWeek(DateTime current)
        {
            return LastDayOfWeek(current, null);
        }

        [DebuggerStepThrough]
        public static DateTime LastDayOfWeek(DateTime current, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);
            return LastDayOfWeek(current, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        }

        [DebuggerStepThrough]
        public static DateTime LastDayOfWeek(DateTime current, DayOfWeek firstDay)
        {
            return FirstDayOfWeek(current, firstDay).AddDays(6);
        }

        [DebuggerStepThrough]
        public static Range<DateTime> Week(DateTime current)
        {
            return WeekRange(current, null);
        }

        [DebuggerStepThrough]
        public static Range<DateTime> WeekRange(DateTime current, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);
            return WeekRange(current, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        }

        [DebuggerStepThrough]
        public static Range<DateTime> WeekRange(DateTime current, DayOfWeek firstDay)
        {
            const double MillisecondsInWeekInclusive = (7 * 24 * 60 * 60 * 1000) - 1;
            var firstDayOfWeek = FirstDayOfWeek(current, firstDay);
            return new Range<DateTime>(firstDayOfWeek, firstDayOfWeek.AddMilliseconds(MillisecondsInWeekInclusive));
        }

        [DebuggerStepThrough]
        public static Range<DateTime> MonthRange(DateTime current)
        {
            var firstDayOfMonth = FirstDayOfMonth(current);
            return new Range<DateTime>(firstDayOfMonth, firstDayOfMonth.AddMonths(1).AddMilliseconds(-1));
        }

        [DebuggerStepThrough]
        public static Range<DateTime> DayRange(DateTime current)
        {
            return new Range<DateTime>(current.Date, BeforeMidnight(current));
        }

        [DebuggerStepThrough]
        public static DateTime Midnight(DateTime current)
        {
            return current.Date;
        }

        [DebuggerStepThrough]
        public static DateTime BeforeMidnight(DateTime current)
        {
            return current.Date.AddDays(1).AddMilliseconds(-1);
        }

        [DebuggerStepThrough]
        public static DateTime Noon(DateTime current)
        {
            return new DateTime(current.Year, current.Month, current.Day, 12, 0, 0);
        }

        [DebuggerStepThrough]
        public static DateTime SecondsOnly(DateTime current)
        {
            return new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second);
        }

        [DebuggerStepThrough]
        public static DateTime MinutesOnly(DateTime current)
        {
            return new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, 0);
        }

        [DebuggerStepThrough]
        public static DateTime HoursOnly(DateTime current)
        {
            return new DateTime(current.Year, current.Month, current.Day, current.Hour, 0, 0);
        }

        [DebuggerStepThrough]
        public static int Age(DateTime dateOfBirth)
        {
            return Age(dateOfBirth, DateTime.Today);
        }

        [DebuggerStepThrough]
        public static int Age(DateTime dateOfBirth, DateTime current)
        {
            var m1 = dateOfBirth.Month;
            var m2 = current.Month;
            if (m1 < m2 || (m1 == m2 && dateOfBirth.Day <= current.Day))
            {
                return current.Year - dateOfBirth.Year;
            }

            return current.Year - dateOfBirth.Year - 1;
        }

        [DebuggerStepThrough]
        public static DateTime NullSafe(DateTime? target)
        {
            return NullSafe(target, DateTime.MinValue);
        }

        [DebuggerStepThrough]
        public static DateTime NullSafe(DateTime? target, DateTime defaultValue)
        {
            if (target == null)
            {
                return defaultValue;
            }

            return target.Value;
        }

        [DebuggerStepThrough]
        public static DateTime? NullEmpty(DateTime? target)
        {
            return NullEmpty(target, DateTime.MinValue);
        }

        [DebuggerStepThrough]
        public static DateTime? NullEmpty(DateTime? target, DateTime empty)
        {
            if (target == null)
            {
                return null;
            }

            if (target == empty)
            {
                return null;
            }

            return target;
        }

        [DebuggerStepThrough]
        public static DateTime? NullEmpty(DateTime target)
        {
            return NullEmpty(target, DateTime.MinValue);
        }

        [DebuggerStepThrough]
        public static DateTime? NullEmpty(DateTime target, DateTime empty)
        {
            if (target == empty)
            {
                return null;
            }

            return target;
        }

        [DebuggerStepThrough]
        public static DateTime TruncateMinutes(DateTime current, int minutes)
        {
            var m = (current.Minute / minutes) * minutes;
            return new DateTime(current.Year, current.Month, current.Day, current.Hour, m, 0);
        }

        [DebuggerStepThrough]
        public static DateTime CeilingMinutes(DateTime current, int minutes)
        {
            var m = current.Minute - (current.Minute % minutes);
            if (m != current.Minute)
            {
                m += minutes;
            }

            return HoursOnly(current).AddMinutes(m);
        }

        [DebuggerStepThrough]
        public static int ToUnix(DateTime current)
        {
            return (int)((current.ToUniversalTime() - UnixEpoch).Ticks / TimeSpan.TicksPerSecond);
        }

        [DebuggerStepThrough]
        public static DateTime FromUnix(int time)
        {
            return new DateTime((time * TimeSpan.TicksPerSecond) + UnixEpoch.Ticks, DateTimeKind.Utc);
        }
    }
}
