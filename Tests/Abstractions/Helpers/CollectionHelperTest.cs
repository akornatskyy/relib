using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class CollectionHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "CollectionHelper")]
        public static void IsNullOrEmpty_Null()
        {
            // Arrange
            ICollection<int> collection = null;

            // Act
            var result = CollectionHelper.IsNullOrEmpty(collection);

            // Assert
            Assert.Null(collection);
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "CollectionHelper")]
        public static void IsNullOrEmpty_Empty()
        {
            // Arrange
            var collection = new List<int>();

            // Act
            var result = CollectionHelper.IsNullOrEmpty(collection);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "CollectionHelper")]
        public static void IsNullOrEmpty_NotEmpty()
        {
            // Arrange
            var collection = new List<int>(new[] { 1, 2, 3 });

            // Act
            var result = CollectionHelper.IsNullOrEmpty(collection);

            // Assert
            Assert.False(result);
        }
    }
}
