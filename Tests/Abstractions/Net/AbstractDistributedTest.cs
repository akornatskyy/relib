using System;
using Moq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class AbstractDistributedTest : IDisposable
    {
        private readonly AbstractDistributed m_abstractDistributed;
        private readonly Mock<IPool<IClient>> m_mockPool;
        private readonly Mock<IPool<IClient>> m_mockIdle;
        private readonly Mock<IClient> m_mockClient;
        private readonly Mock<IClientConnection> m_mockClientConnection;
        private readonly DistributedOptions m_options;

        public AbstractDistributedTest()
        {
            m_options = new DistributedOptions();
            m_mockPool = new Mock<IPool<IClient>>(MockBehavior.Strict);
            m_mockIdle = new Mock<IPool<IClient>>(MockBehavior.Strict);
            m_mockClient = new Mock<IClient>(MockBehavior.Strict);
            m_mockClientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
            m_abstractDistributed = new TestDistributed();
            m_abstractDistributed.Pool = m_mockPool.Object;
            m_abstractDistributed.Idle = m_mockIdle.Object;
            m_abstractDistributed.Options = m_options;
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_abstractDistributed.Dispose();
            m_mockClient.VerifyAll();
            m_mockClientConnection.VerifyAll();
            m_mockPool.VerifyAll();
            m_mockIdle.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void AddRange()
        {
            // Arrange
            var mockClient1 = new Mock<IClient>(MockBehavior.Strict);
            var mockClient2 = new Mock<IClient>(MockBehavior.Strict);
            m_mockPool.Setup(pool => pool.Return(mockClient1.Object)).Returns(true);
            m_mockPool.Setup(pool => pool.Return(mockClient2.Object)).Returns(true);

            // Act
            m_abstractDistributed.AddRange(new[] 
            { 
                mockClient1.Object,
                mockClient2.Object
            });

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void AddRange_Pool_CanNot_Accept_More_Clients()
        {
            // Arrange
            var mockClient1 = new Mock<IClient>(MockBehavior.Strict);
            var mockClient2 = new Mock<IClient>(MockBehavior.Strict);
            m_mockPool.Setup(pool => pool.Return(mockClient1.Object)).Returns(true);
            m_mockPool.Setup(pool => pool.Return(mockClient2.Object)).Returns(false);

            // Act
            Assert.Throws<InvalidOperationException>(() => m_abstractDistributed.AddRange(new[] 
            { 
                mockClient1.Object,
                mockClient2.Object
            }));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void Context()
        {
            // Arrange
            var state = new byte[] { };
            m_mockPool.Setup(pool => pool.Take(state)).Returns(m_mockClient.Object);
            m_mockClient.Setup(client => client.Context(state)).Returns<byte[]>(s => a =>
            {
                a(m_mockClientConnection.Object, s);
                return true;
            });
            var called = false;

            // Act
            var result = m_abstractDistributed.Context(state)((c, s) =>
            {
                called = true;
            });

            // Assert
            Assert.True(result);
            Assert.True(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void Context_AddToIdle()
        {
            // Arrange
            var state = new byte[] { };
            m_mockPool.Setup(pool => pool.Take(state)).Returns(m_mockClient.Object);
            m_mockClient.Setup(client => client.Context(state)).Returns<byte[]>(s => a =>
            {
                return false;
            });
            m_mockPool.Setup(pool => pool.Take(m_mockClient.Object)).Returns(m_mockClient.Object);
            m_mockClient.Setup(client => client.IdleState).Returns(new IdleState());
            m_mockIdle.Setup(idle => idle.Return(m_mockClient.Object)).Returns(true);

            // Act
            var result = m_abstractDistributed.Context(state)((c, s) =>
            {
            });

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void Context_NoFree_Connections()
        {
            // Arrange
            var state = new byte[] { };
            m_mockPool.Setup(pool => pool.Take(state)).Returns<IClient>(null);
            var called = false;

            // Act
            var result = m_abstractDistributed.Context(state)((c, s) =>
            {
                called = true;
            });

            // Assert
            Assert.False(result);
            Assert.False(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void Context_Split()
        {
            // Arrange
            var key1 = new byte[] { 1 };
            var key2 = new byte[] { 2 };
            var state = new byte[][] { key1, key2 };
            m_mockPool.Setup(pool => pool.Take(key1)).Returns(() => m_mockClient.Object);
            m_mockPool.Setup(pool => pool.Take(key2)).Returns(() => m_mockClient.Object);
            m_mockClient.Setup(client => client.Context(It.IsAny<byte[][]>())).Returns<byte[][]>(s => a =>
            {
                a(m_mockClientConnection.Object, s);
                return true;
            });
            m_mockClient.Setup(client => client.Equals(m_mockClient.Object)).Returns(true);
            var called = false;

            // Act
            var result = m_abstractDistributed.Context(state)((c, s) =>
            {
                called = true;
            });

            // Assert
            Assert.True(result);
            Assert.True(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void AddToIdle()
        {
            // Arrange
            m_mockClient.Setup(client => client.IdleState).Returns(new IdleState());
            m_mockIdle.Setup(idle => idle.Return(m_mockClient.Object)).Returns(true);

            // Act
            m_abstractDistributed.AddToIdle(m_mockClient.Object);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void AddToIdle_Null()
        {
            // Arrange

            // Act
            m_abstractDistributed.AddToIdle(null);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractDistributed")]
        public void RetryIdled()
        {
            // Arrange
            m_mockPool.Setup(pool => pool.Return(m_mockClient.Object)).Returns(true);

            // Act
            m_abstractDistributed.RetryIdled(m_mockClient.Object);

            // Assert
        }

        internal class TestDistributed : AbstractDistributed
        {
        }
    }
}
