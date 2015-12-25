using System;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Services;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public sealed class TopicCatalogTest : IDisposable
    {
        private readonly ITopicCatalog m_catalog;

        public TopicCatalogTest()
        {
            m_catalog = new TopicCatalog();
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_catalog.Dispose();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Services, "TopicCatalog")]
        public void GetTopic()
        {
            // Arrange
            var t1 = m_catalog.Get<Topic>("T1");
            var t2 = m_catalog.Get<Topic>("T2");

            // Act
            var t = m_catalog.Get<Topic>("T1");

            // Assert  
            Assert.NotNull(t);
            Assert.NotNull(t1);
            Assert.Same(t1, t);
            Assert.NotNull(t2);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "TopicCatalog")]
        public void GetTopic1()
        {
            // Arrange
            var t1 = m_catalog.Get<Topic<string>>("T1");
            var t2 = m_catalog.Get<Topic<int>>("T2");

            // Act
            var t = m_catalog.Get<Topic<string>>("T1");

            // Assert  
            Assert.NotNull(t);
            Assert.NotNull(t1);
            Assert.Same(t1, t);
            Assert.NotNull(t2);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "TopicCatalog")]
        public void GetTopic1_Generic_Type_Differs()
        {
            // Arrange
            var t1 = m_catalog.Get<Topic<string>>("T1");

            // Act
            var t = m_catalog.Get<Topic<int>>("T1");

            // Assert
            Assert.NotNull(t);
            Assert.NotNull(t1);
            Assert.NotSame(t1, t);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "TopicCatalog")]
        public void GetTopic2()
        {
            // Arrange
            var t1 = m_catalog.Get<Topic<string, int>>("T1");
            var t2 = m_catalog.Get<Topic<int, string>>("T2");

            // Act
            var t = m_catalog.Get<Topic<string, int>>("T1");

            // Assert  
            Assert.NotNull(t);
            Assert.Same(t1, t);
            Assert.NotNull(t2);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "TopicCatalog")]
        public void GetTopic2_Generic_Types_Differ()
        {
            // Arrange
            var t1 = m_catalog.Get<Topic<string, int>>("T1");

            // Act
            var t = m_catalog.Get<Topic<int, string>>("T1");

            // Assert  
            Assert.NotSame(t1, t);
        }
    }
}
