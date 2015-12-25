using System;
using Moq;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.IoC;
using Xunit;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Tests.Caching
{
    public sealed class LinkedCacheDependencyTest : IDisposable
    {
        private readonly Mock<ICache> m_cacheMock;
        private readonly DateTime m_expiresAt;
        private readonly LinkedCacheDependency m_dependency;

        public LinkedCacheDependencyTest()
        {
            m_cacheMock = new Mock<ICache>(MockBehavior.Strict);
            m_expiresAt = DateTime.Now.AddMinutes(10);
            m_dependency = new LinkedCacheDependency(m_cacheMock.Object, "HeadKey", m_expiresAt);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_cacheMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Caching, "LinkedCacheDependency")]
        public void Add_FirstTime()
        {
            // Arrange            
            var head = new Pair<string>();
            m_cacheMock
               .Setup(cache => cache.Get<Pair<string>>("HeadKey")).Returns(() =>
               {
                    return default(Pair<string>);
               });
            m_cacheMock
                .Setup(cache => cache.Store("HeadKey", It.IsAny<Pair<string>>(), m_expiresAt))
                .Callback((string key, object value, DateTime expiresAt) => head = (Pair<string>)value)
                .Returns(true);

            // Act
            m_dependency.Add("A1");

            // Assert
            Assert.Equal("A1", head.First);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "LinkedCacheDependency")]
        public void Add()
        {
            // Arrange            
            var head = new Pair<string>();
            m_cacheMock
               .Setup(cache => cache.Get<Pair<string>>("HeadKey")).Returns(head);
            m_cacheMock
                .Setup(cache => cache.Store("HeadKey", It.IsAny<Pair<string>>(), m_expiresAt))
                .Callback((string key, object value, DateTime expiresAt) => head = (Pair<string>)value)
                .Returns(true);

            // Act
            m_dependency.Add("A1");

            // Assert
            Assert.Equal("A1", head.First);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "LinkedCacheDependency")]
        public void Add_More()
        {
            // Arrange            
            var head = new Pair<string>("A1", null);
            var next = new Pair<string>();
            m_cacheMock
                .Setup(cache => cache.Get<Pair<string>>("HeadKey")).Returns(head);
            m_cacheMock
                .Setup(cache => cache.Store(It.IsAny<string>(), It.IsAny<Pair<string>>(), m_expiresAt))
                .Callback((string key, object value, DateTime expiresAt) => next = (Pair<string>)value)
                .Returns(true);

            // Act
            m_dependency.Add("A2");

            // Assert
            Assert.Equal("A1", head.First);
            Assert.Equal("A2", next.First);
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "LinkedCacheDependency")]
        public void Remove_Empty()
        {
            // Arrange            
            var head = new Pair<string>();
            m_cacheMock
               .Setup(cache => cache.Get<Pair<string>>("HeadKey")).Returns(head);

            // Act
            m_dependency.Remove();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "LinkedCacheDependency")]
        public void Remove()
        {
            // Arrange            
            var head = new Pair<string>("A1", null);
            m_cacheMock
               .Setup(cache => cache.Get<Pair<string>>("HeadKey")).Returns(head);
            m_cacheMock
                .Setup(cache => cache.Remove(head.First))
                .Returns(true);
            m_cacheMock
                .Setup(cache => cache.Remove("HeadKey"))
                .Returns(true);

            // Act
            m_dependency.Remove();

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Caching, "LinkedCacheDependency")]
        public void Remove_More()
        {
            // Arrange            
            var head = new Pair<string>("A1", LinkedCacheDependency.DependencyKey + "A2");
            m_cacheMock
               .Setup(cache => cache.Get<Pair<string>>("HeadKey")).Returns(head);
            m_cacheMock
                .Setup(cache => cache.Remove(head.First))
                .Returns(true);
            m_cacheMock
                .Setup(cache => cache.Remove("HeadKey"))
                .Returns(true);

            var next = new Pair<string>("A2", null);
            m_cacheMock
                .Setup(cache => cache.Get<Pair<string>>(head.Second)).Returns(next);
            m_cacheMock
                .Setup(cache => cache.Remove(next.First))
                .Returns(true);
            m_cacheMock
                .Setup(cache => cache.Remove(head.Second))
                .Returns(true);

            // Act
            m_dependency.Remove();

            // Assert
        }
    }
}
