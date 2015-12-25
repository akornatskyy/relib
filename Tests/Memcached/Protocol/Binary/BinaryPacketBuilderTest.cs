using System;
using System.Text;
using Moq;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Memcached.Protocol;
using ReusableLibrary.Supplemental.System;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class BinaryPacketBuilderTest : IDisposable
    {
        private readonly Mock<IBinaryWriter> m_mockBinaryWriter;
        private readonly IPacketBuilder m_builder;

        public BinaryPacketBuilderTest()
        {
            m_mockBinaryWriter = new Mock<IBinaryWriter>(MockBehavior.Strict);
            m_builder = new BinaryPacketBuilder(new Buffer<byte>(100));
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockBinaryWriter.VerifyAll();
        }

        #endregion

        [Theory]
        [InlineData(0x00, RequestOperation.Get)]
        [InlineData(0x01, RequestOperation.Set)]
        [InlineData(0x04, RequestOperation.Delete)]
        [InlineData(0x0B, RequestOperation.Version)]
        [InlineData(0x08, RequestOperation.Flush)]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteOperation(int expected, RequestOperation operation)
        {
            // Arrange
            int result = -1;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = b[1]).Returns(0);

            // Act
            m_builder.WriteOperation(operation);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0x01, RequestOperation.Set)]
        [InlineData(0x04, RequestOperation.Delete)]
        [InlineData(0x08, RequestOperation.Flush)]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteNoReply(int expected, RequestOperation operation)
        {
            // Arrange
            int result = -1;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = b[1]).Returns(0);
            m_builder.WriteOperation(operation);

            // Act
            m_builder.WriteNoReply();

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(expected | 0x10, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteLength()
        {
            // Arrange
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Returns(0);

            // Act
            m_builder.WriteLength(1000);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteVersion()
        {
            // Arrange
            long result = -1;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = BigEndianConverter.GetInt64(b, 16)).Returns(0);

            // Act
            m_builder.WriteVersion(1234567890123456L);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(1234567890123456L, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteFlags()
        {
            // Arrange
            int extra = -1;
            int result = -1;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                {
                    extra = b[4];
                    result = BigEndianConverter.GetInt32(b, 24);
                }).Returns(0);
            m_builder.Reset();
            m_builder.WriteOperation(RequestOperation.Set);

            // Act
            m_builder.WriteFlags(123456789);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(8, extra);
            Assert.Equal(123456789, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteDelta()
        {
            // Arrange
            int extra = -1;
            long delta = -1L;
            long initial = -1L;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                {
                    extra = b[4];
                    delta = BigEndianConverter.GetInt64(b, 24);
                    initial = BigEndianConverter.GetInt64(b, 32);
                }).Returns(0);
            m_builder.Reset();
            m_builder.WriteOperation(RequestOperation.Increment);

            // Act
            m_builder.WriteDelta(10L, 1001L);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(20, extra);
            Assert.Equal(10L, delta);
            Assert.Equal(1001L, initial);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteExpires()
        {
            // Arrange
            int extra = -1;
            int result = -1;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                {
                    extra = b[4];
                    result = BigEndianConverter.GetInt32(b, 24);
                }).Returns(0);
            m_builder.Reset();
            m_builder.WriteOperation(RequestOperation.Set);

            // Act
            m_builder.WriteExpires(123456789);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(0, extra);
            Assert.Equal(123456789, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteKey()
        {
            // Arrange
            int keylength = -1;
            string result = string.Empty;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                {
                    keylength = BigEndianConverter.GetInt16(b, 2);
                    result = Encoding.ASCII.GetString(b, 24, 5);
                }).Returns(0);
            m_builder.Reset();
            m_builder.WriteOperation(RequestOperation.Set);

            // Act
            m_builder.WriteKey(Encoding.ASCII.GetBytes("Hello"));

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(5, keylength);
            Assert.Equal("Hello", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteValue()
        {
            // Arrange
            string result = string.Empty;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                {
                    result = Encoding.ASCII.GetString(b, 24, 5);
                }).Returns(0);
            m_builder.Reset();
            m_builder.WriteOperation(RequestOperation.Set);

            // Act
            m_builder.WriteValue(new ArraySegment<byte>(Encoding.ASCII.GetBytes("World")));

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal("World", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "BinaryPacketBuilder")]
        public void WriteTo()
        {
            // Arrange
            int result = 0;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                {
                    result = BigEndianConverter.GetInt32(b, 8);
                }).Returns(0);
            m_builder.Reset();
            m_builder.WriteOperation(RequestOperation.Set);
            m_builder.WriteFlags(1234);
            m_builder.WriteKey(Encoding.ASCII.GetBytes("Hello"));
            m_builder.WriteValue(new ArraySegment<byte>(Encoding.ASCII.GetBytes("World")));

            // Act
            m_builder.WriteTo(m_mockBinaryWriter.Object);

            // Assert            
            Assert.Equal(14, result);
        }
    }
}
