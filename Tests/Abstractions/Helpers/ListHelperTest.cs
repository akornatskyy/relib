using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public static class ListHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ListHelper")]
        public static void AddRange_List_IsNull()
        {
            // Arrange
            List<int> list = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => ListHelper.AddRange(list, new[] { 1, 2, 3 }));
            Assert.Throws<ArgumentNullException>(() => ListHelper.AddRange(new List<int>(), (IEnumerable<int>)null));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ListHelper")]
        public static void AddRange()
        {
            // Arrange
            List<int> list = new List<int>(new[] { 1, 2, 3 });

            // Act
            ListHelper.AddRange(list, new[] { 4, 5, 6 });

            // Assert
            Assert.Equal(6, list.Count);
            Assert.Contains(4, list);
            Assert.Contains(5, list);
            Assert.Contains(6, list);
        }
    }
}
