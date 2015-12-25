using System;
using System.Collections.Generic;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using ReusableLibrary.Memcached.Protocol;
using ReusableLibrary.Memcached.Tests.Infrastructure;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class DefaultProtocolTest : IDisposable
    {
        private readonly Mock<IProtocolFactory> m_mockFactory;
        private readonly Mock<IEncoder> m_mockEncoder;
        private readonly Mock<IPacketBuilder> m_mockPacketBuilder;
        private readonly Mock<IObjectFormatter> m_mockObjectFormatter;
        private readonly Mock<ICommandWriter> m_mockCommandWriter;
        private readonly Mock<IClientConnection> m_mockClientConnection;
        private readonly Mock<IBinaryWriter> m_mockBinaryWriter;
        private readonly Mock<IBinaryReader> m_mockBinaryReader;
        private readonly Mock<IPacketParser> m_mockPacketParser;
        private readonly Mock<ICommandReader> m_mockCommandReader;

        public DefaultProtocolTest()
        {
            m_mockFactory = new Mock<IProtocolFactory>(MockBehavior.Strict);
            m_mockEncoder = new Mock<IEncoder>(MockBehavior.Strict);
            m_mockPacketBuilder = new Mock<IPacketBuilder>(MockBehavior.Strict);
            m_mockObjectFormatter = new Mock<IObjectFormatter>(MockBehavior.Strict);
            m_mockCommandWriter = new Mock<ICommandWriter>(MockBehavior.Strict);
            m_mockClientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
            m_mockBinaryWriter = new Mock<IBinaryWriter>(MockBehavior.Strict);
            m_mockBinaryReader = new Mock<IBinaryReader>(MockBehavior.Strict);
            m_mockPacketParser = new Mock<IPacketParser>(MockBehavior.Strict);
            m_mockCommandReader = new Mock<ICommandReader>(MockBehavior.Strict);

            m_mockFactory.Setup(factory => factory.CreateEncoder()).Returns(m_mockEncoder.Object);
            m_mockFactory.Setup(factory => factory.CreatePacketBuilder(It.IsAny<Buffer<byte>>())).Returns(m_mockPacketBuilder.Object);
            m_mockFactory.Setup(factory => factory.CreateObjectFormatter()).Returns(m_mockObjectFormatter.Object);
            m_mockFactory.Setup(factory => factory.CreateCommandWriter(m_mockPacketBuilder.Object)).Returns(m_mockCommandWriter.Object);
        }

        #region PropertyData Input

        public static IEnumerable<object[]> StoreInput
        {
            get { return DomainModelFactory.StoreInputSequence(); }
        }

        public static IEnumerable<object[]> DeleteInput
        {
            get { return DomainModelFactory.DeleteInputSequence(); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            m_mockFactory.VerifyAll();
            m_mockEncoder.VerifyAll();
            m_mockPacketBuilder.VerifyAll();
            m_mockObjectFormatter.VerifyAll();
            m_mockCommandWriter.VerifyAll();
            m_mockClientConnection.VerifyAll();
            m_mockBinaryWriter.VerifyAll();
            m_mockBinaryReader.VerifyAll();
            m_mockPacketParser.VerifyAll();
            m_mockCommandReader.VerifyAll();
        }

        #endregion

        [Theory]
        [PropertyData("StoreInput")]
        [Trait(Constants.TraitNames.Protocol, "DefaultProtocol")]
        public void Store(StoreOperation operation, string optionsString, bool connectSucceed, bool storeSucceed)
        {
            // Arrange
            var options = new ProtocolOptions(optionsString);
            var protocol = new DefaultProtocol(m_mockFactory.Object, options);
            var datakey = new DataKey<int>("Key1")
            {
                Value = 12345,
                Version = 556
            };
            int flags = 10101;
            var key = new byte[] { };
            var value = new ArraySegment<byte>(new byte[] { });
            StorePacket packet = null;
            m_mockObjectFormatter.Setup(formatter => formatter.Serialize<int>(12345, out flags)).Returns(value);
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key1")).Returns(key);
            m_mockCommandWriter.Setup(writer => writer.Store(It.IsAny<StorePacket>(), options.NoReply))
                .Callback<StorePacket, bool>((p, nr) => packet = p);
            m_mockFactory.Setup(factory => factory.Context(key))
                .Returns<byte[]>(state => (Action2<IClientConnection, object> action) =>
                {
                    if (connectSucceed)
                    {
                        action(m_mockClientConnection.Object, state);
                    }

                    return connectSucceed;
                });
            if (connectSucceed)
            {
                m_mockClientConnection.SetupGet(connection => connection.Writer).Returns(m_mockBinaryWriter.Object);
                m_mockPacketBuilder.Setup(builder => builder.WriteTo(m_mockBinaryWriter.Object)).Returns(0);
                if (!options.NoReply)
                {
                    m_mockClientConnection.SetupGet(connection => connection.Reader).Returns(m_mockBinaryReader.Object);
                    m_mockFactory.Setup(factory => factory.CreatePacketParser(m_mockBinaryReader.Object, It.IsAny<Buffer<byte>>())).Returns(m_mockPacketParser.Object);
                    m_mockFactory.Setup(factory => factory.CreateCommandReader(m_mockPacketParser.Object)).Returns(m_mockCommandReader.Object);
                    m_mockCommandReader.Setup(reader => reader.ReadStored()).Returns(storeSucceed);
                }
            }

            // Act
            var result = protocol.Store(operation, datakey, 100);

            // Assert
            Assert.Equal(connectSucceed && (options.NoReply || storeSucceed), result);
            Assert.NotNull(packet);
            Assert.Equal(100, packet.Expires);
            Assert.Equal(flags, packet.Flags);
            Assert.Equal(key, packet.Key);
            Assert.Equal(operation, packet.Operation);
            Assert.Equal(value, packet.Value);
            Assert.Equal(556, packet.Version);
        }

        [Theory]
        [InlineData(GetOperation.Get)]
        [InlineData(GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "DefaultProtocol")]
        public void Get_NotFound(GetOperation operation)
        {
            // Arrange
            var options = new ProtocolOptions();
            var protocol = new DefaultProtocol(m_mockFactory.Object, options);
            var datakey = new DataKey<int>("Key1");
            var key = new byte[] { };
            ValuePacket packet = null;
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key1")).Returns(key);
            m_mockCommandWriter.Setup(writer => writer.Get(operation, key));
            m_mockFactory.Setup(factory => factory.Context(key))
                .Returns<byte[]>(state => (Action2<IClientConnection, object> action) =>
                {
                    action(m_mockClientConnection.Object, state);
                    return true;
                });
            m_mockClientConnection.SetupGet(connection => connection.Writer).Returns(m_mockBinaryWriter.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteTo(m_mockBinaryWriter.Object)).Returns(0);
            m_mockClientConnection.SetupGet(connection => connection.Reader).Returns(m_mockBinaryReader.Object);
            m_mockFactory.Setup(factory => factory.CreatePacketParser(m_mockBinaryReader.Object, It.IsAny<Buffer<byte>>())).Returns(m_mockPacketParser.Object);
            m_mockFactory.Setup(factory => factory.CreateCommandReader(m_mockPacketParser.Object)).Returns(m_mockCommandReader.Object);
            m_mockCommandReader.Setup(reader => reader.ReadValue(operation == GetOperation.Gets)).Returns(packet);

            // Act
            var result = protocol.Get(operation, datakey);

            // Assert
            Assert.True(result);
            Assert.False(datakey.HasValue);
        }

        [Theory]
        [InlineData(GetOperation.Get)]
        [InlineData(GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "DefaultProtocol")]
        public void Get(GetOperation operation)
        {
            // Arrange
            var options = new ProtocolOptions();
            var protocol = new DefaultProtocol(m_mockFactory.Object, options);
            var datakey = new DataKey<int>("Key1");
            var key = new byte[] { };
            var packet = new ValuePacket()
            {
                Flags = 101,
                Key = key,
                Value = new ArraySegment<byte>(new byte[] { }),
                Version = 245
            };
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key1")).Returns(key);
            m_mockCommandWriter.Setup(writer => writer.Get(operation, key));
            m_mockFactory.Setup(factory => factory.Context(key))
                .Returns<byte[]>(state => (Action2<IClientConnection, object> action) =>
                {
                    action(m_mockClientConnection.Object, state);
                    return true;
                });
            m_mockClientConnection.SetupGet(connection => connection.Writer).Returns(m_mockBinaryWriter.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteTo(m_mockBinaryWriter.Object)).Returns(0);
            m_mockClientConnection.SetupGet(connection => connection.Reader).Returns(m_mockBinaryReader.Object);
            m_mockFactory.Setup(factory => factory.CreatePacketParser(m_mockBinaryReader.Object, It.IsAny<Buffer<byte>>())).Returns(m_mockPacketParser.Object);
            m_mockFactory.Setup(factory => factory.CreateCommandReader(m_mockPacketParser.Object)).Returns(m_mockCommandReader.Object);
            m_mockCommandReader.Setup(reader => reader.ReadValue(operation == GetOperation.Gets)).Returns(packet);
            m_mockObjectFormatter.Setup(formatter => formatter.Deserialize<int>(packet.Value, packet.Flags)).Returns(507);
            m_mockCommandReader.Setup(reader => reader.ReadSucceed());

            // Act
            var result = protocol.Get(operation, datakey);

            // Assert
            Assert.True(result);
            Assert.True(datakey.HasValue);
            Assert.Equal(245, datakey.Version);
            Assert.Equal(507, datakey.Value);
        }

        [Theory]
        [InlineData(GetOperation.Get)]
        [InlineData(GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "DefaultProtocol")]
        public void GetMany_NotFound(GetOperation operation)
        {
            // Arrange
            var options = new ProtocolOptions();
            var protocol = new DefaultProtocol(m_mockFactory.Object, options);
            var datakey1 = new DataKey<int>("Key1");
            var datakey2 = new DataKey<bool>("Key2");
            var key1 = new byte[] { 1 };
            var key2 = new byte[] { 2 };
            byte[][] keys = null;
            ValuePacket packet = null;
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key1")).Returns(key1);
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key2")).Returns(key2);
            m_mockFactory.Setup(factory => factory.Context(It.IsAny<byte[][]>()))
                .Returns<byte[][]>(state => (Action2<IClientConnection, object> action) =>
                {
                    keys = (byte[][])state;
                    action(m_mockClientConnection.Object, state);
                    return true;
                });
            m_mockCommandWriter.Setup(writer => writer.GetMany(operation, It.IsAny<byte[][]>()));
            m_mockClientConnection.SetupGet(connection => connection.Writer).Returns(m_mockBinaryWriter.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteTo(m_mockBinaryWriter.Object)).Returns(0);
            m_mockClientConnection.SetupGet(connection => connection.Reader).Returns(m_mockBinaryReader.Object);
            m_mockFactory.Setup(factory => factory.CreatePacketParser(m_mockBinaryReader.Object, It.IsAny<Buffer<byte>>())).Returns(m_mockPacketParser.Object);
            m_mockFactory.Setup(factory => factory.CreateCommandReader(m_mockPacketParser.Object)).Returns(m_mockCommandReader.Object);
            m_mockCommandReader.Setup(reader => reader.ReadValue(operation == GetOperation.Gets)).Returns(packet);

            // Act
            var result = protocol.GetMany(operation, new DataKey[] { datakey1, datakey2 });

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(GetOperation.Get)]
        [InlineData(GetOperation.Gets)]
        [Trait(Constants.TraitNames.Protocol, "DefaultProtocol")]
        public void GetMany(GetOperation operation)
        {
            // Arrange
            var options = new ProtocolOptions();
            var protocol = new DefaultProtocol(m_mockFactory.Object, options);
            var datakey1 = new DataKey<int>("Key1");
            var datakey2 = new DataKey<bool>("Key2");
            var key1 = new byte[] { 1 };
            var key2 = new byte[] { 2 };
            byte[][] keys = null;
            var packet = new ValuePacket()
            {
                Flags = 101,
                Key = key1,
                Value = new ArraySegment<byte>(new byte[] { }),
                Version = 245
            };
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key1")).Returns(key1);
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key2")).Returns(key2);
            m_mockFactory.Setup(factory => factory.Context(It.IsAny<byte[][]>()))
                .Returns<byte[][]>(state => (Action2<IClientConnection, object> action) =>
                {
                    keys = (byte[][])state;
                    action(m_mockClientConnection.Object, state);
                    return true;
                });
            m_mockCommandWriter.Setup(writer => writer.GetMany(operation, It.IsAny<byte[][]>()));
            m_mockClientConnection.SetupGet(connection => connection.Writer).Returns(m_mockBinaryWriter.Object);
            m_mockPacketBuilder.Setup(builder => builder.WriteTo(m_mockBinaryWriter.Object)).Returns(0);
            m_mockClientConnection.SetupGet(connection => connection.Reader).Returns(m_mockBinaryReader.Object);
            m_mockFactory.Setup(factory => factory.CreatePacketParser(m_mockBinaryReader.Object, It.IsAny<Buffer<byte>>())).Returns(m_mockPacketParser.Object);
            m_mockFactory.Setup(factory => factory.CreateCommandReader(m_mockPacketParser.Object)).Returns(m_mockCommandReader.Object);
            m_mockCommandReader.Setup(reader => reader.ReadValue(operation == GetOperation.Gets)).Returns(() =>
            {
                var p = packet;
                if (packet != null)
                {
                    packet = null;
                }

                return p;
            });
            m_mockObjectFormatter.Setup(formatter => formatter.Deserialize<int>(packet.Value, packet.Flags)).Returns(507);

            // Act
            var result = protocol.GetMany(operation, new DataKey[] { datakey1, datakey2 });

            // Assert
            Assert.True(result);
            Assert.True(datakey1.HasValue);
            Assert.Equal(245, datakey1.Version);
            Assert.Equal(507, datakey1.Value);
            Assert.False(datakey2.HasValue);
        }

        [Theory]
        [PropertyData("DeleteInput")]
        [Trait(Constants.TraitNames.Protocol, "DefaultProtocol")]
        public void Delete(string optionsString, bool connectSucceed, bool deleteSucceed)
        {
            // Arrange
            var options = new ProtocolOptions(optionsString);
            var protocol = new DefaultProtocol(m_mockFactory.Object, options);
            var key = new byte[] { };
            m_mockEncoder.Setup(encoder => encoder.GetBytes("Key1")).Returns(key);
            m_mockCommandWriter.Setup(writer => writer.Delete(key, options.NoReply));
            m_mockFactory.Setup(factory => factory.Context(key))
                .Returns<byte[]>(state => (Action2<IClientConnection, object> action) =>
                {
                    if (connectSucceed)
                    {
                        action(m_mockClientConnection.Object, state);
                    }

                    return connectSucceed;
                });
            if (connectSucceed)
            {
                m_mockClientConnection.SetupGet(connection => connection.Writer).Returns(m_mockBinaryWriter.Object);
                m_mockPacketBuilder.Setup(builder => builder.WriteTo(m_mockBinaryWriter.Object)).Returns(0);
                if (!options.NoReply)
                {
                    m_mockClientConnection.SetupGet(connection => connection.Reader).Returns(m_mockBinaryReader.Object);
                    m_mockFactory.Setup(factory => factory.CreatePacketParser(m_mockBinaryReader.Object, It.IsAny<Buffer<byte>>())).Returns(m_mockPacketParser.Object);
                    m_mockFactory.Setup(factory => factory.CreateCommandReader(m_mockPacketParser.Object)).Returns(m_mockCommandReader.Object);
                    m_mockCommandReader.Setup(reader => reader.ReadDeleted()).Returns(deleteSucceed);
                }
            }

            // Act
            var result = protocol.Delete("Key1");

            // Assert
            Assert.Equal(connectSucceed && (options.NoReply || deleteSucceed), result);
        }
    }
}
