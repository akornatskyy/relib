using System;
using Moq;
using Moq.Matchers;
using ReusableLibrary.Abstractions.Caching;
using Xunit;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Serialization.Formatters;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class LazyDataKeyTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void Value()
        {
            // Arrange
            var datakey = new LazyDataKey<int>("key");
            datakey.Value = 100;

            // Act
            var result = datakey.Value;

            // Assert
            Assert.True(datakey.HasValue);
            Assert.Equal(100, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void Value_HasNo_Value()
        {
            // Arrange
            var datakey = new LazyDataKey<int>("key");

            // Act
            var result = datakey.Value;

            // Assert
            Assert.False(datakey.HasValue);
            Assert.Equal(0, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void Load_HasValue()
        {
            // Arrange
            var mockFormatter = new Mock<IObjectFormatter>(MockBehavior.Strict);
            var datakey = new LazyDataKey<int>("key");
            datakey.Initialize(mockFormatter.Object);

            // Act
            datakey.Load(new ArraySegment<byte>(new byte[] { }), 0);

            // Assert            
            Assert.True(datakey.HasValue);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void Load_Value()
        {
            // Arrange
            var mockFormatter = new Mock<IObjectFormatter>(MockBehavior.Strict);
            var datakey = new LazyDataKey<int>("key");
            mockFormatter
                .Setup(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7))
                .Returns(100);
            datakey.Initialize(mockFormatter.Object);

            // Act
            datakey.Load(new ArraySegment<byte>(new byte[] { }), 7);
            mockFormatter.Verify(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7), Times.Never());
            Assert.Equal(100, datakey.Value);
            mockFormatter.Verify(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7), Times.Once());

            // Assert            
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void Load_Value_Doesnt_Deserialize_Twice()
        {
            // Arrange
            var mockFormatter = new Mock<IObjectFormatter>(MockBehavior.Strict);
            var datakey = new LazyDataKey<int>("key");
            mockFormatter
                .Setup(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7))
                .Returns(100);
            datakey.Initialize(mockFormatter.Object);
            datakey.Load(new ArraySegment<byte>(new byte[] { }), 7);
            Assert.True(datakey.HasValue);
            mockFormatter.Verify(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7), Times.Never());
            Assert.Equal(100, datakey.Value);
            mockFormatter.Verify(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7), Times.Once());

            // Act
            Assert.Equal(100, datakey.Value);
            mockFormatter.Verify(formatter => formatter.Deserialize<int>(It.IsAny<ArraySegment<byte>>(), 7), Times.Once());

            // Assert            
            mockFormatter.VerifyAll();
        }
    }
}
