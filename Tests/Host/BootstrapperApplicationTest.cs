using System;
using System.Globalization;
using Moq;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Tracing;
using Xunit;

namespace ReusableLibrary.Host.Tests
{
    public sealed class BootstrapperApplicationTest : IDisposable
    {
        private readonly MockBootstrapperApplication m_bootstrapperApplication;
        private readonly Mock<IDependencyResolver> m_resolverMock;
        private readonly Mock<IExceptionHandler> m_exceptionHandlerMock;

        public BootstrapperApplicationTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            m_exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);

            DependencyResolver.InitializeWith(m_resolverMock.Object);
            m_bootstrapperApplication = new MockBootstrapperApplication();
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_bootstrapperApplication.Dispose();
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
            m_exceptionHandlerMock.VerifyAll();            
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void Name()
        {
            // Arrange

            // Act
            var name = m_bootstrapperApplication.Name;

            // Assert
            Assert.Equal("Mock", name);
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void Version()
        {
            // Arrange
            var expected = String.Format(CultureInfo.InvariantCulture, "{0} v{1}", m_bootstrapperApplication.Name, AssemblyHelper.ToLongVersionString(GetType().Assembly));

            // Act
            var version = m_bootstrapperApplication.Version;

            // Assert
            Assert.Equal(expected, version);
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void CanRun()
        {
            // Arrange

            // Act
            var result = m_bootstrapperApplication.CanRun();

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void Run()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IStartupTask>()).Returns(new IStartupTask[] { });
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IShutdownTask>()).Returns(new IShutdownTask[] { });
            m_resolverMock.Setup(resolver => resolver.Dispose());

            // Act
            m_bootstrapperApplication.Run();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void OnStart()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IStartupTask>()).Returns(new IStartupTask[] { });

            // Act
            m_bootstrapperApplication.OnStart();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void OnStop()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IShutdownTask>()).Returns(new IShutdownTask[] { });
            m_resolverMock.Setup(resolver => resolver.Dispose());

            // Act
            m_bootstrapperApplication.OnStop();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void OnStart_Error()
        {
            // Arrange
            m_bootstrapperApplication.RaiseErrorOnStart = true;
            var exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Resolve<IExceptionHandler>()).Returns(exceptionHandlerMock.Object);
            exceptionHandlerMock.Setup(handler => handler.HandleException(It.IsAny<InvalidOperationException>())).Returns(true);
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IShutdownTask>()).Returns(new IShutdownTask[] { });
            m_resolverMock.Setup(resolver => resolver.Dispose());

            // Act
            Assert.DoesNotThrow(() => m_bootstrapperApplication.Run());

            // Assert
            exceptionHandlerMock.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "BootstrapperApplication")]
        public void OnStop_Error()
        {
            // Arrange
            m_bootstrapperApplication.RaiseErrorOnStop = true;
            m_resolverMock.Setup(resolver => resolver.ResolveAll<IStartupTask>()).Returns(new IStartupTask[] { });
            var exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Resolve<IExceptionHandler>()).Returns(exceptionHandlerMock.Object);
            exceptionHandlerMock.Setup(handler => handler.HandleException(It.IsAny<InvalidOperationException>())).Returns(true);

            // Act
            Assert.DoesNotThrow(() => m_bootstrapperApplication.Run());

            // Assert
            exceptionHandlerMock.VerifyAll();
        }
    }
}
