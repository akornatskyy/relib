using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Services;
using Xunit;
using Xunit.Extensions;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Tests.WorkItem
{
    public sealed class AbstractWorkItemTest : IDisposable
    {
        private readonly MockAbstractWorkItem m_workItem;
        private readonly Mock<IDependencyResolver> m_resolverMock;
        private readonly Mock<IUnitOfWork> m_unitOfWorkMock;
        private readonly Mock<IExceptionHandler> m_exceptionHandlerMock;

        public AbstractWorkItemTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            m_unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            m_exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);

            DependencyResolver.InitializeWith(m_resolverMock.Object);
            m_workItem = new MockAbstractWorkItem();
            m_workItem.ExceptionHandler = m_exceptionHandlerMock.Object;
            m_workItem.UnitOfWorkName = "TestUnitOfWork";
            m_resolverMock.Setup(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork")).Returns(m_unitOfWorkMock.Object);
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Dispose());
            Assert.NotNull(m_workItem.StateBag);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
            m_unitOfWorkMock.VerifyAll();
            m_exceptionHandlerMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_HasNoWork()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = false;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.False(hasMoreWork);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Once());
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once());
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Once());
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_HasWork()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.True(hasMoreWork);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_Throws_Error_But_Unhandled()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWorkThrows = true;
            m_exceptionHandlerMock.Setup(exceptionHandler => exceptionHandler.HandleException(It.IsAny<Exception>()))
                .Returns(false);

            // Act
            Assert.Throws<InvalidOperationException>(() => m_workItem.DoWork(() => false));

            // Assert
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(1));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(1));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_Throws_Error_And_Handled()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWorkThrows = true;
            m_exceptionHandlerMock.Setup(exceptionHandler => exceptionHandler.HandleException(It.IsAny<Exception>()))
                .Returns(true);

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.False(hasMoreWork);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(1));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(1));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_HasWork_But_Must_Shutdown()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());

            // Act
            var hasMoreWork = m_workItem.DoWork(() => true);

            // Assert
            Assert.False(hasMoreWork);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Once());
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Once());
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Once());
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_Rules_Is_Null()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_workItem.Rules2 = null;

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.True(hasMoreWork);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_Rules_Is_Empty()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_workItem.Rules2 = new Func2<bool>[] { };

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.True(hasMoreWork);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_OneRule_IsNot_Satisfied()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_workItem.Rules2 = new Func2<bool>[] { () => true, () => false };

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.True(hasMoreWork);
            Assert.False(m_workItem.AllRulesSatisfied);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_AllRulesSatisfied()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_workItem.Rules2 = new Func2<bool>[] { () => true, () => true };

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.True(hasMoreWork);
            Assert.True(m_workItem.AllRulesSatisfied);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_AllRulesSatisfied_Throws_Error_But_Unhandled()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_workItem.AllRulesSatisfiedThrows = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_exceptionHandlerMock.Setup(exceptionHandler => exceptionHandler.HandleException(It.IsAny<Exception>()))
                .Returns(false);
            m_workItem.Rules2 = new Func2<bool>[] { () => true, () => true };

            // Act
            Assert.Throws<InvalidOperationException>(() => m_workItem.DoWork(() => false));

            // Assert
            Assert.False(m_workItem.AllRulesSatisfied);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(1));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_AllRulesSatisfied_Throws_Error_And_Handled()
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_workItem.AllRulesSatisfiedThrows = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_exceptionHandlerMock.Setup(exceptionHandler => exceptionHandler.HandleException(It.IsAny<Exception>()))
                .Returns(true);
            m_workItem.Rules2 = new Func2<bool>[] { () => true, () => true };

            // Act
            var hasMoreWork = m_workItem.DoWork(() => false);

            // Assert
            Assert.True(hasMoreWork);
            Assert.False(m_workItem.AllRulesSatisfied);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [Trait(Constants.TraitNames.WorkItem, "AbstractWorkItem")]
        public void AcquireUnitOfWork_RulesSatisfied_But_Must_Shutdown(int shutdownStep)
        {
            // Arrange
            m_workItem.AcquiredUnitOfWork = true;
            m_unitOfWorkMock.Setup(unitOfWork => unitOfWork.Commit());
            m_workItem.Rules2 = new Func2<bool>[] { () => true, () => true };

            // Act
            var step = 1;
            var hasMoreWork = m_workItem.DoWork(() => step++ >= shutdownStep);

            // Assert
            Assert.False(hasMoreWork);
            Assert.False(m_workItem.AllRulesSatisfied);
            m_resolverMock.Verify(resolver => resolver.Resolve<IUnitOfWork>("TestUnitOfWork"), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Commit(), Times.Exactly(2));
            m_unitOfWorkMock.Verify(unitOfWork => unitOfWork.Dispose(), Times.Exactly(2));
        }
    }
}
