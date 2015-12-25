using System;
using Moq;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class DecoratedObjectFormatterTest : IDisposable
    {
        private readonly Mock<IObjectFormatter> m_mockInner;

        public DecoratedObjectFormatterTest()
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
        [Trait(Constants.TraitNames.Serialization, "DecoratedObjectFormatter")]
        public void Serialize()
        {
            // Arrange
            var mockFormatter = new Mock<DecoratedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = -1;
            mockFormatter.CallBase = true;
            m_mockInner.Setup(inner => inner.Serialize<string>(null, out flags)).Returns(new ArraySegment<byte>());

            // Act
            var result = formatter.Serialize<string>(null, out flags);

            // Assert
            Assert.Equal(-1, flags);
            Assert.Equal(0, result.Count);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "DecoratedObjectFormatter")]
        public void Deserialize()
        {
            // Arrange
            var mockFormatter = new Mock<DecoratedObjectFormatter>(m_mockInner.Object);
            var formatter = mockFormatter.Object;
            var flags = -1;
            mockFormatter.CallBase = true;
            var data = new ArraySegment<byte>(new byte[] { });
            m_mockInner.Setup(inner => inner.Deserialize<string>(data, flags)).Returns("x");

            // Act
            var result = formatter.Deserialize<string>(data, flags);

            // Assert
            Assert.Equal("x", result);
            mockFormatter.VerifyAll();
        }
    }
}
