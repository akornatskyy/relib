using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class PagingSettingsTest
    {
        private readonly PagingSettings m_settings;

        public PagingSettingsTest()
        {
            m_settings = new PagingSettings()
            {
                PageCount = 5,
                DefaultItemsPerPage = 10,
                PageSizes = new[] { 10, 25, 50 }
            };
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(25)]
        [InlineData(50)]
        [Trait(Constants.TraitNames.Models, "PagingSettings")]
        public static void AdjustPageSize_For_Single_PageSize(int totalItemCount)
        {
            // Arrange
            var settings = new PagingSettings()
            {
                PageCount = 5,
                DefaultItemsPerPage = 10,
                PageSizes = new[] { 10 }
            };

            // Act
            var result = settings.AdjustPageSize(totalItemCount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Length);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagingSettings")]
        public void MaxPageSize()
        {
            // Arrange

            // Act
            var result = m_settings.MaxPageSize;

            // Assert
            Assert.Equal(50, result);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData(-2, 0)]
        [InlineData(2, 2)]
        [Trait(Constants.TraitNames.Models, "PagingSettings")]
        public void EnsurePageIndexInRange(int? pageIndex, int expected)
        {
            // Arrange

            // Act
            var result = m_settings.EnsurePageIndexInRange(pageIndex);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, 10)]
        [InlineData(-2, 10)]
        [InlineData(100, 50)]
        [InlineData(20, 20)]
        [Trait(Constants.TraitNames.Models, "PagingSettings")]
        public void EnsurePageSizeInRange(int? pageSize, int expected)
        {
            // Arrange

            // Act
            var result = m_settings.EnsurePageSizeInRange(pageSize);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 5)]
        [InlineData(0, 10)]
        [InlineData(2, 11)]
        [InlineData(2, 15)]
        [InlineData(2, 25)]
        [InlineData(3, 30)]
        [InlineData(3, 50)]
        [InlineData(3, 100)]
        [Trait(Constants.TraitNames.Models, "PagingSettings")]
        public void AdjustPageSize(int expectedLength, int totalItemCount)
        {
            // Arrange

            // Act
            var result = m_settings.AdjustPageSize(totalItemCount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLength, result.Length);
            EnumerableHelper.ForEach(result, (pageSize) => Assert.True(EnumerableHelper.Contains(m_settings.PageSizes, pageSize)));
        }
    }
}
