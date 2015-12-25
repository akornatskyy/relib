using System;
using System.Globalization;
using System.Linq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.Collections;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public static class HistoryLogKeyHelperTest
    {
        [Theory]
        [InlineData("relib:hl:d", null)]
        [InlineData("relib:hl:d", "")]
        [InlineData("relib:hl:d:user1", "user1")]
        [Trait(Constants.TraitNames.Models, "HistoryLogKeyHelper")]
        public static void Dependency(string expected, string username)
        {
            // Arrange
            
            // Act
            var result = HistoryLogKeyHelper.Dependency(username);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("relib:hl:s::201102130000:201102140000:::", null)]
        [InlineData("relib:hl:s::201102130000:201102140000:::", "")]
        [InlineData("relib:hl:s:user1:201102130000:201102140000:::", "user1")]
        [Trait(Constants.TraitNames.Models, "HistoryLogKeyHelper")]
        public static void Specification_Username(string expected, string username)
        {
            // Arrange
            var spec = new HistoryLogSpecification()
            {
                Username = username,
                DateRange = new Range<DateTime>(new DateTime(2011, 02, 13), new DateTime(2011, 02, 14))
            };

            // Act
            var result = HistoryLogKeyHelper.Specification(spec);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("relib:hl:s::201001111215:201102132359:::", "2010/01/11 12:15", "2011/02/13 23:59")]
        [InlineData("relib:hl:s::201001111215:201102132359:::", "2010/01/11 12:15:37", "2011/02/13 23:59:45")]
        [Trait(Constants.TraitNames.Models, "HistoryLogKeyHelper")]
        public static void Specification_DateRange(string expected, string from, string to)
        {
            // Arrange
            var spec = new HistoryLogSpecification()
            {
                DateRange = new Range<DateTime>(DateTime.Parse(from, CultureInfo.InvariantCulture), DateTime.Parse(to, CultureInfo.InvariantCulture))
            };

            // Act
            var result = HistoryLogKeyHelper.Specification(spec);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("relib:hl:s::201102130000:201102140000:::", 0)]
        [InlineData("relib:hl:s::201102130000:201102140000:1001::", 1001)]
        [Trait(Constants.TraitNames.Models, "HistoryLogKeyHelper")]
        public static void Specification_EventId(string expected, int eventid)
        {
            // Arrange
            var spec = new HistoryLogSpecification()
            {
                DateRange = new Range<DateTime>(new DateTime(2011, 02, 13), new DateTime(2011, 02, 14)),
                EventId = (short)eventid
            };

            // Act
            var result = HistoryLogKeyHelper.Specification(spec);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("relib:hl:s::201102130000:201102140000::2345:", "2345")]
        [InlineData("relib:hl:s::201102130000:201102140000::1001,2345,5277:", "1001;2345;5277")]
        [Trait(Constants.TraitNames.Models, "HistoryLogKeyHelper")]
        public static void Specification_Events(string expected, string events)
        {
            // Arrange
            var spec = new HistoryLogSpecification()
            {
                DateRange = new Range<DateTime>(new DateTime(2011, 02, 13), new DateTime(2011, 02, 14)),
                Events = events.Split(';').Translate(s => Int16.Parse(s, CultureInfo.InvariantCulture)).ToArray()
            };

            // Act
            var result = HistoryLogKeyHelper.Specification(spec);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("relib:hl:s::201102130000:201102140000:::", null)]
        [InlineData("relib:hl:s::201102130000:201102140000:::", "")]
        [InlineData("relib:hl:s::201102130000:201102140000:::zzz", "zzz")]
        [Trait(Constants.TraitNames.Models, "HistoryLogKeyHelper")]
        public static void Specification_RelatedTo(string expected, string related)
        {
            // Arrange
            var spec = new HistoryLogSpecification()
            {
                RelatedTo = related,
                DateRange = new Range<DateTime>(new DateTime(2011, 02, 13), new DateTime(2011, 02, 14))
            };

            // Act
            var result = HistoryLogKeyHelper.Specification(spec);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
