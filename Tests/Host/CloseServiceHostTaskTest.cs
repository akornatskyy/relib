﻿using System;
using System.Reflection;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Tracing;
using Xunit;

namespace ReusableLibrary.Host.Tests
{
    public sealed class CloseServiceHostTaskTest : IDisposable
    {
        private readonly Mock<IServiceHost> m_host;
        private readonly Mock<IDependencyResolver> m_resolverMock;
        private readonly Mock<IExceptionHandler> m_exceptionHandlerMock;
        private readonly CloseServiceHostTask m_task;

        public CloseServiceHostTaskTest()
        {
            m_host = new Mock<IServiceHost>(MockBehavior.Strict);
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            m_exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);

            DependencyResolver.InitializeWith(m_resolverMock.Object);            
            m_task = new CloseServiceHostTask(m_host.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_host.VerifyAll();
            m_resolverMock.VerifyAll();
            m_exceptionHandlerMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Application, "CloseServiceHostTask")]
        public void Execute_Close()
        {
            // Arrange
            m_host.Setup(host => host.Close());

            // Act
            m_task.Execute();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "CloseServiceHostTask")]
        public void Execute_Close_Throws_SystemException()
        {
            // Arrange
            var ex = new InvalidOperationException();
            m_host.Setup(host => host.Close()).Callback(() => { throw ex; });
            m_resolverMock.Setup(resolver => resolver.Resolve<IExceptionHandler>()).Returns(m_exceptionHandlerMock.Object);
            m_exceptionHandlerMock.Setup(handler => handler.HandleException(ex)).Returns(true);

            // Act
            Assert.DoesNotThrow(() => m_task.Execute());

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Application, "CloseServiceHostTask")]
        public void Execute_Close_Throws_Exception()
        {
            // Arrange
            m_host.Setup(host => host.Close()).Callback(() => { throw new TargetInvocationException(new InvalidOperationException()); });

            // Act
            Assert.Throws<TargetInvocationException>(() => m_task.Execute());

            // Assert
        }
    }
}
