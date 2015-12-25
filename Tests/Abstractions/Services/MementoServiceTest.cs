using System;
using System.Security.Principal;
using System.Threading;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Services;
using Xunit;
using ReusableLibrary.Abstractions.Repository;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public sealed class MementoServiceTest : IDisposable
    {
        private readonly IPrincipal m_savedPrincipal;        
        private readonly Mock<IMementoRepository> m_mockRepository;
        private readonly IMementoService m_service;

        public MementoServiceTest()
        {
            m_savedPrincipal = Thread.CurrentPrincipal;
            m_mockRepository = new Mock<IMementoRepository>(MockBehavior.Strict);
            m_service = new MementoService(m_mockRepository.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread.CurrentPrincipal = m_savedPrincipal;
            m_mockRepository.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Save_Disabled()
        {
            // Arrange
            var value = (object)null;
            ((MementoService)m_service).Enabled = false;

            // Act
            var succeed = m_service.Save(value);

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Save_Value_Is_Null()
        {
            // Arrange
            var value = (object)null;

            // Act
            Assert.Throws<ArgumentNullException>(() => m_service.Save(value));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Save()
        {
            // Arrange
            var value = new State();
            m_mockRepository
                .Setup(repository => repository.Store<State>(
                    "ReusableLibrary.Abstractions.Tests.Services.MementoServiceTest+State", 
                    value))
                .Returns(true);            

            // Act
            var succeed = m_service.Save(value);

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Save_Per_User()
        {
            // Arrange
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("master"), new string[] { });
            var value = new State();
            m_mockRepository
                .Setup(repository => repository.Store<State>(
                    "MASTER:ReusableLibrary.Abstractions.Tests.Services.MementoServiceTest+State",
                    value))
                .Returns(true);

            // Act
            var succeed = m_service.Save(value);

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Load_Disabled()
        {
            // Arrange
            ((MementoService)m_service).Enabled = false;

            // Act
            var result = m_service.Load<State>();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Load()
        {
            // Arrange
            var value = new State();
            m_mockRepository
                .Setup(repository => repository.Retrieve<State>(
                    "ReusableLibrary.Abstractions.Tests.Services.MementoServiceTest+State"))
                .Returns(value);

            // Act
            var result = m_service.Load<State>();

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "MementoService")]
        public void Load_Per_User()
        {
            // Arrange
            var value = new State();
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("master"), new string[] { });
            m_mockRepository
                .Setup(repository => repository.Retrieve<State>(
                    "MASTER:ReusableLibrary.Abstractions.Tests.Services.MementoServiceTest+State"))
                .Returns(value);

            // Act
            var result = m_service.Load<State>();

            // Assert
            Assert.Equal(value, result);
        }

        private class State 
        { 
        }
    }
}
