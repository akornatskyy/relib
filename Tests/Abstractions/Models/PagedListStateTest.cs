using ReusableLibrary.Abstractions.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class PagedListStateTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void Default_Constructor_Default_Settings()
        {
            // Arrange
            var state = new PagedListState();

            // Act
            var settings = state.Settings;

            // Assert
            Assert.Same(PagingSettings.Default, settings);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void Constructor_Default_Settings()
        {
            // Arrange
            var state = new PagedListState(0, 10, 250, true);

            // Act
            var settings = state.Settings;

            // Assert
            Assert.Same(PagingSettings.Default, settings);
        }

        [Theory]
        [InlineData(1, 10, false)]
        [InlineData(2, 10, true)]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void PageCount(int expected, int totalItemsCount, bool hasMore)
        {
            // Arrange
            var state = new PagedListState(0, 10, totalItemsCount, hasMore);

            // Act
            var actual = state.PageCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void TotalItemCount()
        {
            // Arrange
            var state = new PagedListState(0, 10, 250, false);

            // Act
            var actual = state.TotalItemCount;

            // Assert
            Assert.Equal(250, actual);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void PageIndex()
        {
            // Arrange
            var state = new PagedListState(1, 10, 250, false);

            // Act
            var actual = state.PageIndex;

            // Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void PageSize()
        {
            // Arrange
            var state = new PagedListState(1, 10, 250, false);

            // Act
            var actual = state.PageSize;

            // Assert
            Assert.Equal(10, actual);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void HasMore()
        {
            // Arrange
            var state = new PagedListState(1, 10, 250, false);

            // Act
            var actual = state.HasMore;

            // Assert
            Assert.Equal(false, actual);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void PageNumber()
        {
            // Arrange
            var state = new PagedListState(1, 10, 250, false);

            // Act
            var actual = state.PageNumber;

            // Assert
            Assert.Equal(2, actual);
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 100)]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void HasItems(bool expected, int totalItemsCount)
        {
            // Arrange
            var state = new PagedListState(1, 10, totalItemsCount, false);

            // Act
            var actual = state.HasItems;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void HasPreviousPage(bool expected, int pageIndex)
        {
            // Arrange
            var state = new PagedListState(pageIndex, 10, 250, false);

            // Act
            var actual = state.HasPreviousPage;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, 0, 11)]
        [InlineData(false, 0, 10)]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void HasNextPage(bool expected, int pageIndex, int totalItemsCount)
        {
            // Arrange
            var state = new PagedListState(pageIndex, 10, totalItemsCount, false);

            // Act
            var actual = state.HasNextPage;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void IsFirstPage(bool expected, int pageIndex)
        {
            // Arrange
            var state = new PagedListState(pageIndex, 10, 250, false);

            // Act
            var actual = state.IsFirstPage;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(true, 24)]
        [InlineData(false, 1)]
        [Trait(Constants.TraitNames.Models, "PagedListState")]
        public static void IsLastPage(bool expected, int pageIndex)
        {
            // Arrange
            var state = new PagedListState(pageIndex, 10, 250, false);

            // Act
            var actual = state.IsLastPage;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
