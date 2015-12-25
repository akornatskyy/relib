using System;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class CacheProxyTest : IDisposable
    {
        private readonly Mock<ICache> m_mockCache;
        private readonly CacheProxy m_cacheProxy;

        public CacheProxyTest()
        {
            m_mockCache = new Mock<ICache>(MockBehavior.Strict);
            m_cacheProxy = new CacheProxy(m_mockCache.Object);

            var datakey = new DataKey<int>("x")
            {
                Value = 100
            };
            m_mockCache.Setup(cache => cache.Store(datakey)).Returns(true);
            m_cacheProxy.Store(datakey);
            Assert.Equal(1, m_cacheProxy.Count);
            Assert.Equal(0, m_cacheProxy.MissCount);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockCache.VerifyAll();
            m_cacheProxy.Dispose();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Clear()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Clear())
                .Returns(true);

            // Act
            m_cacheProxy.Clear();

            // Assert
            Assert.Equal(0, m_cacheProxy.Count);
            Assert.Equal(0, m_cacheProxy.MissCount);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Get_Existing()
        {
            // Arrange            

            // Act
            var result = m_cacheProxy.Get<int>("x");

            // Assert
            Assert.Equal(100, result);
            Assert.Equal(0, m_cacheProxy.MissCount);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Get_Inner_Exist()
        {
            // Arrange
            var datakey = new DataKey<int>("k");
            m_mockCache.Setup(cache => cache.Get(datakey))
                .Callback<DataKey<int>>(dk => { dk.Value = 765; })
                .Returns(true);

            // Act
            m_cacheProxy.Get(datakey);

            // Assert
            Assert.Equal(765, datakey.Value);
            Assert.Equal(2, m_cacheProxy.Count);
            Assert.Equal(0, m_cacheProxy.MissCount);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Get_Inner_Missed()
        {
            // Arrange
            var datakey = new DataKey<int>("k");
            m_mockCache.Setup(cache => cache.Get(datakey)).Returns(true);

            // Act
            m_cacheProxy.Get(datakey);

            // Assert
            Assert.False(datakey.HasValue);
            Assert.Equal(1, m_cacheProxy.Count);
            Assert.Equal(1, m_cacheProxy.MissCount);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Store()
        {
            // Arrange
            var datakey = new DataKey<int>("k")
            {
                Value = 765
            };
            m_mockCache.Setup(cache => cache.Store(datakey)).Returns(true);

            // Act
            m_cacheProxy.Store(datakey);

            // Assert
            Assert.Equal(2, m_cacheProxy.Count);
            var datakey2 = new DataKey<int>(datakey.Key);
            m_cacheProxy.Get(datakey2);
            Assert.Equal(datakey.Value, datakey2.Value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Store_ExpiresAt()
        {
            // Arrange
            var datakey = new DataKey<int>("k")
            {
                Value = 765
            };
            var expiresAt = DateTime.UtcNow;
            m_mockCache.Setup(cache => cache.Store(datakey, expiresAt)).Returns(true);

            // Act
            m_cacheProxy.Store(datakey, expiresAt);

            // Assert
            Assert.Equal(2, m_cacheProxy.Count);
            var datakey2 = new DataKey<int>(datakey.Key);
            m_cacheProxy.Get(datakey2);
            Assert.Equal(datakey.Value, datakey2.Value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Store_ValidFor()
        {
            // Arrange
            var datakey = new DataKey<int>("k")
            {
                Value = 765
            };
            var validFor = TimeSpan.FromSeconds(10);
            m_mockCache.Setup(cache => cache.Store(datakey, validFor)).Returns(true);

            // Act
            m_cacheProxy.Store(datakey, validFor);

            // Assert
            Assert.Equal(2, m_cacheProxy.Count);
            var datakey2 = new DataKey<int>(datakey.Key);
            m_cacheProxy.Get(datakey2);
            Assert.Equal(datakey.Value, datakey2.Value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheProxy")]
        public void Remove()
        {
            // Arrange
            m_mockCache.Setup(cache => cache.Remove("x")).Returns(true);

            // Act
            m_cacheProxy.Remove("x");

            // Assert
            Assert.Equal(0, m_cacheProxy.Count);
        }
    }
}
