using System;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class OnePassTest : IDisposable
    {
        private static readonly TimeSpan g_period = TimeSpan.FromMinutes(10);

        private readonly Mock<ICache> m_mockCache;
        private readonly OnePass m_onepass;

        public OnePassTest()
        {
            m_mockCache = new Mock<ICache>(MockBehavior.Strict);
            m_onepass = new OnePass(m_mockCache.Object, g_period);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockCache.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "OnePass")]
        public void TryEnter()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Increment("c", 1L, 1L, g_period))
                .Returns(1L);

            // Act
            var succeed = m_onepass.TryEnter("c");

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "OnePass")]
        public void TryEnter_Fails()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Increment("c", 1L, 1L, g_period))
                .Returns(-1L);

            // Act
            var succeed = m_onepass.TryEnter("c");

            // Assert
            Assert.False(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "OnePass")]
        public void TryLeave()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Remove("c"))
                .Returns(true);

            // Act
            var succeed = m_onepass.TryLeave("c");

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "OnePass")]
        public void TryLeave_Fails()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Remove("c"))
                .Returns(false);

            // Act
            var succeed = m_onepass.TryLeave("c");

            // Assert
            Assert.False(succeed);
        }
    }
}
