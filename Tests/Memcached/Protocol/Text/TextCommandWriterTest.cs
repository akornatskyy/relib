using System;
using Moq;
using ReusableLibrary.Memcached.Protocol;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class TextCommandWriterTest : IDisposable
    {
        private readonly Mock<IPacketBuilder> m_mockPacketBuilder;
        private readonly TextCommandWriter m_writer;

        public TextCommandWriterTest()
        {
            m_mockPacketBuilder = new Mock<IPacketBuilder>(MockBehavior.Strict);
            m_writer = new TextCommandWriter(m_mockPacketBuilder.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockPacketBuilder.VerifyAll();
        }

        #endregion

        [Theory]
        [InlineData(StoreOperation.Add)]
        [InlineData(StoreOperation.CheckAndSet)]
        [InlineData(StoreOperation.Replace)]
        [InlineData(StoreOperation.Set)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void Store(StoreOperation operation)
        {
            // Arrange
            var packet = new StorePacket()
            {
                Expires = 1000,
                Flags = 101,
                Key = new byte[] { 7 },
                Operation = operation,
                Value = new ArraySegment<byte>(new byte[] { 46, 62 }),
                Version = 4363431212
            };
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteOperation((RequestOperation)packet.Operation)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteKey(packet.Key)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteFlags(packet.Flags)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteExpires(packet.Expires)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteLength(packet.Value.Count)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteVersion(packet.Version)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteNoReply()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteValue(packet.Value)).Returns(m_mockPacketBuilder.Object);

            // Act
            m_writer.Store(packet, true);

            // Assert
        }

        [Theory]
        [InlineData(GetOperation.Get)]
        [InlineData(GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void Get(GetOperation operation)
        {
            // Arrange
            var key = new byte[] { 7 };
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteOperation((RequestOperation)operation)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteKey(key)).Returns(m_mockPacketBuilder.Object);

            // Act
            m_writer.Get(operation, key);

            // Assert
        }

        [Theory]
        [InlineData(GetOperation.Get)]
        [InlineData(GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void GetMany(GetOperation operation)
        {
            // Arrange
            var keys = new byte[][]
            {
                new byte[] { 7 },
                new byte[] { 8 }
            };
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteOperation((RequestOperation)operation)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteKey(keys[0])).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteKey(keys[1])).Returns(m_mockPacketBuilder.Object);

            // Act
            m_writer.GetMany(operation, keys);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void Version()
        {
            // Arrange
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteOperation(RequestOperation.Version)).Returns(m_mockPacketBuilder.Object);

            // Act
            m_writer.Version();

            // Assert
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(0, false)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void Flush(int delay, bool noreply)
        {
            // Arrange
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteOperation(RequestOperation.Flush)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteDelay(delay)).Returns(m_mockPacketBuilder.Object);

            if (noreply)
            {
                m_mockPacketBuilder.Setup(builder => builder.WriteNoReply()).Returns(m_mockPacketBuilder.Object);
            }

            // Act
            m_writer.Flush(delay, noreply);

            // Assert
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void Delete(bool noreply)
        {
            // Arrange
            var key = new byte[] { 7 };
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteOperation(RequestOperation.Delete)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteKey(key)).Returns(m_mockPacketBuilder.Object);

            if (noreply)
            {
                m_mockPacketBuilder.Setup(builder => builder.WriteNoReply()).Returns(m_mockPacketBuilder.Object);
            }

            // Act
            m_writer.Delete(key, noreply);

            // Assert
        }

        [Theory]
        [InlineData(10L, true)]
        [InlineData(-10L, false)]
        [Trait(Constants.TraitNames.Protocol, "TextCommandWriter")]
        public void Incr(long delta, bool noreply)
        {
            // Arrange
            var packet = new IncrPacket() 
            { 
                Key = new byte[] { 7 },
                Delta = delta,
                InitialValue = 1000L,
                Expires = 200
            };
            m_mockPacketBuilder.Setup(builder => builder.Reset()).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder
                .Setup(builder => builder.WriteOperation(delta >= 0L ? RequestOperation.Increment : RequestOperation.Decrement))
                .Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteKey(packet.Key)).Returns(m_mockPacketBuilder.Object);
            m_mockPacketBuilder
                .Setup(builder => builder.WriteDelta(Math.Abs(delta), 0L)).Returns(m_mockPacketBuilder.Object);

            if (noreply)
            {
                m_mockPacketBuilder.Setup(builder => builder.WriteNoReply()).Returns(m_mockPacketBuilder.Object);
            }

            // Act
            m_writer.Incr(packet, noreply);

            // Assert
        }
    }
}
