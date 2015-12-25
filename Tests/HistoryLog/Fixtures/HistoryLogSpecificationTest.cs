using System;
using System.Linq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.Collections;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public sealed class HistoryLogSpecificationTest
    {
        private readonly HistoryLogSpecification m_spec;
        private readonly HistoryLogItem[] m_items;

        public HistoryLogSpecificationTest()
        {
            m_spec = new HistoryLogSpecification();
            m_items = new[] 
            { 
                new HistoryLogItem() { Timestamp = DateTime.Today, Username = "user1", EventId = 1001 }, 
                new HistoryLogItem() { Timestamp = DateTime.Today.AddDays(-2), Username = "user2", RelatedTo = "x", EventId = 1005 }
            };
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_Username()
        {
            // Arrange
            m_spec.Username = "user1";

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(m_spec.Username, result[0].Username);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_DateRange()
        {
            // Arrange
            m_spec.DateRange = new Range<DateTime>(DateTime.Today.Date, DateTime.Now);

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(DateTime.Today, result[0].Timestamp);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_DateRange_NotInRange()
        {
            // Arrange
            m_spec.DateRange = new Range<DateTime>(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(-3));

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_RelatedTo()
        {
            // Arrange
            m_spec.RelatedTo = "x";

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(m_spec.RelatedTo, result[0].RelatedTo);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_EventId()
        {
            // Arrange
            m_spec.EventId = 1001;

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(m_spec.EventId, result[0].EventId);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_EventRange_From()
        {
            // Arrange
            m_spec.EventRange = new Range<short>(1005, 0);

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(1005, result[0].EventId);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_EventRange_To()
        {
            // Arrange
            m_spec.EventRange = new Range<short>(0, 1001);

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(1001, result[0].EventId);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_EventRange()
        {
            // Arrange
            m_spec.EventRange = new Range<short>(1000, 1010);

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(1001, result[0].EventId);
            Assert.Equal(1005, result[1].EventId);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "HistoryLogSpecification")]
        public void IsSatisfied_Events()
        {
            // Arrange
            m_spec.Events = new short[] { 1000, 1001, 1003 };

            // Act
            var result = (from x in m_items
                          select x).AsQueryable().Satisfy(m_spec).ToArray();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal(1001, result[0].EventId);
        }
    }
}
