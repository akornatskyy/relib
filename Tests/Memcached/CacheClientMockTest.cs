using System;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Memcached.Protocol;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests
{
    public sealed class CacheClientMockTest : IDisposable
    {
        private readonly Mock<IPool<IProtocol>> m_mockPool;
        private readonly Mock<IProtocol> m_mockProtocol;
        private readonly Mock<IProtocolFactory> m_mockProtocolFactory;
        private readonly CacheClient m_client;

        public CacheClientMockTest()
        {
            m_mockPool = new Mock<IPool<IProtocol>>(MockBehavior.Strict);
            m_mockProtocol = new Mock<IProtocol>(MockBehavior.Strict);
            m_mockProtocolFactory = new Mock<IProtocolFactory>(MockBehavior.Strict);

            m_mockPool.Setup(pool => pool.Take(null)).Returns(m_mockProtocol.Object);
            m_mockPool.Setup(pool => pool.Return(m_mockProtocol.Object)).Returns(true);
            m_mockProtocolFactory.Setup(factory => factory.AquireProtocol()).Returns(new Pooled<IProtocol>(m_mockPool.Object));

            m_client = new CacheClient(m_mockProtocolFactory.Object);
        }

        [Theory]
        [InlineData(600, "00:10:00")]
        [InlineData(55, "00:00:55")]
        [InlineData(4065, "01:07:45")]
        [InlineData(2592000, "30.00:00:00")]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public static void GetExpires_ValidFor(int expected, string expires)
        {
            // Arrange
            var validFor = TimeSpan.Parse(expires);

            // Act
            var result = CacheClient.GetExpires(validFor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("30.00:00:01")]
        [InlineData("00:00:00.9")]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public static void GetExpires_ValidFor_OutOfRange(string expires)
        {
            // Arrange
            var validFor = TimeSpan.Parse(expires);

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => CacheClient.GetExpires(validFor));

            // Assert
        }

        [Theory]
        [InlineData(1286399400, "00:10:00")]
        [InlineData(1286398855, "00:00:55")]
        [InlineData(1286402865, "01:07:45")]
        [InlineData(1288990800, "30.00:00:00")]
        [InlineData(1288990801, "30.00:00:01")]
        [InlineData(1286398800, "00:00:00.9")]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public static void GetExpires_ExpiresAt(int expected, string expires)
        {
            // Arrange
            var expiresAt = new DateTime(2010, 10, 6, 21, 0, 0, DateTimeKind.Utc).ToUniversalTime().Add(TimeSpan.Parse(expires));

            // Act
            var result = CacheClient.GetExpires(expiresAt);

            // Assert
            Assert.Equal(expected, result);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_client.Dispose();
            m_mockPool.VerifyAll();
            m_mockProtocol.VerifyAll();
            m_mockProtocolFactory.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public void Clear_NotImplemented()
        {
            // Arrange

            // Act
            Assert.Throws<NotImplementedException>(() => m_client.Clear());

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public void Get()
        {
            // Arrange
            var datakey = new DataKey<int>("Key1");
            m_mockProtocol.Setup(protocol => protocol.Get(GetOperation.Get, datakey)).Returns(true);

            // Act
            m_client.Get(datakey);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public void Store()
        {
            // Arrange
            var datakey = new DataKey<int>("Key1");
            m_mockProtocol.Setup(protocol => protocol.Store(StoreOperation.Set, datakey, 0)).Returns(true);

            // Act
            m_client.Store(datakey);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public void Store_ExpiresAt()
        {
            // Arrange
            var expiresAt = DateTime.Now;
            var datakey = new DataKey<int>("Key1");
            m_mockProtocol.Setup(protocol => protocol.Store(StoreOperation.Set, datakey, CacheClient.GetExpires(expiresAt))).Returns(true);

            // Act
            m_client.Store(datakey, expiresAt);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public void Store_ValidFor()
        {
            // Arrange
            var validFor = TimeSpan.FromMinutes(10);
            var datakey = new DataKey<int>("Key1");
            m_mockProtocol.Setup(protocol => protocol.Store(StoreOperation.Set, datakey, CacheClient.GetExpires(validFor))).Returns(true);

            // Act
            m_client.Store(datakey, validFor);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Protocol, "CacheClient")]
        public void Remove()
        {
            // Arrange
            m_mockProtocol.Setup(protocol => protocol.Delete("Key1")).Returns(true);

            // Act
            m_client.Remove("Key1");

            // Assert
        }
    }
}
