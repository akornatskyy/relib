using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Net;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public static class BigEndianConverterTest
    {
        private static readonly Random g_random = new Random();

        [Fact]
        [Trait(Constants.TraitNames.Net, "BigEndianConverter")]
        public static void Convert_Int16()
        {
            // Arrange
            var value = RandomHelper.NextInt(g_random, Int16.MinValue, Int16.MaxValue);

            // Act
            var bytes = BigEndianConverter.GetBytes(value);
            var result = BigEndianConverter.GetInt32(bytes, 0);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "BigEndianConverter")]
        public static void Convert_Int32()
        {
            // Arrange
            var value = RandomHelper.NextInt(g_random, Int32.MinValue, Int32.MaxValue - 1);

            // Act
            var bytes = BigEndianConverter.GetBytes(value);
            var result = BigEndianConverter.GetInt32(bytes, 0);

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(1234567890L)]
        [InlineData(731234678567890L)]
        [InlineData(9223372036854775807L)]
        [InlineData(-9223372036854775808L)]
        [InlineData(-45482342934393L)]
        [InlineData(-1L)]
        [Trait(Constants.TraitNames.Net, "BigEndianConverter")]
        public static void Convert_Int64(long value)
        {
            // Arrange

            // Act
            var bytes = BigEndianConverter.GetBytes(value);
            var result = BigEndianConverter.GetInt64(bytes, 0);

            // Assert
            Assert.Equal(value, result);
        }
    }
}
