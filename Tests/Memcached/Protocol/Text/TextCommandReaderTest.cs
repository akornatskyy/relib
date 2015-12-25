using System;
using Moq;
using ReusableLibrary.Memcached.Protocol;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class TextCommandReaderTest : IDisposable
    {
        private readonly Mock<IPacketParser> m_mockPacketParser;
        private readonly TextCommandReader m_reader;

        public TextCommandReaderTest()
        {
            m_mockPacketParser = new Mock<IPacketParser>(MockBehavior.Strict);
            m_reader = new TextCommandReader(m_mockPacketParser.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockPacketParser.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadSucceed()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.NoError);

            // Act
            Assert.DoesNotThrow(() => m_reader.ReadSucceed());

            // Assert
        }

        [Theory]
        [InlineData(ResponseStatus.InvalidArguments)]
        [InlineData(ResponseStatus.ItemNotStored)]
        [InlineData(ResponseStatus.KeyExists)]
        [InlineData(ResponseStatus.KeyNotFound)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadSucceed_Throws_InvalidOperationException(ResponseStatus status)
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(status);

            // Act
            Assert.Throws<InvalidOperationException>(() => m_reader.ReadSucceed());

            // Assert
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
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
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
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
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadValue_Throws_InvalidOperationException(ResponseStatus status)
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(status);

            // Act
            Assert.Throws<InvalidOperationException>(() => m_reader.ReadValue(true));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadStored()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.NoError);

            // Act
            var result = m_reader.ReadStored();

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadStored_ItemNotStored()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.ItemNotStored);

            // Act
            var result = m_reader.ReadStored();

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadDeleted()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.NoError);

            // Act
            var result = m_reader.ReadDeleted();

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadDeleted_KeyNotFound()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadStatus()).Returns(ResponseStatus.KeyNotFound);

            // Act
            var result = m_reader.ReadDeleted();

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandReader")]
        public void ReadIncrement()
        {
            // Arrange
            m_mockPacketParser.Setup(parser => parser.ReadIncrement()).Returns(100L);

            // Act
            var result = m_reader.ReadIncrement();

            // Assert
            Assert.Equal(100L, result);
        }
    }
}
