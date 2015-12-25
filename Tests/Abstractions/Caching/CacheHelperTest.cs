using System;
using Moq;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class CacheHelperTest : IDisposable
    {
        private readonly Mock<ICache> m_cacheMock;

        public CacheHelperTest()
        {
            m_cacheMock = new Mock<ICache>(MockBehavior.Strict);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void CacheKey0()
        {
            // Arrange

            // Act
            var result = CacheHelper.CacheKey(GetData0CacheFunc);

            // Assert
            Assert.Equal("CacheHelperTest.GetData0", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void CacheKey1()
        {
            // Arrange

            // Act
            var result = CacheHelper.CacheKey(GetData1CacheFunc, "x");

            // Assert
            Assert.Equal("CacheHelperTest.GetData1 String='X'", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public static void CacheKey2()
        {
            // Arrange

            // Act
            var result = CacheHelper.CacheKey(GetData2CacheFunc, "x", 10);

            // Assert
            Assert.Equal("CacheHelperTest.GetData2 String='X' Int32='10'", result);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_cacheMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public void Get_T_Cached()
        {
            // Arrange
            m_cacheMock.Setup(cache => cache.Get(It.IsAny<DataKey<int>>()))
                .Callback<DataKey<int>>(datakey => datakey.Value = 100)
                .Returns(true);

            // Act
            var result = CacheHelper.Get(m_cacheMock.Object, GetData1CacheFunc, "x");

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "CacheHelper")]
        public void Get_T_NotCached()
        {
            // Arrange
            m_cacheMock
                .Setup(cache => cache.Get(It.IsAny<DataKey<int>>()))
                .Returns(true);
            m_cacheMock.Setup(cache => cache.Store(It.IsAny<DataKey<int>>()))
                .Callback<DataKey<int>>(datakey =>
                {
                    Assert.True(datakey.HasValue);
                    Assert.Equal(100, datakey.Value);
                })
                .Returns(true);

            // Act
            var result = CacheHelper.Get(m_cacheMock.Object, GetData1CacheFunc, "x");

            // Assert
            Assert.Equal(100, result);
        }

        private static int GetData0()
        {
            return 100;
        }

        private static Func2<int> GetData0CacheFunc
        {
            get { return new Func2<int>(GetData0); }
        }

        private static int GetData1(string id)
        {
            return 100;
        }

        private static Func2<string, int> GetData1CacheFunc
        {
            get { return new Func2<string, int>(GetData1); }
        }

        private static int GetData2(string id, int lastDays)
        {
            return 100;
        }

        private static Func2<string, int, int> GetData2CacheFunc
        {
            get { return new Func2<string, int, int>(GetData2); }
        }
    }
}
