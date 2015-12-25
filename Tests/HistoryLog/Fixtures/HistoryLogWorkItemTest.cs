using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Services;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.HistoryLog.Repository;
using ReusableLibrary.HistoryLog.WorkItem;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public sealed class HistoryLogWorkItemTest : IDisposable
    {
        private readonly HistoryLogWorkItem m_workItem;
        private readonly Mock<IHistoryLogRepository> m_mockHistoryLogRepository;
        private readonly Mock<IDependencyResolver> m_mockDependencyResolver;
        private readonly Mock<IUnitOfWork> m_mockUnitOfWork;
        private readonly HistoryLogQueue m_historyLogQueue;

        public HistoryLogWorkItemTest()
        {
            m_mockDependencyResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_mockDependencyResolver
                .Setup(resolver => resolver.Dispose());
            DependencyResolver.InitializeWith(m_mockDependencyResolver.Object);
            m_mockHistoryLogRepository = new Mock<IHistoryLogRepository>(MockBehavior.Strict);
            m_mockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            m_historyLogQueue = new HistoryLogQueue();
            m_workItem = new HistoryLogWorkItem("MyUnitOfWork", m_mockHistoryLogRepository.Object, m_historyLogQueue);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_mockDependencyResolver.VerifyAll();           
            m_mockHistoryLogRepository.VerifyAll();
            m_mockUnitOfWork.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "HistoryLogWorkItem")]
        public void Execute_Queue_IsEmpty()
        {
            // Arrange

            // Act
            var result = m_workItem.DoWork();

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "HistoryLogWorkItem")]
        public void Execute()
        {
            // Arrange
            var item1 = new HistoryLogItem();
            var item2 = new HistoryLogItem();
            
            m_historyLogQueue.Enqueue(item1);
            m_historyLogQueue.Enqueue(item2);

            m_mockDependencyResolver
                .Setup(resolver => resolver.Resolve<IUnitOfWork>("MyUnitOfWork"))
                .Returns(m_mockUnitOfWork.Object);
            m_mockUnitOfWork
                .Setup(unitOfWork => unitOfWork.Dispose());
            m_mockHistoryLogRepository
                .Setup(repository => repository.Add(item1));
            m_mockHistoryLogRepository
                .Setup(repository => repository.Add(item2));
            m_mockUnitOfWork
                .Setup(unitOfWork => unitOfWork.Commit());

            // Act
            var result = m_workItem.DoWork();

            // Assert
            Assert.True(result);
            m_mockHistoryLogRepository
                .Verify(repository => repository.Add(item1), Times.Once());
            m_mockHistoryLogRepository
                .Verify(repository => repository.Add(item2), Times.Once());
            m_mockUnitOfWork
                .Verify(unitOfWork => unitOfWork.Commit(), Times.Once());
        }
    }
}
