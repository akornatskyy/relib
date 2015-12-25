using System;
using System.Text;
using Moq;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Memcached.Protocol;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class TextPacketBuilderTest : IDisposable
    {
        private readonly Mock<IBinaryWriter> m_mockBinaryWriter;
        private readonly IPacketBuilder m_builder;

        public TextPacketBuilderTest()
        {
            m_mockBinaryWriter = new Mock<IBinaryWriter>(MockBehavior.Strict);
            m_builder = new TextPacketBuilder(new Buffer<byte>(100));
        }

        [Theory]
        [InlineData(RequestOperation.Get, GetOperation.Get)]
        [InlineData(RequestOperation.Gets, GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public static void RequestOperation_Contains_GetOperation(RequestOperation ro, GetOperation go)
        {
            // Arrange

            // Act
            Assert.Equal((int)ro, (int)go);

            // Assert
        }

        [Theory]
        [InlineData(RequestOperation.Add, StoreOperation.Add)]
        [InlineData(RequestOperation.CheckAndSet, StoreOperation.CheckAndSet)]
        [InlineData(RequestOperation.Replace, StoreOperation.Replace)]
        [InlineData(RequestOperation.Set, StoreOperation.Set)]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public static void RequestOperation_Contains_StoreOperation(RequestOperation ro, StoreOperation so)
        {
            // Arrange

            // Act
            Assert.Equal((int)ro, (int)so);

            // Assert
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockBinaryWriter.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void Reset()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) => 
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.Reset();

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("get", RequestOperation.Get)]
        [InlineData("gets", RequestOperation.Gets)]
        [InlineData("set", RequestOperation.Set)]
        [InlineData("cas", RequestOperation.CheckAndSet)]
        [InlineData("delete", RequestOperation.Delete)]
        [InlineData("incr", RequestOperation.Increment)]
        [InlineData("decr", RequestOperation.Decrement)]
        [InlineData("version", RequestOperation.Version)]
        [InlineData("flush_all", RequestOperation.Flush)]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteOperation(string expected, RequestOperation operation)
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteOperation(operation);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteKey()
        {
            // Arrange
            var key = "Key1";
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteKey(Encoding.ASCII.GetBytes(key));

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" " + key, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteFlags()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteFlags(1234567890);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" 1234567890", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteExpires()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteExpires(1234567890);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" 1234567890", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteLength()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteLength(1234567890);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" 1234567890", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteVersion()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteVersion(1234567890);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" 1234567890", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteDelta()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteDelta(10L, 0L);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" 10", result);
        }

        [Theory]
        [InlineData(" noreply")]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteNoReply(string expected)
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteNoReply();

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteDelay()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteDelay(1234567890);

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal(" 1234567890", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextPacketBuilder")]
        public void WriteValue()
        {
            // Arrange
            string result = null;
            m_mockBinaryWriter.Setup(writer => writer.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()))
                .Callback<byte[], int, int>((b, i, c) =>
                    result = Encoding.ASCII.GetString(b, i, c - 2)).Returns(0);

            // Act
            m_builder.WriteValue(new ArraySegment<byte>(Encoding.ASCII.GetBytes("test")));

            // Assert
            m_builder.WriteTo(m_mockBinaryWriter.Object);
            Assert.Equal("\r\ntest", result);
        }
    }
}
