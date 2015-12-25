using System;
using System.Security.Principal;
using System.Threading;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Services;
using Xunit;
using ReusableLibrary.Abstractions.Repository;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class CachingMementoRepositoryTest : IDisposable
    {
        private readonly Mock<ICache> m_mockCache;
        private readonly Mock<IMementoRepository> m_mockInner;
        private readonly IMementoRepository m_repository;

        public CachingMementoRepositoryTest()
        {
            m_mockInner = new Mock<IMementoRepository>(MockBehavior.Strict);
            m_mockCache = new Mock<ICache>(MockBehavior.Strict);            
            m_repository = new CachingMementoRepository(m_mockInner.Object, m_mockCache.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockInner.VerifyAll();
            m_mockCache.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Services, "CachingMementoRepository")]
        public void Store_Inner_Failed()
        {
            // Arrange
            var value = new State();
            m_mockInner
                .Setup(inner => inner.Store<State>("ID", value))
                .Returns(false);

            // Act
            var succeed = m_repository.Store("ID", value);

            // Assert
            Assert.False(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "CachingMementoRepository")]
        public void Store()
        {
            // Arrange
            var value = new State();
            m_mockInner
                .Setup(inner => inner.Store<State>("ID", value))
                .Returns(true);
            m_mockCache
                .Setup(cache => cache.Store<State>(
                    "Memento:ID",
                    value,
                    CachingMementoRepository.DefaultLifetime))
                .Returns(true);

            // Act
            var succeed = m_repository.Store("ID", value);

            // Assert
            Assert.True(succeed);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "CachingMementoRepository")]
        public void Retrieve_Cache_Fail()
        {
            // Arrange
            var value = new State();            
            m_mockCache
               .Setup(cache => cache.Get<State>(It.IsAny<DataKey<State>>()))
               .Returns(false);
            m_mockInner
                .Setup(inner => inner.Retrieve<State>("ID"))
                .Returns(value);

            // Act
            var result = m_repository.Retrieve<State>("ID");

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "CachingMementoRepository")]
        public void Retrieve_Cache_Miss()
        {
            // Arrange
            var value = new State();
            m_mockCache
               .Setup(cache => cache.Get<State>(It.IsAny<DataKey<State>>()))
               .Returns(true);
            m_mockInner
                .Setup(inner => inner.Retrieve<State>("ID"))
                .Returns(value);

            // Act
            var result = m_repository.Retrieve<State>("ID");

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "CachingMementoRepository")]
        public void Retrieve_Cache_Hit()
        {
            // Arrange
            var value = new State();
            m_mockCache
               .Setup(cache => cache.Get<State>(It.IsAny<DataKey<State>>()))
               .Callback<DataKey<State>>(datakey => 
               {
                   Assert.Equal("Memento:ID", datakey.Key);
                   datakey.Value = value; 
               })
               .Returns(true);

            // Act
            var result = m_repository.Retrieve<State>("ID");

            // Assert
            Assert.Equal(value, result);
        }

        private class State
        {
        }
    }
}
