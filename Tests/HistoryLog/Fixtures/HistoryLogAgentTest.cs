using System;
using Moq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.HistoryLog.Agents;
using ReusableLibrary.HistoryLog.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.HistoryLog.Tests.Fixtures
{
    public sealed class HistoryLogAgentTest : IDisposable
    {
        private readonly IHistoryLogAgent m_historyLogAgent;
        private readonly Mock<IRemoteLocationProvider> m_mockRemoteLocationProvider;
        private readonly HistoryLogQueue m_historyLogQueue;

        public HistoryLogAgentTest()
        {
            m_mockRemoteLocationProvider = new Mock<IRemoteLocationProvider>(MockBehavior.Strict);
            m_mockRemoteLocationProvider.Setup(provider => provider.RemoteLocation).Returns(RemoteLocation.Localhost);
            m_historyLogQueue = new HistoryLogQueue();
            m_historyLogAgent = new HistoryLogAgent(m_mockRemoteLocationProvider.Object, m_historyLogQueue);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_mockRemoteLocationProvider.VerifyAll();
        }

        #endregion

        [Theory]
        [InlineData(null, 0, null, null)]
        [InlineData("user1", 0, "ZX-34", "a\tb")]
        [Trait(Constants.TraitNames.Agent, "HistoryLogAgent")]
        public void Add(string username, int eventid, string related, string args)
        {
            // Arrange
            Assert.True(m_historyLogQueue.IsEmpty());

            // Act
            m_historyLogAgent.Add(username, (short)eventid, related, HistoryLogItem.Split(args));

            // Assert
            Assert.False(m_historyLogQueue.IsEmpty());
            HistoryLogItem item = null;
            m_historyLogQueue.Process(x => item = x);
            Assert.NotNull(item);

            Assert.Equal(username, item.Username);
            Assert.True(DateTime.UtcNow.Subtract(item.Timestamp) < TimeSpan.FromSeconds(1));
            Assert.Equal(eventid, item.EventId);
            Assert.Equal(1, item.Hosts.Length);
            Assert.Equal(IpNumberHelper.ToIpNumber(RemoteLocation.Localhost.Hosts[0]), item.Hosts[0]);
            Assert.Equal(related, item.RelatedTo);
            Assert.Equal(args, item.Arguments);
        }
    }
}
