using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Memcached.Protocol;
using ReusableLibrary.Supplemental.Collections;
using ReusableLibrary.Supplemental.System;
using Xunit;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class BinaryPacketParserTest : IDisposable
    {
        #region Protocol Responses

        private const string ErrorResponse =
            "81 01 00 00 " +
            "00 00 00 {0} " +
            "00 00 00 09 " +
            "00 00 00 00 " +
            "00 00 00 00 " +
            "00 00 00 00 " +
            "4E 6F 74 20 " +
            "66 6F 75 6E " +
            "64";

        private const string GetResponse =
            "81 00 00 00 " +
            "04 00 00 {0} " +
            "00 00 00 09 " +
            "00 00 00 00 " +
            "00 00 00 00 " +
            "00 00 00 01 " +
            "DE AD BE EF " +
            "57 6F 72 6C " +
            "64";

        private const string IncrResponse =
            "81 05 00 00 " +
            "00 00 00 00 " +
            "00 00 00 08 " +
            "00 00 00 00 " +
            "00 00 00 00 " +
            "00 00 00 05 " +
            "{0}";

        #endregion

        private readonly MemoryStream m_stream;
        private readonly IBinaryReader m_reader;
        private readonly BinaryPacketParser m_parser;

        public BinaryPacketParserTest()
        {
            m_stream = new MemoryStream();
            m_reader = new BinaryStreamReader(m_stream);
            m_parser = new BinaryPacketParser(m_reader, new Buffer<byte>(1024));
        }

        #region IDisposable Members

        public void Dispose()
        {
            Assert.Equal(-1, m_stream.ReadByte());
            m_stream.Dispose();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_Value()
        {
            // Arrange
            var length = SetupStream(GetResponse.FormatWith("00"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.Value, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_NoError()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("00"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.NoError, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_KeyNotFound()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("01"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.KeyNotFound, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_KeyExists()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("02"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.KeyExists, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_ValueTooLarge()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("03"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.ValueTooLarge, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_InvalidArguments()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("04"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.InvalidArguments, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_ItemNotStored()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("05"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.ItemNotStored, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_UnknownCommand()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("81"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.UnknownCommand, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadStatus_OutOfMemory()
        {
            // Arrange
            var length = SetupStream(ErrorResponse.FormatWith("82"));

            // Act
            var result = m_parser.ReadStatus();

            // Assert
            Assert.Equal(ResponseStatus.OutOfMemory, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadKey()
        {
            // Arrange
            var length = SetupStream(GetResponse.FormatWith("00"));
            m_parser.ReadStatus();

            // Act
            var result = m_parser.ReadKey();

            // Assert
            Assert.Equal(0, result.Length);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadFlags()
        {
            // Arrange
            var length = SetupStream(GetResponse.FormatWith("00"));
            m_parser.ReadStatus();

            // Act
            var result = m_parser.ReadFlags();

            // Assert
            Assert.Equal(Int32.Parse("deadbeef", NumberStyles.HexNumber, CultureInfo.InvariantCulture), result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadLength()
        {
            // Arrange
            var length = SetupStream(GetResponse.FormatWith("00"));
            m_parser.ReadStatus();

            // Act
            var result = m_parser.ReadLength();

            // Assert
            Assert.Equal(5, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadVersion()
        {
            // Arrange
            var length = SetupStream(GetResponse.FormatWith("00"));
            m_parser.ReadStatus();

            // Act
            var result = m_parser.ReadVersion();

            // Assert
            Assert.Equal(1L, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadIncrement()
        {
            // Arrange
            var length = SetupStream(IncrResponse.FormatWith("01 A0 30 11 06 20 10 41"));
            m_parser.ReadStatus();

            // Act
            var result = m_parser.ReadIncrement();

            // Assert
            Assert.Equal(117146439986974785L, result);
            Assert.Equal(length, m_stream.Position);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketParser")]
        public void ReadValue()
        {
            // Arrange
            var length = SetupStream(GetResponse.FormatWith("00"));
            m_parser.ReadStatus();

            // Act
            var result = m_parser.ReadValue(m_parser.ReadLength());

            // Assert
            Assert.Equal("World", Encoding.UTF8.GetString(result.Array, result.Offset, result.Count));
            Assert.Equal(length, m_stream.Position);
        }

        private int SetupStream(string input)
        {
            var codes = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var bytes = codes.Translate(c => byte.Parse(c, NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToArray();

            m_stream.Write(bytes, 0, bytes.Length);
            m_stream.Position = 0;
            return bytes.Length;
        }
    }
}
