using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.IoC
{
    public sealed class DependencyResolverTest : IDisposable
    {
        private readonly Mock<IDependencyResolver> m_resolverMock;

        public DependencyResolverTest()
        {
            m_resolverMock = new Mock<IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());

            DependencyResolver.InitializeWith(m_resolverMock.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void Initialize_Twice_DoesThrow()
        {
            // Arrange

            // Act
            Assert.Throws<InvalidOperationException>(() => DependencyResolver.InitializeWith(m_resolverMock.Object));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void Resolve()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.Resolve<int>()).Returns(100);

            // Act
            var result = DependencyResolver.Resolve<int>();

            // Assert
            Assert.Equal(100, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("name")]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void Resolve_By_Name(string name)
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.Resolve<int>(name)).Returns(100);

            // Act
            var result = DependencyResolver.Resolve<int>(name);

            // Assert
            Assert.Equal(100, result);
        }

        [Theory]
        [InlineData(typeof(NameValueCollection))]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void Resolve_By_Type(Type type)
        {
            // Arrange
            var instance = Activator.CreateInstance(type) as ICollection;
            m_resolverMock.Setup(resolver => resolver.Resolve<ICollection>(type)).Returns(instance);

            // Act
            var result = DependencyResolver.Resolve<ICollection>(type);

            // Assert
            Assert.NotNull(result);
            Assert.Same(instance, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void Resolve_By_Type_With_Null()
        {
            // Arrange
            Type type = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => DependencyResolver.Resolve<ICollection>(type));

            // Assert
            Assert.Null(type);
            Assert.NotNull(m_resolverMock.Object);
        }

        [Fact]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void ResolveAll()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.ResolveAll<int>()).Returns(new[] { 1, 2 });

            // Act
            var result = new List<int>(DependencyResolver.ResolveAll<int>());

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
        }

        [Fact]
        [Trait(Constants.TraitNames.IoC, "DependencyResolver")]
        public void Reset()
        {
            // Arrange
            m_resolverMock.Setup(resolver => resolver.Dispose());

            // Act
            Assert.DoesNotThrow(() => DependencyResolver.Reset());

            // Assert
        }
    }
}
