using System;
using Moq;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class CompressedObjectFormatterTest : IDisposable
    {
        private readonly Mock<IObjectFormatter> m_mockInner;

        public CompressedObjectFormatterTest()
        {
            m_mockInner = new Mock<IObjectFormatter>(MockBehavior.Strict);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockInner.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "CompressedObjectFormatter")]
        public void Serialize_Below_CompressionThreshold()
        {
            // Arrange
            var mockFormatter = new Mock<CompressedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = 0;
            var value = new object();
            var data = new ArraySegment<byte>(new byte[] { 20 });
            mockFormatter.CallBase = true;
            formatter.CompressionThreshold = 2;
            m_mockInner.Setup(inner => inner.Serialize<object>(value, out flags)).Returns(data);

            // Act
            var result = formatter.Serialize<object>(value, out flags);

            // Assert
            Assert.Equal(0, flags);
            Assert.Equal(data, result);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "CompressedObjectFormatter")]
        public void Serialize_CompressionedSize_Greater_Original()
        {
            // Arrange
            var mockFormatter = new Mock<CompressedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = 0;
            var value = new object();
            var data = new ArraySegment<byte>(new byte[] { 20, 20 });
            mockFormatter.CallBase = true;
            formatter.CompressionThreshold = 1;
            mockFormatter.Setup(f => f.Compress(data)).Returns(new ArraySegment<byte>(new byte[] { 20, 20, 20 }));
            m_mockInner.Setup(inner => inner.Serialize<object>(value, out flags)).Returns(data);

            // Act
            var result = formatter.Serialize<object>(value, out flags);

            // Assert
            Assert.Equal(0, flags);
            Assert.Equal(data, result);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "CompressedObjectFormatter")]
        public void Serialize_CompressionedSize_Less_Original()
        {
            // Arrange
            var mockFormatter = new Mock<CompressedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = 0;
            var value = new object();
            var data = new ArraySegment<byte>(new byte[] { 20, 20 });
            mockFormatter.CallBase = true;
            formatter.CompressionThreshold = 1;
            mockFormatter.Setup(f => f.Compress(data)).Returns(new ArraySegment<byte>(new byte[] { 20 }));
            m_mockInner.Setup(inner => inner.Serialize<object>(value, out flags)).Returns(data);

            // Act
            var result = formatter.Serialize<object>(value, out flags);

            // Assert
            Assert.Equal(CompressedObjectFormatter.CompressedFlag, flags);
            Assert.Equal(1, result.Count);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "CompressedObjectFormatter")]
        public void Deserialize()
        {
            // Arrange
            var mockFormatter = new Mock<CompressedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = CompressedObjectFormatter.CompressedFlag | 1;
            var data = new ArraySegment<byte>(new byte[] { 10, 20 });
            var decompressed = new ArraySegment<byte>(new byte[] { 15 });
            var value = new object();
            mockFormatter.CallBase = true;
            mockFormatter.Setup(f => f.Decompress(data)).Returns(decompressed);
            m_mockInner.Setup(inner => inner.Deserialize<object>(decompressed, 1)).Returns(value);

            // Act
            var result = formatter.Deserialize<object>(data, flags);

            // Assert
            Assert.Equal(value, result);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "CompressedObjectFormatter")]
        public void Deserialize_DoesNot_Handle()
        {
            // Arrange
            var mockFormatter = new Mock<CompressedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = 1;
            var data = new ArraySegment<byte>(new byte[] { });
            mockFormatter.CallBase = true;
            m_mockInner.Setup(inner => inner.Deserialize<string>(data, flags)).Returns("x");

            // Act
            var result = formatter.Deserialize<string>(data, flags);

            // Assert
            Assert.Equal("x", result);
        }
    }
}
