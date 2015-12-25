using System;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.System
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime current)
        {
            return DateTimeHelper.FirstDayOfMonth(current);
        }

        public static DateTime LastDayOfMonth(this DateTime current)
        {
            return DateTimeHelper.LastDayOfMonth(current);
        }

        public static int DaysOfMonth(this DateTime current)
        {
            return DateTimeHelper.DaysOfMonth(current);
        }

        public static DateTime FirstDayOfWeek(this DateTime current)
        {
            return DateTimeHelper.FirstDayOfWeek(current, null);
        }

        public static DateTime FirstDayOfWeek(this DateTime current, CultureInfo cultureInfo)
        {
            return DateTimeHelper.FirstDayOfWeek(current, cultureInfo);
        }

        public static DateTime FirstDayOfWeek(this DateTime current, DayOfWeek firstDay)
        {
            return DateTimeHelper.FirstDayOfWeek(current, firstDay);
        }

        public static DateTime LastDayOfWeek(this DateTime current)
        {
            return DateTimeHelper.LastDayOfWeek(current, null);
        }

        public static DateTime LastDayOfWeek(this DateTime current, CultureInfo cultureInfo)
        {
            return DateTimeHelper.LastDayOfWeek(current, cultureInfo);
        }

        public static DateTime LastDayOfWeek(this DateTime current, DayOfWeek firstDay)
        {
            return DateTimeHelper.LastDayOfWeek(current, firstDay);
        }

        public static Range<DateTime> DayRange(this DateTime current)
        {
            return DateTimeHelper.DayRange(current);
        }

        public static Range<DateTime> WeekRange(this DateTime current)
        {
            return DateTimeHelper.WeekRange(current, null);
        }

        public static Range<DateTime> WeekRange(this DateTime current, CultureInfo cultureInfo)
        {
            return DateTimeHelper.WeekRange(current, cultureInfo);
        }

        public static Range<DateTime> WeekRange(this DateTime current, DayOfWeek firstDay)
        {
            return DateTimeHelper.WeekRange(current, firstDay);
        }

        public static Range<DateTime> MonthRange(this DateTime current)
        {
            return DateTimeHelper.MonthRange(current);
        }

        public static DateTime BeforeMidnight(this DateTime current)
        {
            return DateTimeHelper.BeforeMidnight(current);
        }

        public static DateTime Midnight(this DateTime current)
        {
            return DateTimeHelper.Midnight(current);
        }

        public static DateTime Noon(this DateTime current)
        {
            return DateTimeHelper.Noon(current);
        }

        public static DateTime SecondsOnly(this DateTime current)
        {
            return DateTimeHelper.SecondsOnly(current);
        }

        public static DateTime MinutesOnly(this DateTime current)
        {
            return DateTimeHelper.MinutesOnly(current);
        }

        public static DateTime HoursOnly(this DateTime current)
        {
            return DateTimeHelper.HoursOnly(current);
        }

        public static int Age(this DateTime dateOfBirth)
        {
            return DateTimeHelper.Age(dateOfBirth);
        }

        public static int Age(this DateTime dateOfBirth, DateTime current)
        {
            return DateTimeHelper.Age(dateOfBirth, current);
        }

        public static DateTime? AsNullable(this DateTime current)
        {
            return current != DateTime.MinValue ? current : (DateTime?)null;
        }

        public static DateTime NullSafe(this DateTime? target)
        {
            return DateTimeHelper.NullSafe(target);
        }

        public static DateTime NullSafe(this DateTime? target, DateTime defaultValue)
        {
            return DateTimeHelper.NullSafe(target, defaultValue);
        }

        public static DateTime? NullEmpty(this DateTime? target)
        {
            return DateTimeHelper.NullEmpty(target);
        }

        public static DateTime? NullEmpty(this DateTime? target, DateTime empty)
        {
            return DateTimeHelper.NullEmpty(target, empty);
        }

        public static DateTime? NullEmpty(this DateTime target)
        {
            return DateTimeHelper.NullEmpty(target);
        }

        public static DateTime? NullEmpty(this DateTime target, DateTime empty)
        {
            return DateTimeHelper.NullEmpty(target, empty);
        }

        public static DateTime TruncateMinutes(this DateTime current, int minutes)
        {
            return DateTimeHelper.TruncateMinutes(current, minutes);
        }

        public static DateTime CeilingMinutes(this DateTime current, int minutes)
        {
            return DateTimeHelper.CeilingMinutes(current, minutes);
        }

        public static int ToUnix(this DateTime current)
        {
            return DateTimeHelper.ToUnix(current);
        }
    }
}
