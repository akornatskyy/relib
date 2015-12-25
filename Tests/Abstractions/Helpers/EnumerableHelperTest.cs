using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class EnumerableHelperTest
    {
        private static IEnumerable<int> g_items = new List<int>(new[] { 1, 2, 3, 4, 5 });

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ForEachIndexed_ActionCall()
        {
            // Arrange
            var index = 0;
            IEnumerable<int> items = g_items;

            // Act
            EnumerableHelper.ForEach(items, (i, item) => 
            {
                Assert.Equal(index, item - 1);
                Assert.Equal(index++, i);
            });

            // Assert
            Assert.Equal(5, index);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ForEachIndexed_Items_IsNull()
        {
            // Arrange
            IEnumerable<int> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableHelper.ForEach(items, (index, ignore) => Assert.False(true)));

            // Assert
            Assert.Null(items);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ForEach_OddEven_Action_Call()
        {
            // Arrange
            var items = g_items;
            var oddCalls = 0;
            var evenCalls = 0;

            // Act
            EnumerableHelper.ForEach(items, (odd) =>
            {
                oddCalls++;
                Assert.Equal(1, odd % 2);
            }, (even) =>
            {
                evenCalls++;
                Assert.Equal(0, even % 2);
            });

            // Assert
            Assert.Equal(3, oddCalls);
            Assert.Equal(2, evenCalls);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ForEach_Action_Call()
        {
            // Arrange
            var items = g_items;
            var calls = 0;

            // Act
            EnumerableHelper.ForEach(items, (item) => 
            {
                calls++;
                Assert.Equal(calls, item);
            });

            // Assert
            Assert.Equal(5, calls);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ForEach_Items_IsNull()
        {
            // Arrange
            IEnumerable<int> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableHelper.ForEach(items, (ignore) => Assert.False(true)));

            // Assert
            Assert.Null(items);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void Count_Items_IsNull()
        {
            // Arrange
            IEnumerable<int> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => EnumerableHelper.Count(items));

            // Assert
            Assert.Null(items);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void Count_Items()
        {
            // Arrange
            var items = g_items;

            // Act
            var count = EnumerableHelper.Count(items);

            // Assert
            Assert.Equal(5, count);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(true, 5)]
        [InlineData(false, 0)]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void Contains(bool expected, int value)
        {
            // Arrange
            var items = g_items;

            // Act
            var result = EnumerableHelper.Contains(items, value);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void Contains_Items_IsNull()
        {
            // Arrange
            IEnumerable<int> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => EnumerableHelper.Contains(items, 1));

            // Assert
            Assert.Null(items);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ToDictionary_ValueObject()
        {
            // Arrange
            var items = new[] { new ValueObject<int>(1, "1"), new ValueObject<int>(2, "2") };

            // Act
            var result = EnumerableHelper.ToDictionary<int, ValueObject<int>>(items);

            // Assert
            Assert.Equal(items.Length, result.Count);
            Assert.Equal("1", result[1].DisplayName);
            Assert.Equal("2", result[2].DisplayName);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ToDictionary_Items_IsNull()
        {
            // Arrange
            IEnumerable<ValueObject<int>> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => EnumerableHelper.ToDictionary<int, ValueObject<int>>(items));

            // Assert
            Assert.Null(items);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ToDictionary()
        {
            // Arrange
            var items = new[] { new ValueObject<int>(1, "1"), new ValueObject<int>(2, "2") };

            // Act
            var result = EnumerableHelper.ToDictionary(items, 
                vo => new KeyValuePair<int, string>(vo.Key, vo.DisplayName));

            // Assert
            Assert.Equal(items.Length, result.Count);
            Assert.Equal("1", result[1]);
            Assert.Equal("2", result[2]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void ToNameValueCollection()
        {
            // Arrange
            var items = new[] { new ValueObject<string>("1", "A1"), new ValueObject<string>("2", "A2") };

            // Act
            var result = EnumerableHelper.ToNameValueCollection(items,
                vo => new KeyValuePair<string, string>(vo.Key, vo.DisplayName));

            // Assert
            Assert.Equal(items.Length, result.Count);
            Assert.Equal("A1", result["1"]);
            Assert.Equal("A2", result["2"]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void Translate()
        {
            // Arrange
            var items = new[] { new ValueObject<int>(1, "1"), new ValueObject<int>(2, "2") };

            // Act
            var result = new List<int>(EnumerableHelper.Translate(items, item => item.Key));

            // Assert
            Assert.Equal(items.Length, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "EnumerableHelper")]
        public static void Translate_Items_IsNull()
        {
            // Arrange
            IEnumerable<ValueObject<int>> items = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => EnumerableHelper.Translate(items, item => item.Key));

            // Assert
            Assert.Null(items);
        }
    }
}
