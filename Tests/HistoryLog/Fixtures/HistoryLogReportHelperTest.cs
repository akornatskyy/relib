using System;
using System.Collections.Generic;
using System.Linq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.HistoryLog.Models;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public static class HistoryLogReportHelperTest
    {
        private static readonly HistoryLogSpecification g_spec = new HistoryLogSpecification()
        {
            DateRange = new Range<DateTime>(new DateTime(2011, 10, 1), new DateTime(2011, 10, 3))
        };

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogReportHelper")]
        public static void AddMissing_None()
        {
            // Arrange
            var items = new List<HistoryLogReport>();
            items.AddRange(new[] { new HistoryLogReport(), new HistoryLogReport(), new HistoryLogReport() });
            Assert.Equal(3, items.Count);

            // Act
            var result = HistoryLogReportHelper.AddMissing(g_spec, items);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogReportHelper")]
        public static void AddMissing_Above_Available()
        {
            // Arrange
            var items = new List<HistoryLogReport>();
            Assert.Equal(0, items.Count);

            // Act
            var result = HistoryLogReportHelper.AddMissing(g_spec, items).ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(new DateTime(2011, 10, 3), result[0].Timestamp);
            Assert.Equal(new DateTime(2011, 10, 2), result[1].Timestamp);
            Assert.Equal(new DateTime(2011, 10, 1), result[2].Timestamp);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogReportHelper")]
        public static void AddMissing_Below_Available()
        {
            // Arrange
            var items = new List<HistoryLogReport>();
            items.AddRange(new[] 
            { 
                new HistoryLogReport() 
                {
                    Timestamp = new DateTime(2011, 10, 2)
                }, 
                new HistoryLogReport() 
                {
                    Timestamp = new DateTime(2011, 10, 1)
                }
            });
            Assert.Equal(2, items.Count);

            // Act
            var result = HistoryLogReportHelper.AddMissing(g_spec, items).ToList();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal(new DateTime(2011, 10, 3), result[0].Timestamp);
        }
    }
}
