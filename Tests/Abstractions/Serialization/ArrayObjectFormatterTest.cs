using System;
using Moq;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class ArrayObjectFormatterTest : IDisposable
    {
        private readonly Mock<IObjectFormatter> m_mockInner;

        public ArrayObjectFormatterTest()
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
        [Trait(Constants.TraitNames.Serialization, "ArrayObjectFormatter")]
        public void Serialize()
        {
            // Arrange
            var formatter = new ArrayObjectFormatter(m_mockInner.Object);
            var flags = -1;
            var data = new byte[] { 7, 9 };

            // Act
            var result = formatter.Serialize<byte[]>(data, out flags);

            // Assert
            Assert.Equal(ArrayObjectFormatter.ArrayFlag, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(data, result.Array);
            Assert.Same(data, result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "ArrayObjectFormatter")]
        public void Serialize_DoesNot_Handle()
        {
            // Arrange
            var formatter = new ArrayObjectFormatter(m_mockInner.Object);
            var flags = -1;
            m_mockInner.Setup(inner => inner.Serialize<string>("x", out flags)).Returns(new ArraySegment<byte>());

            // Act
            var result = formatter.Serialize<string>("x", out flags);

            // Assert
            Assert.Equal(-1, flags);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "ArrayObjectFormatter")]
        public void Deserialize()
        {
            // Arrange
            var formatter = new ArrayObjectFormatter(m_mockInner.Object);
            var flags = ArrayObjectFormatter.ArrayFlag;
            var data = new byte[] { 7, 9 };

            // Act
            var result = formatter.Deserialize<byte[]>(new ArraySegment<byte>(data), flags);

            // Assert
            Assert.Equal(ArrayObjectFormatter.ArrayFlag, flags);
            Assert.NotNull(result);
            Assert.Equal(data, result);
            Assert.NotSame(data, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "ArrayObjectFormatter")]
        public void Deserialize_DoesNot_Handle()
        {
            // Arrange
            var formatter = new ArrayObjectFormatter(m_mockInner.Object);
            var flags = 0;
            var data = new ArraySegment<byte>(new byte[] { });
            m_mockInner.Setup(inner => inner.Deserialize<string>(data, flags)).Returns("x");

            // Act
            var result = formatter.Deserialize<string>(data, flags);

            // Assert
            Assert.Equal("x", result);
        }
    }
}
