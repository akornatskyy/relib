using System;
using Moq;
using ReusableLibrary.Memcached.Protocol;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class BinaryCommandReaderTest : IDisposable
    {
        private readonly Mock<IPacketParser> m_mockPacketParser;
        private readonly BinaryCommandReader m_reader;

        public BinaryCommandReaderTest()
        {
            m_mockPacketParser = new Mock<IPacketParser>(MockBehavior.Strict);
            m_reader = new BinaryCommandReader(m_mockPacketParser.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockPacketParser.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadSucceed()
        {
            // Arrange

            // Act
            Assert.DoesNotThrow(() => m_reader.ReadSucceed());

            // Assert
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadValue(bool includeVersion)
        {
            // Arrange
            var key = new byte[] { 10 };
            var flags = 100;
            var length = 2;
            var version = includeVersion ? 1231L : 0L;
            var value = new ArraySegment<byte>(new byte[] { 10, 23 });
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.Value);
            m_mockPacketParser.Setup(parser => parser.ReadKey()).Returns(key);
            m_mockPacketParser.Setup(parser => parser.ReadFlags()).Returns(flags);
            m_mockPacketParser.Setup(parser => parser.ReadLength()).Returns(length);
            m_mockPacketParser.Setup(parser => parser.ReadValue(length)).Returns(value);
            if (includeVersion)
            {
                m_mockPacketParser.Setup(parser => parser.ReadVersion()).Returns(version);
            }

            // Act
            var result = m_reader.ReadValue(includeVersion);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(key, result.Key);
            Assert.Equal(flags, result.Flags);
            Assert.Equal(version, result.Version);
            Assert.Equal(value, result.Value);
        }

        [Theory]
        [InlineData(ResponseStatus.NoError)]
        [InlineData(ResponseStatus.KeyNotFound)]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadValue_NoValue(ResponseStatus status)
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(status);

            // Act
            var result = m_reader.ReadValue(false);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(ResponseStatus.InvalidArguments)]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadValue_Throws_InvalidOperationException(ResponseStatus status)
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(status);

            // Act
            Assert.Throws<InvalidOperationException>(() => m_reader.ReadValue(true));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadStored()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.NoError);

            // Act
            var result = m_reader.ReadStored();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(ResponseStatus.NoError)]
        [InlineData(ResponseStatus.KeyNotFound)]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadDeleted(ResponseStatus response)
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(response);

            // Act
            var result = m_reader.ReadDeleted();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(ResponseStatus.InvalidArguments)]
        [InlineData(ResponseStatus.OutOfMemory)]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadDeleted_Failed(ResponseStatus response)
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(response);

            // Act
            var result = m_reader.ReadDeleted();

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadIncrement()
        {
            // Arrange
            m_mockPacketParser
                .Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.NoError);
            m_mockPacketParser
                .Setup(parser => parser.ReadIncrement()).Returns(100L);

            // Act
            var result = m_reader.ReadIncrement();

            // Assert
            Assert.Equal(100L, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryCommandReader")]
        public void ReadIncrement_Error()
        {
            // Arrange
            m_mockPacketParser
                .Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.OutOfMemory);

            // Act
            var result = m_reader.ReadIncrement();

            // Assert
            Assert.Equal(-1L, result);
        }
    }
}
