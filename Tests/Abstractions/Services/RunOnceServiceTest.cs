using System;
using System.Net.Mail;
using System.Security;
using System.Threading;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Abstractions.Services;
using ReusableLibrary.Abstractions.Tracing;
using Xunit;
using Xunit.Extensions;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public sealed class RunOnceServiceTest : IDisposable
    {
        private static readonly TimeSpan g_period = TimeSpan.FromMinutes(5);

        private readonly Mock<ICache> m_mockCache;
        private readonly Mock<IValidationState> m_mockValidationState;
        private readonly IRunOnceService m_service;

        public RunOnceServiceTest()
        {
            m_mockCache = new Mock<ICache>(MockBehavior.Strict);
            m_mockValidationState = new Mock<IValidationState>(MockBehavior.Strict);
            m_service = new RunOnceService(m_mockCache.Object, g_period, m_mockValidationState.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockCache.VerifyAll();
            m_mockValidationState.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void Begin()
        {
            // Arrange
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(1L);

            // Act
            var result = m_service.Begin("key");

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void Begin_But_Started()
        {
            // Arrange
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(10L);

            // Act
            var result = m_service.Begin("key");

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void Error()
        {
            // Arrange
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(1L);
            m_mockCache
                .Setup(cache => cache.Store<string>(RunOnceService.ErrorKeyPrefix + "key", "error", g_period))
                .Returns(true);
            m_service.Begin("key");

            // Act
            m_service.Error("error");

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void Error_Throws_InvalidOperationException()
        {
            // Arrange

            // Act
            Assert.Throws<InvalidOperationException>(() => m_service.Error("error"));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void Result()
        {
            // Arrange
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(1L);
            m_mockCache
                .Setup(cache => cache.Store<int>(RunOnceService.ResultKeyPrefix + "key", 1000, g_period))
                .Returns(true);
            m_service.Begin("key");

            // Act
            m_service.Result(1000);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void Result_Throws_InvalidOperationException()
        {
            // Arrange

            // Act
            Assert.Throws<InvalidOperationException>(() => m_service.Result(1000));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void End_With_Error()
        {
            // Arrange
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(1L);
            m_mockCache
                .Setup(cache => cache.Get<string>(RunOnceService.ErrorKeyPrefix + "key"))
                .Returns("error");
            m_mockValidationState.Setup(vs => vs.AddError("error"));
            m_service.Begin("key");

            // Act
            var result = m_service.End<int>();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.True(result.HasError);
            Assert.Equal("error", result.Error);
            Assert.Equal(0, result.Result);
            Assert.Equal(RunOnceState.Error, result.State());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void End_With_Result()
        {
            // Arrange
            DataKey<int> datakey = null;
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(1L);
            m_mockCache
                .Setup(cache => cache.Get<string>(RunOnceService.ErrorKeyPrefix + "key"))
                .Returns((string)null);
            m_mockCache
                .Setup(cache => cache.Get<int>(It.IsAny<DataKey<int>>()))
                .Callback<DataKey<int>>(x => 
                { 
                    datakey = x;
                    datakey.Value = 1000;
                })
                .Returns(true);
            m_service.Begin("key");

            // Act
            var result = m_service.End<int>();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsCompleted);
            Assert.False(result.HasError);
            Assert.Equal(null, result.Error);
            Assert.Equal(1000, result.Result);
            Assert.Equal(RunOnceState.Done, result.State());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void End_With_NoResult()
        {
            // Arrange
            m_mockCache
                .Setup(cache => cache.Increment("key", 1L, 1L, g_period))
                .Returns(1L);
            m_mockCache
                .Setup(cache => cache.Get<string>(RunOnceService.ErrorKeyPrefix + "key"))
                .Returns((string)null);
            m_mockCache
                .Setup(cache => cache.Get<int>(It.IsAny<DataKey<int>>()))
                .Returns(true);
            m_service.Begin("key");

            // Act
            var result = m_service.End<int>();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsCompleted);
            Assert.False(result.HasError);
            Assert.Equal(null, result.Error);
            Assert.Equal(0, result.Result);
            Assert.Equal(RunOnceState.Wait, result.State());
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "RunOnceService")]
        public void End_Throws_InvalidOperationException()
        {
            // Arrange

            // Act
            Assert.Throws<InvalidOperationException>(() => m_service.End<int>());

            // Assert
        }
    }
}
