using System;
using System.Globalization;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Modules
{
    public sealed class StopwatchModuleTest
    {
        [Theory]
        [InlineData(0.01742, "8/9/2010 21:57:45.120")]
        [Trait(Constants.TraitNames.Modules, "StopwatchModule")]
        public static void Format(double seconds, string current)
        {
            // Arrange

            // Act
            var result = StopwatchModule.Format(seconds, DateTime.Parse(current, CultureInfo.InvariantCulture));

            // Assert
            Assert.Equal("<!-- 17.4 ms (57.4 req/sec); 2010-08-09T21:57:45 -->", result);
        }
    }
}
