using System;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class OnePassScopeTest
    {
        private static readonly TimeSpan g_period = TimeSpan.FromMinutes(10);

        private readonly Mock<ICache> m_mockCache;
        private readonly OnePass m_onepass;

        public OnePassScopeTest()
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
        [Trait(Constants.TraitNames.Caching, "OnePassScope")]
        public void Aquired_True()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Increment("c", 1L, 1L, g_period))
                .Returns(1L);
            m_mockCache
                .Setup(cache => cache.Remove("c"))
                .Returns(true);

            // Act
            using (var scope = new OnePassScope(m_onepass, "c"))
            {
                // Assert
                Assert.True(scope.Aquired);
            }            
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "OnePassScope")]
        public void Aquired_False()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Increment("c", 1L, 1L, g_period))
                .Returns(-1L);

            // Act
            using (var scope = new OnePassScope(m_onepass, "c"))
            {
                // Assert
                Assert.False(scope.Aquired);
            }
        }
    }
}
