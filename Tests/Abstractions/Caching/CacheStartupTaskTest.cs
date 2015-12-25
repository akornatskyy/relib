using System;
using Moq;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.IoC;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class CacheStartupTaskTest : IDisposable
    {
        private readonly Mock<IDependencyResolver> m_resolverMock;
        private readonly Mock<ICache> m_cacheMock;
        private readonly CacheStartupTask m_task;

        public CacheStartupTaskTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            m_cacheMock = new Mock<ICache>(MockBehavior.Strict);
            DependencyResolver.InitializeWith(m_resolverMock.Object);

            m_task = new CacheStartupTask();
        }

        #region IDisposable Members

        public void Dispose()
        {
            DefaultCache.Reset();
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
            m_cacheMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheStartupTask")]
        public void Execute()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.Resolve<ICache>()).Returns(m_cacheMock.Object);

            // Act
            Assert.DoesNotThrow(() => (m_task as IStartupTask).Execute());

            // Assert
        }
    }
}
