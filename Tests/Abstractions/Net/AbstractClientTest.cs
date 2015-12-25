using System;
using Moq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class AbstractClientTest : IDisposable
    {
        private readonly AbstractClient<IClientConnection> m_abstractClient;
        private readonly Mock<IPool<IClientConnection>> m_mockPool;
        private readonly Mock<IClientConnection> m_mockClientConnection;
        private readonly ConnectionOptions m_options;

        public AbstractClientTest()
        {
            m_options = new ConnectionOptions("Port = 1234");
            m_mockClientConnection = new Mock<IClientConnection>(MockBehavior.Strict);
            m_mockPool = new Mock<IPool<IClientConnection>>(MockBehavior.Strict);
            m_abstractClient = new TestClient();
            m_abstractClient.Options = m_options;
            m_abstractClient.Pool = m_mockPool.Object;
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_abstractClient.Dispose();
            m_mockClientConnection.VerifyAll();
            m_mockPool.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractClient")]
        public void Context()
        {
            // Arrange
            m_mockPool.Setup(pool => pool.Take(m_options)).Returns(m_mockClientConnection.Object);
            m_mockPool.Setup(pool => pool.Return(m_mockClientConnection.Object)).Returns(true);
            m_mockClientConnection.Setup(connection => connection.TryConnect()).Returns(true);
            var called = false;

            // Act
            var result = m_abstractClient.Context("state")((connection, state) => 
            {
                Assert.Equal(m_mockClientConnection.Object, connection);
                Assert.Equal("state", (string)state);
                called = true;
            });

            // Assert
            Assert.True(result);
            Assert.True(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractClient")]
        public void Context_NoFree_Connections()
        {
            // Arrange
            m_mockPool.Setup(pool => pool.Take(m_options)).Returns<IClientConnection>(null);
            var called = false;

            // Act
            var result = m_abstractClient.Context("state")((connection, state) =>
            {
                called = true;
            });

            // Assert
            Assert.False(result);
            Assert.False(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractClient")]
        public void Context_CanNot_Connect()
        {
            // Arrange
            m_mockPool.Setup(pool => pool.Take(m_options)).Returns(m_mockClientConnection.Object);
            m_mockPool.Setup(pool => pool.Return(m_mockClientConnection.Object)).Returns(true);
            m_mockClientConnection.Setup(connection => connection.TryConnect()).Returns(false);
            var called = false;

            // Act
            var result = m_abstractClient.Context("state")((connection, state) =>
            {
                called = true;
            });

            // Assert
            Assert.False(result);
            Assert.False(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractClient")]
        public void Context_Error_While_Trying_To_Connect()
        {
            // Arrange
            m_mockPool.Setup(pool => pool.Take(m_options)).Returns(m_mockClientConnection.Object);
            m_mockPool.Setup(pool => pool.Return(m_mockClientConnection.Object)).Returns(true);
            m_mockClientConnection.Setup(connection => connection.TryConnect()).Throws<SystemException>();
            m_mockClientConnection.Setup(connection => connection.Close());
            var called = false;

            // Act
            var result = m_abstractClient.Context("state")((connection, state) =>
            {
                called = true;
            });

            // Assert
            Assert.False(result);
            Assert.False(called);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "AbstractClient")]
        public void Client_ToString()
        {
            // Arrange

            // Act
            var result = m_abstractClient.ToString();

            // Assert
            Assert.Equal("Client [Name=Default, Server=127.0.0.1:1234]", result);
        }

        internal class TestClient : AbstractClient<IClientConnection>
        {
        }
    }
}
