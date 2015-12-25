using Moq;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class PagedListHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "PagedListHelper")]
        public static void IsNullOrEmpty_Null()
        {
            // Arrange
            IPagedList<int> list = null;

            // Act
            var result = PagedListHelper.IsNullOrEmpty(list);

            // Assert
            Assert.Null(list);
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "PagedListHelper")]
        public static void IsNullOrEmpty_Empty()
        {
            // Arrange
            var mockList = new Mock<IPagedList<int>>();
            mockList.SetupGet(list => list.State.HasItems).Returns(false);

            // Act
            var result = PagedListHelper.IsNullOrEmpty(mockList.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "PagedListHelper")]
        public static void IsNullOrEmpty_NotEmpty()
        {
            // Arrange
            var mockList = new Mock<IPagedList<int>>();
            mockList.SetupGet(list => list.State.HasItems).Returns(true);

            // Act
            var result = PagedListHelper.IsNullOrEmpty(mockList.Object);

            // Assert
            Assert.False(result);
        }
    }
}
