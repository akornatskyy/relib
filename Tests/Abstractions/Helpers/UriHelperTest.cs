using System;
using System.Collections.Specialized;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class UriHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ToQuery()
        {
            // Arrange
            var items = new NameValueCollection();
            items.Add("x", "1");
            items.Add("y", "2");

            // Act
            var result = UriHelper.ToQuery(items);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("x=1&y=2", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ToQuery_KeyRepeats()
        {
            // Arrange
            var items = new NameValueCollection();
            items.Add("x", "1");
            items.Add("x", "2");
            items.Add("y", "2");
            items.Add("y", "3");

            // Act
            var result = UriHelper.ToQuery(items);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("x=1&x=2&y=2&y=3", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ToQuery_Empty()
        {
            // Arrange
            var items = new NameValueCollection();

            // Act
            var result = UriHelper.ToQuery(items);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ToQuery_Null()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => UriHelper.ToQuery(null));

            // Assert
        }

        [Theory]
        [InlineData("?y=2&z=3&x=")]
        [InlineData("?y=2&x=&z=3")]
        [InlineData("?x=&y=2&z=3")]
        [InlineData("y=2&z=3&x=")]
        [InlineData("y=2&x=&z=3")]
        [InlineData("x=&y=2&z=3")]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ParseQuery(string query)
        {
            // Arrange

            // Act
            var result = UriHelper.ParseQuery(query);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(string.Empty, result["x"]);
            Assert.Equal("2", result["y"]);
            Assert.Equal("3", result["z"]);
        }

        [Theory]
        [InlineData("?x=1&x=2&y=2&y=3")]
        [InlineData("y=2&y=3&x=1&x=2")]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ParseQuery_KeyRepeats(string query)
        {
            // Arrange

            // Act
            var result = UriHelper.ParseQuery(query);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("1,2", String.Join(",", result.GetValues("x")));
            Assert.Equal("2,3", String.Join(",", result.GetValues("y")));
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ParseQuery_Empty()
        {
            // Arrange

            // Act
            var result = UriHelper.ParseQuery(String.Empty);

            // Assert
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "UriHelper")]
        public static void ParseQuery_Null()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => UriHelper.ParseQuery(null));

            // Assert
        }
    }
}
