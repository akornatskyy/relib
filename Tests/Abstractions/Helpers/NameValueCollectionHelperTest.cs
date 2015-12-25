using System;
using System.Collections.Specialized;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public static class NameValueCollectionHelperTest
    {
        [Theory]
        [InlineData("x", true)]
        [InlineData("X", true)]
        [InlineData("y", true)]
        [InlineData(null, true)]
        [InlineData("z", false)]        
        [Trait(Constants.TraitNames.Helpers, "NameValueCollectionHelper")]
        public static void HasKey(string key, bool expected)
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", "1");
            collection.Add("y", "2");
            collection.Add(null, "3");

            // Act
            var result = NameValueCollectionHelper.HasKey(collection, key);

            // Assert
            Assert.Equal(expected, result);
            Assert.Equal(expected, collection[key] != null);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "NameValueCollectionHelper")]
        public static void HasKey_Collection_IsNull()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => NameValueCollectionHelper.HasKey(null, "x"));

            // Assert
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Models, "NameValueCollectionHelper")]
        public static void ConvertToInt32_Required(string value)
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", value);

            // Act
            Assert.Throws<ArgumentException>(() => 
                NameValueCollectionHelper.ConvertToInt32(collection, "x"));

            // Assert
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Models, "NameValueCollectionHelper")]
        public static void ConvertToInt32_Default(string value)
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", value);

            // Act
            var result = NameValueCollectionHelper.ConvertToInt32(collection, "x", 100);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "NameValueCollectionHelper")]
        public static void ConvertToInt32()
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", "100");

            // Act
            var result = NameValueCollectionHelper.ConvertToInt32(collection, "x");

            // Assert
            Assert.Equal(100, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Models, "NameValueCollectionHelper")]
        public static void ConvertToBoolean_Required(string value)
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", value);

            // Act
            Assert.Throws<ArgumentException>(() =>
                NameValueCollectionHelper.ConvertToBoolean(collection, "x"));

            // Assert
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Models, "NameValueCollectionHelper")]
        public static void ConvertToBoolean_Default(string value)
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", value);

            // Act
            var result = NameValueCollectionHelper.ConvertToBoolean(collection, "x", true);

            // Assert
            Assert.Equal(true, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "NameValueCollectionHelper")]
        public static void ConvertToBoolean()
        {
            // Arrange
            var collection = new NameValueCollection();
            collection.Add("x", "true");

            // Act
            var result = NameValueCollectionHelper.ConvertToBoolean(collection, "x");

            // Assert
            Assert.Equal(true, result);
        }
    }
}
