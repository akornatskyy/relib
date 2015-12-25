using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class BufferTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "Buffer")]
        public static void ArrayCapacity()
        {
            // Arrange
            var buffer = new Buffer<byte>(10);

            // Act
            var result = buffer.Array;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Length);
            Assert.Equal(10, buffer.Capacity);
            Assert.Equal(result, buffer.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Buffer")]
        public static void EnsureCapacity_LessThanAvailable()
        {
            // Arrange
            var buffer = new Buffer<byte>(10);
            var before = buffer.Array;

            // Act
            buffer.EnsureCapacity(5);

            // Assert
            Assert.Equal(before, buffer.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Buffer")]
        public static void EnsureCapacity_MoreThanAvailable()
        {
            // Arrange
            var buffer = new Buffer<byte>(10, 2);
            var before = buffer.Array;

            // Act
            buffer.EnsureCapacity(15);

            // Assert
            Assert.NotEqual(before, buffer.Array);
            Assert.Equal(20, buffer.Capacity);
        }
    }
}
