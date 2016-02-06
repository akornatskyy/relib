using System;
using System.IO;
using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Memcached.Protocol;
using Xunit;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class TextPacketParserTest : IDisposable
    {
        private static readonly Encoding g_encoding = Encoding.ASCII;

        private readonly MemoryStream m_stream;
        private readonly IBinaryReader m_reader;
        private readonly TextPacketParser m_parser;

        public TextPacketParserTest()
        {
            m_stream = new MemoryStream();
            m_reader = new BinaryStreamReader(m_stream);
            m_parser = new TextPacketParser(m_reader, new Buffer<byte>(1024));
        }

        #region IDisposable Members

        public void Dispose()
        {
            Assert.Equal(-1, m_stream.ReadByte());
            m_stream.Dispose();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_Value()
        {
            // Arrange
            SetupStream("VALUE ");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.Value, result);
            Assert.Equal(6, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_Deleted()
        {
            // Arrange
            SetupStream("DELETED\r\n");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.NoError, result);
            Assert.Equal(9, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_Error()
        {
            // Arrange
            SetupStream("ERROR\r\n");

            // Act
            Assert.Throws<InvalidOperationException>(() => m_parser.ReadStatus());

            // Assert
            Assert.Equal(7, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_ClientError()
        {
            // Arrange
            SetupStream("CLIENT_ERROR bad command line\r\n");

            // Act
            var result = Assert.Throws<InvalidOperationException>(() => m_parser.ReadStatus());

            // Assert
            Assert.Equal("bad command line", result.Message);
            Assert.Equal(31, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_ServerError()
        {
            // Arrange
            SetupStream("SERVER_ERROR out of memory\r\n");

            // Act
            var result = Assert.Throws<InvalidOperationException>(() => m_parser.ReadStatus());

            // Assert
            Assert.Equal("out of memory", result.Message);
            Assert.Equal(28, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_End()
        {
            // Arrange
            SetupStream("END\r\n");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.NoError, result);
            Assert.Equal(5, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_Exists()
        {
            // Arrange
            SetupStream("EXISTS\r\n");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.KeyExists, result);
            Assert.Equal(8, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_Stored()
        {
            // Arrange
            SetupStream("STORED\r\n");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.NoError, result);
            Assert.Equal(8, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_NotStored()
        {
            // Arrange
            SetupStream("NOT_STORED\r\n");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.ItemNotStored, result);
            Assert.Equal(12, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_NotFound()
        {
            // Arrange
            SetupStream("NOT_FOUND\r\n");

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.KeyNotFound, result);
            Assert.Equal(11, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadStatus_Stat_NotImplemented()
        {
            // Arrange
            SetupStream("STAT ");

            // Act
            Assert.Throws<NotImplementedException>(() => m_parser.ReadStatus());

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadKey()
        {
            // Arrange
            SetupStream("key1234 ");

            // Act
            var result = m_parser.ReadKey();

            // Assert
            Assert.Equal("key1234", g_encoding.GetString(result));
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadFlags()
        {
            // Arrange
            SetupStream("1234567890 ");

            // Act
            var result = m_parser.ReadFlags();

            // Assert
            Assert.Equal(1234567890, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadLength()
        {
            // Arrange
            SetupStream("1234567890 ");

            // Act
            var result = m_parser.ReadLength();

            // Assert
            Assert.Equal(1234567890, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadVersion()
        {
            // Arrange
            SetupStream("1234567890 ");

            // Act
            var result = m_parser.ReadVersion();

            // Assert
            Assert.Equal(1234567890, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadIncrement()
        {
            // Arrange
            SetupStream("1234567890\r\n");

            // Act
            var result = m_parser.ReadIncrement();

            // Assert
            Assert.Equal(1234567890L, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadIncrement_Error()
        {
            // Arrange
            SetupStream("NOT_FOUND\r\n");

            // Act
            var result = m_parser.ReadIncrement();

            // Assert
            Assert.Equal(-1L, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketParser")]
        public void ReadValue()
        {
            // Arrange
            SetupStream("test_value\r\n");

            // Act
            var result = m_parser.ReadValue(10);

            // Assert
            Assert.Equal("test_value", g_encoding.GetString(result.Array, result.Offset, result.Count));
        }

        private void SetupStream(string input)
        {
            var bytes = g_encoding.GetBytes(input);
            m_stream.Write(bytes, 0, bytes.Length);
            m_stream.Position = 0;
        }
    }
}
