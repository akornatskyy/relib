using System;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class RunOnceTest : IDisposable
    {
        private static readonly TimeSpan g_period = TimeSpan.FromMinutes(10);

        private readonly Mock<ICache> m_mockCache;
        private readonly RunOnce m_runOnce;

        public RunOnceTest()
        {
            m_mockCache = new Mock<ICache>(MockBehavior.Strict);
            m_runOnce = new RunOnce(m_mockCache.Object, g_period);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockCache.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "RunOnce")]
        public void TryEnter()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Increment("c", 1L, 1L, g_period))
                .Returns(1L);

            // Act
            var succeed = m_runOnce.TryEnter("c");

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "RunOnce")]
        public void TryEnter_Next()
        {
            // Arrange            
            m_mockCache
                .Setup(cache => cache.Increment("c", 1L, 1L, g_period))
                .Returns(2L);

            // Act
            var succeed = m_runOnce.TryEnter("c");

            // Assert
            Assert.False(succeed);
        }
    }
}
