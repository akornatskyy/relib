using System;
using System.Collections.Generic;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class DateTimeHelperTest
    {
        private static readonly Random g_random = new Random();

        public static IEnumerable<object[]> RandomDateSequence
        {
            get
            {
                return EnumerableHelper.Translate(
                    RandomHelper.NextSequence(g_random, i => RandomHelper.NextDate(g_random, -Int16.MaxValue)),
                    d => new object[] { d });
            }
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void FirstDayOfMonth(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.FirstDayOfMonth(current);

            // Assert
            Assert.Equal(0, result.Millisecond);
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Hour);
            Assert.Equal(1, result.Day);
            Assert.Equal(current.Month, result.Month);
            Assert.Equal(current.Year, result.Year);
            Assert.True(result < current);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void LastDayOfMonth(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.LastDayOfMonth(current);

            // Assert
            Assert.Equal(DateTimeHelper.FirstDayOfMonth(current).AddMonths(1).AddDays(-1).Date, result);
        }

        [Theory]
        [InlineData(31, "3/25/2010 5:20:45.345")]
        [InlineData(30, "4/12/2010 5:23:15.001")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void DaysOfMonth(int expectedDays, string value)
        {
            // Arrange
            var current = DateTime.Parse(value, CultureInfo.InvariantCulture);

            // Act
            var result = DateTimeHelper.DaysOfMonth(current);

            // Assert
            Assert.Equal(expectedDays, result);
        }

        [Theory]
        [InlineData("3/21/2010", "3/25/2010 5:20:45.345", DayOfWeek.Sunday)]
        [InlineData("4/11/2010", "4/12/2010 5:23:15.001", DayOfWeek.Sunday)]
        [InlineData("3/28/2010", "4/1/2010 5:23:15.001", DayOfWeek.Sunday)]
        [InlineData("3/22/2010", "3/25/2010 5:20:45.345", DayOfWeek.Monday)]
        [InlineData("4/12/2010", "4/12/2010 5:23:15.001", DayOfWeek.Monday)]
        [InlineData("3/29/2010", "4/1/2010 5:23:15.001", DayOfWeek.Monday)]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void FirstDayOfWeek(string expected, string value, DayOfWeek firstDay)
        {
            // Arrange
            var current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            var expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            // Act
            var result = DateTimeHelper.FirstDayOfWeek(current, firstDay);

            // Assert
            Assert.Equal(expectedDate, result);
        }

        [Theory]
        [InlineData("3/27/2010", "3/25/2010 5:20:45.345", DayOfWeek.Sunday)]
        [InlineData("4/17/2010", "4/12/2010 5:23:15.001", DayOfWeek.Sunday)]
        [InlineData("4/3/2010", "4/1/2010 5:23:15.001", DayOfWeek.Sunday)]
        [InlineData("3/28/2010", "3/25/2010 5:20:45.345", DayOfWeek.Monday)]
        [InlineData("4/18/2010", "4/12/2010 5:23:15.001", DayOfWeek.Monday)]
        [InlineData("4/4/2010", "4/1/2010 5:23:15.001", DayOfWeek.Monday)]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void LastDayOfWeek(string expected, string value, DayOfWeek firstDay)
        {
            // Arrange
            var current = DateTime.Parse(value, CultureInfo.InvariantCulture);
            var expectedDate = DateTime.Parse(expected, CultureInfo.InvariantCulture);

            // Act
            var result = DateTimeHelper.LastDayOfWeek(current, firstDay);

            // Assert
            Assert.Equal(expectedDate, result);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void DayRange(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.DayRange(current);

            // Assert
            Assert.Equal(current.Date, result.From);
            Assert.Equal(current.Date.AddDays(1).AddMilliseconds(-1), result.To);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void WeekRange(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.WeekRange(current, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(DateTimeHelper.FirstDayOfWeek(current, CultureInfo.InvariantCulture), result.From);
            Assert.Equal(DateTimeHelper.LastDayOfWeek(current, CultureInfo.InvariantCulture).AddDays(1).AddMilliseconds(-1), result.To);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void MonthRange(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.MonthRange(current);

            // Assert
            Assert.Equal(DateTimeHelper.FirstDayOfMonth(current), result.From);
            Assert.Equal(DateTimeHelper.FirstDayOfMonth(current).AddMonths(1).AddMilliseconds(-1), result.To);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void BeforeMidnight(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.BeforeMidnight(current);

            // Assert
            Assert.Equal(current.Date.AddDays(1).AddMilliseconds(-1), result);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void Midnight(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.Midnight(current);

            // Assert
            Assert.Equal(current.Date, result);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void Noon(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.Noon(current);

            // Assert
            Assert.Equal(0, result.Millisecond);
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.Minute);
            Assert.Equal(12, result.Hour);
            Assert.Equal(current.Day, result.Day);
            Assert.Equal(current.Month, result.Month);
            Assert.Equal(current.Year, result.Year);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void SecondsOnly(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.SecondsOnly(current);

            // Assert
            Assert.Equal(0, result.Millisecond);
            Assert.True(result < current);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void MinutesOnly(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.MinutesOnly(current);

            // Assert
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.Millisecond);
            Assert.True(result < current);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void HoursOnly(DateTime current)
        {
            // Arrange

            // Act
            var result = DateTimeHelper.HoursOnly(current);

            // Assert
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);
            Assert.Equal(0, result.Millisecond);
            Assert.True(result < current);
        }

        [Theory]
        [PropertyData("RandomDateSequence")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void Age(DateTime dateOfBirth)
        {
            // Arrange
            var today = DateTime.Today;

            // Act
            var result = DateTimeHelper.Age(dateOfBirth);

            // Assert
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.AddYears(age).Date > today.Date)
            {
                age -= 1;
            }

            Assert.Equal(age, result);
        }

        [Theory]
        [InlineData("2010/06/16")]
        [InlineData("0001/01/01")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void NullSafe(string s)
        {
            // Arrange
            var defaultValue = DateTime.Parse(s, CultureInfo.InvariantCulture);

            // Act
            Assert.Equal(defaultValue, DateTimeHelper.NullSafe(null, defaultValue));
            Assert.Equal(DateTime.MinValue, DateTimeHelper.NullSafe(DateTime.MinValue, defaultValue));
            Assert.Equal(DateTime.Today, DateTimeHelper.NullSafe(DateTime.Today, defaultValue));

            // Assert
        }

        [Theory]
        [InlineData("2010/06/16")]
        [InlineData("0001/01/01")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void NullEmpty(string s)
        {
            // Arrange
            var empty = DateTime.Parse(s, CultureInfo.InvariantCulture);

            // Act
            Assert.Equal(null, DateTimeHelper.NullEmpty(null, empty));
            Assert.Equal(null, DateTimeHelper.NullEmpty(empty, empty));
            Assert.Equal(DateTime.Today, DateTimeHelper.NullEmpty(DateTime.Today, empty));

            // Assert
        }

        [Theory]
        [InlineData("2010/07/19 11:59", "2010/07/19 11:50")]
        [InlineData("2010/07/19 11:00", "2010/07/19 11:00")]
        [InlineData("2010/07/19 11:03", "2010/07/19 11:00")]
        [InlineData("2010/07/19 11:09", "2010/07/19 11:00")]
        [InlineData("2010/07/19 11:31", "2010/07/19 11:30")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void TruncateMinutes(string s, string e)
        {
            // Arrange
            var current = DateTime.Parse(s, CultureInfo.InvariantCulture);
            var expected = DateTime.Parse(e, CultureInfo.InvariantCulture);

            // Act
            var result = DateTimeHelper.TruncateMinutes(current, 10);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2010/07/19 11:59", "2010/07/19 12:00")]
        [InlineData("2010/07/19 11:00", "2010/07/19 11:00")]
        [InlineData("2010/07/19 11:03", "2010/07/19 11:10")]
        [InlineData("2010/07/19 11:09", "2010/07/19 11:10")]
        [InlineData("2010/07/19 11:31", "2010/07/19 11:40")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void CeilingMinutes(string s, string e)
        {
            // Arrange
            var current = DateTime.Parse(s, CultureInfo.InvariantCulture);
            var expected = DateTime.Parse(e, CultureInfo.InvariantCulture);

            // Act
            var result = DateTimeHelper.CeilingMinutes(current, 10);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1288879440, "2010/11/4 16:04")]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void ToUnix(int expected, string s)
        {
            // Arrange
            var current = DateTime.Parse(s, CultureInfo.InvariantCulture);

            // Act
            var result = DateTimeHelper.ToUnix(current);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2010/11/4 16:04", 1288879440)]
        [Trait(Constants.TraitNames.Helpers, "DateTimeHelper")]
        public static void FromUnix(string s, int current)
        {
            // Arrange
            var expected = DateTime.Parse(s, CultureInfo.InvariantCulture).ToUniversalTime();

            // Act
            var result = DateTimeHelper.FromUnix(current);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
