using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Services;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public sealed class UnitOfWorkTest : IDisposable
    {
        private readonly Mock<IDependencyResolver> m_resolverMock;
        private readonly Mock<IUnitOfWork> m_unitOfWorkMock;

        public UnitOfWorkTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            m_unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);

            DependencyResolver.InitializeWith(m_resolverMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [Trait(Constants.TraitNames.Services, "UnitOfWork")]
        public static void Begin_But_Name_IsNullOrEmpty(string name)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => UnitOfWork.Begin(name));

            // Assert
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
            m_unitOfWorkMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Services, "UnitOfWork")]
        public void Begin()
        {
            // Arrange
            var unitOfWork = m_unitOfWorkMock.Object;
            m_resolverMock.Setup(resolver => resolver.Resolve<IUnitOfWork>("test")).Returns(unitOfWork);

            // Act
            var result = UnitOfWork.Begin("test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(unitOfWork, result);
        }
    }
}
