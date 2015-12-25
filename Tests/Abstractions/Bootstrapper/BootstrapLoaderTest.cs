using System;
using Moq;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.IoC;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Bootstrapper
{
    public sealed class BootstrapLoaderTest : IDisposable
    {
        private readonly Mock<IDependencyResolver> m_resolverMock;

        public BootstrapLoaderTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            DependencyResolver.InitializeWith(m_resolverMock.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Bootstrapper, "BootstrapLoader")]
        public void Start()
        {
            // Arrange
            var task = new Mock<IStartupTask>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IStartupTask>()).Returns(new IStartupTask[] 
            { 
                task.Object
            });
            task.Setup(t => t.Execute());

            // Act
            BootstrapLoader.Start();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Bootstrapper, "BootstrapLoader")]
        public void End()
        {
            // Arrange
            var task = new Mock<IShutdownTask>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IShutdownTask>()).Returns(new IShutdownTask[] 
            { 
                task.Object
            });
            m_resolverMock.Setup(resolver => resolver.Dispose());
            task.Setup(t => t.Execute());

            // Act
            BootstrapLoader.End();

            // Assert
        }
    }
}
