using System;
using System.Globalization;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class DefaultCacheTest : IDisposable
    {
        private readonly Mock<ICache> m_cacheMock;

        public DefaultCacheTest()
        {
            m_cacheMock = new Mock<ICache>(MockBehavior.Strict);

            DefaultCache.InitializeWith(m_cacheMock.Object);
        }
        
        #region IDisposable Members

        public void Dispose()
        {
            DefaultCache.Reset();
            m_cacheMock.VerifyAll();            
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Initialize_Twice_DoesThrow()
        {
            // Arrange

            // Act
            Assert.Throws<InvalidOperationException>(() => DefaultCache.InitializeWith(m_cacheMock.Object));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Clear()
        {
            // Arrange
            m_cacheMock.Setup(cache => cache.Clear()).Returns(true);

            // Act
            DefaultCache.Clear();

            // Assert
        }

        [Theory]
        [InlineData("key")]
        [InlineData((string)null)]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Get(string key)
        {
            // Arrange
            m_cacheMock.Setup(cache => cache.Get<int>(key)).Returns(100);

            // Act
            var value = DefaultCache.Get<int>(key);

            // Assert
            Assert.Equal(100, value);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Get_DataKey()
        {
            // Arrange
            var datakey = new DataKey<int>("key");
            m_cacheMock.Setup(cache => cache.Get(datakey)).Returns(true);

            // Act
            DefaultCache.Get(datakey);

            // Assert
        }

        [Theory]
        [InlineData("key", 100)]
        [InlineData((string)null, 100)]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Store(string key, int value)
        {
            // Arrange
            m_cacheMock.Setup(cache => cache.Store(key, value)).Callback<string, object>((k, v) => 
            {
                Assert.Equal(key, k);
                Assert.Equal(value, v);
            })
            .Returns(true);

            // Act
            Assert.DoesNotThrow(() => DefaultCache.Store(key, value));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Store_DataKey()
        {
            // Arrange
            var datakey = new DataKey<int>("key");
            m_cacheMock.Setup(cache => cache.Store(datakey)).Returns(true);

            // Act
            DefaultCache.Store(datakey);

            // Assert
        }

        [Theory]
        [InlineData("key", 101, "2/21/2010")]
        [InlineData((string)null, 100, "2/22/2010")]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void StoreExpiresAt(string key, int value, string expires)
        {
            // Arrange
            var expiresAt = DateTime.Parse(expires, CultureInfo.InvariantCulture);
            m_cacheMock.Setup(cache => cache.Store(key, value, expiresAt))
                .Callback<string, object, DateTime>((k, v, e) =>
                {
                    Assert.Equal(key, k);
                    Assert.Equal(value, v);
                    Assert.Equal(expiresAt, e);
                })
                .Returns(true);

            // Act
            Assert.DoesNotThrow(() => DefaultCache.Store(key, value, expiresAt));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void StoreExpiresAt_DataKey()
        {
            // Arrange
            var datakey = new DataKey<int>("key");
            var expires = DateTime.Now;
            m_cacheMock.Setup(cache => cache.Store(datakey, expires)).Returns(true);

            // Act
            DefaultCache.Store(datakey, expires);

            // Assert
        }

        [Theory]
        [InlineData("key", 101, "0:0:2.125")]
        [InlineData((string)null, 100, "0:3:0.0")]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void StoreValidFor(string key, int value, string valid)
        {
            // Arrange
            var validFor = TimeSpan.Parse(valid, CultureInfo.InvariantCulture);
            m_cacheMock.Setup(cache => cache.Store(key, value, validFor))
                .Callback<string, object, TimeSpan>((k, v, t) =>
                {
                    Assert.Equal(key, k);
                    Assert.Equal(value, v);
                    Assert.Equal(validFor, t);
                })
                .Returns(true);

            // Act
            Assert.DoesNotThrow(() => DefaultCache.Store(key, value, validFor));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void StoreValidFor_DataKey()
        {
            // Arrange
            var datakey = new DataKey<int>("key");
            var validFor = TimeSpan.FromMinutes(10);
            m_cacheMock.Setup(cache => cache.Store(datakey, validFor)).Returns(true);

            // Act
            DefaultCache.Store(datakey, validFor);

            // Assert
        }

        [Theory]
        [InlineData("key")]
        [InlineData((string)null)]
        [Trait(Constants.TraitNames.Caching, "DefaultCache")]
        public void Remove(string key)
        {
            // Arrange
            m_cacheMock.Setup(cache => cache.Remove(key)).Returns(true);

            // Act
            Assert.DoesNotThrow(() => DefaultCache.Remove(key));

            // Assert
        }
    }
}
