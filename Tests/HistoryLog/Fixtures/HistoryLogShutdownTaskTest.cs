using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.WorkItem;
using ReusableLibrary.HistoryLog.WorkItem;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public sealed class HistoryLogShutdownTaskTest : IDisposable
    {
        private readonly HistoryLogShutdownTask m_task;
        private readonly Mock<IDependencyResolver> m_mockDependencyResolver;
        private readonly Mock<IWorkItem> m_mockWorkItem;

        public HistoryLogShutdownTaskTest()
        {
            m_task = new HistoryLogShutdownTask("MyWorkItem");
            m_mockDependencyResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_mockDependencyResolver
                .Setup(resolver => resolver.Dispose());
            DependencyResolver.InitializeWith(m_mockDependencyResolver.Object);
            m_mockWorkItem = new Mock<IWorkItem>(MockBehavior.Strict);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_mockDependencyResolver.VerifyAll();            
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "HistoryLogShutdownTask")]
        public void Execute()
        {
            // Arrange
            m_mockDependencyResolver
                .Setup(resolver => resolver.Resolve<IWorkItem>("MyWorkItem"))
                .Returns(m_mockWorkItem.Object);
            m_mockWorkItem
                .Setup(workItem => workItem.DoWork())
                .Returns(true);

            // Act
            m_task.Execute();

            // Assert
        }
    }
}
