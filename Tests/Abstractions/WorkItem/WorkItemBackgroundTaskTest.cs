using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.WorkItem;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.WorkItem
{
    public sealed class WorkItemBackgroundTaskTest : IDisposable
    {
        private readonly WorkItemBackgroundTask m_task;
        private readonly Mock<IDependencyResolver> m_resolverMock;
        private readonly Mock<IWorkItem> m_workItemMock;

        public WorkItemBackgroundTaskTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());

            DependencyResolver.InitializeWith(m_resolverMock.Object);
            m_workItemMock = new Mock<IWorkItem>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Resolve<IWorkItem>("TestWorkItem")).Returns(m_workItemMock.Object);
            m_task = new WorkItemBackgroundTask("TestWorkItem");
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
            m_workItemMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "WorkItemBackgroundTask")]
        public void DoWork_Task_Succeed()
        {
            // Arrange
            Func2<bool> shutdownCallback = () => false;
            m_workItemMock.Setup(workItem => workItem.DoWork(shutdownCallback)).Returns(true);

            // Act
            var hasMoreWork = m_task.DoWork(shutdownCallback);

            // Assert
            Assert.True(hasMoreWork);
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "WorkItemBackgroundTask")]
        public void DoWork_Task_DidNot_Succeed()
        {
            // Arrange
            Func2<bool> shutdownCallback = () => false;
            m_workItemMock.Setup(workItem => workItem.DoWork(shutdownCallback)).Returns(false);

            // Act
            var hasMoreWork = m_task.DoWork(shutdownCallback);

            // Assert
            Assert.False(hasMoreWork);
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "WorkItemBackgroundTask")]
        public void DoWork_Task_Succeed_But_Must_Shutdown()
        {
            // Arrange
            Func2<bool> shutdownCallback = () => true;
            m_workItemMock.Setup(workItem => workItem.DoWork(shutdownCallback)).Returns(true);

            // Act
            var hasMoreWork = m_task.DoWork(shutdownCallback);

            // Assert
            Assert.False(hasMoreWork);
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "WorkItemBackgroundTask")]
        public void DoWork_Task_DidNot_Succeed_And_Must_Shutdown()
        {
            // Arrange
            Func2<bool> shutdownCallback = () => true;
            m_workItemMock.Setup(workItem => workItem.DoWork(shutdownCallback)).Returns(false);

            // Act
            var hasMoreWork = m_task.DoWork(shutdownCallback);

            // Assert
            Assert.False(hasMoreWork);
        }
    }
}
