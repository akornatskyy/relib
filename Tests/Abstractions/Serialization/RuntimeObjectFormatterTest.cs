using System;
using System.IO;
using System.Runtime.Serialization;
using Moq;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class RuntimeObjectFormatterTest : IDisposable
    {
        private readonly Mock<IObjectFormatter> m_mockInner;

        public RuntimeObjectFormatterTest()
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
        [Trait(Constants.TraitNames.Serialization, "RuntimeObjectFormatter")]
        public void Serialize()
        {
            // Arrange
            var mockFormatter = new Mock<IFormatter>(MockBehavior.Strict);
            var formatter = new RuntimeObjectFormatter(mockFormatter.Object, m_mockInner.Object);
            var flags = -1;
            var value = new object();
            mockFormatter.Setup(inner => inner.Serialize(It.IsAny<Stream>(), value));

            // Act
            var result = formatter.Serialize<object>(value, out flags);

            // Assert
            Assert.Equal(RuntimeObjectFormatter.RuntimeObjectFlag | (int)Type.GetTypeCode(value.GetType()), flags);
            Assert.NotNull(result.Array);
            Assert.Equal(0, result.Offset);
            Assert.Equal(0, result.Count);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "RuntimeObjectFormatter")]
        public void Deserialize()
        {
            // Arrange
            var mockFormatter = new Mock<IFormatter>(MockBehavior.Strict);
            var formatter = new RuntimeObjectFormatter(mockFormatter.Object, m_mockInner.Object);
            var flags = RuntimeObjectFormatter.RuntimeObjectFlag;
            var data = new ArraySegment<byte>(new byte[] { });
            var value = new object();
            mockFormatter.Setup(inner => inner.Deserialize(It.IsAny<Stream>())).Returns(value);

            // Act
            var result = formatter.Deserialize<object>(data, flags);

            // Assert
            Assert.Equal(value, result);
            mockFormatter.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "RuntimeObjectFormatter")]
        public void Deserialize_DoesNot_Handle()
        {
            // Arrange
            var mockFormatter = new Mock<IFormatter>(MockBehavior.Strict);
            var formatter = new RuntimeObjectFormatter(mockFormatter.Object, m_mockInner.Object);
            var flags = 0;
            var value = new object();
            var data = new ArraySegment<byte>(new byte[] { });
            m_mockInner.Setup(inner => inner.Deserialize<object>(data, flags)).Returns(value);

            // Act
            var result = formatter.Deserialize<object>(data, flags);

            // Assert
            Assert.Equal(value, result);
            mockFormatter.VerifyAll();
        }
    }
}
