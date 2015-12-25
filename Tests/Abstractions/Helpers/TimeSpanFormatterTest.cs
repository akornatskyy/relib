using System;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class TimeSpanHelperTest
    {
        [Theory]
        [InlineData("5.4:3:2.01", "5 Days 4 Hours")]
        [InlineData("5.0:3:2.01", "5 Days")]
        [InlineData("4:3:2.01", "4 Hours 3 Minutes")]
        [InlineData("4:0:2.01", "4 Hours")]
        [InlineData("0:3:2.01", "3 Minutes 2 Seconds")]
        [InlineData("0:3:0.01", "3 Minutes")]
        [InlineData("0:0:2.01", "2 Seconds 10 Milliseconds")]
        [InlineData("0:0:0.01", "10 Milliseconds")]
        [Trait(Constants.TraitNames.Helpers, "TimeSpanHelper")]
        public static void ToLongTimeString(string sample, string expected)
        {
            // Arrange
            var ts = TimeSpan.Parse(sample, CultureInfo.InvariantCulture);

            // Act
            var formatted = TimeSpanHelper.ToLongTimeString(ts);

            // Assert
            Assert.Equal(expected, formatted);
        }

        [Theory]
        [InlineData("5.4:3:2.01", "7443m")]
        [InlineData("4:3:2.01", "243m")]
        [InlineData("0:3:2.01", "3m 2s")]
        [InlineData("0:0:2.01", "2s 10ms")]
        [InlineData("0:0:0.01", "10ms")]
        [InlineData("0:0:0.750", "750ms")]
        [InlineData("0:0:0.000025", "25mcs")]
        [Trait(Constants.TraitNames.Helpers, "TimeSpanHelper")]
        public static void ToShortTimeString(string sample, string expected)
        {
            // Arrange
            var ts = TimeSpan.Parse(sample, CultureInfo.InvariantCulture);

            // Act
            var formatted = TimeSpanHelper.ToShortTimeString(ts);

            // Assert
            Assert.Equal(expected, formatted);
        }
    }
}
