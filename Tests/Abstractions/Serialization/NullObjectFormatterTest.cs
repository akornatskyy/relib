using System;
using Moq;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class NullObjectFormatterTest : IDisposable
    {
        private readonly Mock<IObjectFormatter> m_mockInner;

        public NullObjectFormatterTest()
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
        [Trait(Constants.TraitNames.Serialization, "NullObjectFormatter")]
        public void Serialize()
        {
            // Arrange
            var formatter = new NullObjectFormatter(m_mockInner.Object);
            var flags = -1;

            // Act
            var result = formatter.Serialize<string>(null, out flags);

            // Assert
            Assert.Equal(0, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(0, result.Offset);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "NullObjectFormatter")]
        public void Serialize_DoesNot_Handle()
        {
            // Arrange
            var formatter = new NullObjectFormatter(m_mockInner.Object);
            var flags = -1;
            m_mockInner.Setup(inner => inner.Serialize<string>("test", out flags)).Returns(new ArraySegment<byte>());

            // Act
            var result = formatter.Serialize<string>("test", out flags);

            // Assert
            Assert.Equal(-1, flags);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "NullObjectFormatter")]
        public void Deserialize()
        {
            // Arrange
            var formatter = new NullObjectFormatter(m_mockInner.Object);
            var flags = 0;

            // Act
            var result = formatter.Deserialize<string>(new ArraySegment<byte>(), flags);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "NullObjectFormatter")]
        public void Deserialize_DoesNot_Handle()
        {
            // Arrange
            var formatter = new NullObjectFormatter(m_mockInner.Object);
            var flags = -1;
            var data = new ArraySegment<byte>(new byte[] { });
            m_mockInner.Setup(inner => inner.Deserialize<string>(data, flags)).Returns("x");

            // Act
            var result = formatter.Deserialize<string>(data, flags);

            // Assert
            Assert.Equal("x", result);
        }
    }
}
