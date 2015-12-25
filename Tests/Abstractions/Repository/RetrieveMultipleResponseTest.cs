using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Repository;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class RetrieveMultipleResponseTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Repository, "RetrieveMultipleResponse")]
        public static void Constructor_Items_AreNull()
        {
            // Arrange
            IList<int> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                var res = new RetrieveMultipleResponse<int>(items, false);
                Assert.Null(res);
            });

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "RetrieveMultipleResponse")]
        public static void Constructor_Items_Empty_But_HasMore()
        {
            // Arrange
            var items = new List<int>();

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var res = new RetrieveMultipleResponse<int>(items, true);
                Assert.Null(res);
            });

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "RetrieveMultipleResponse")]
        public static void Constructor_Items_Empty_Has_No_More()
        {
            // Arrange
            var items = new List<int>();

            // Act
            var res = new RetrieveMultipleResponse<int>(items, false);

            // Assert
            Assert.NotSame(items, res);
            Assert.NotNull(res.Items);
            Assert.Equal(0, items.Count);
            Assert.False(res.HasMore);
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "RetrieveMultipleResponse")]
        public static void Items_Are_Readonly()
        {
            // Arrange
            var items = new List<int>(new[] { 1, 2, 3 });

            // Act
            var res = new RetrieveMultipleResponse<int>(items, true);

            // Assert
            Assert.NotSame(items, res);
            Assert.Equal(3, EnumerableHelper.Count(res.Items));
            Assert.True(res.HasMore);
            Assert.Throws<NotSupportedException>(() => 
            {
                var list = res.Items as IList<int>;
                list.RemoveAt(0);
            });
        }
    }
}
