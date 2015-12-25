using System;
using System.Security.Principal;
using System.Threading;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class LazyPrincipalTest : IDisposable
    {
        private readonly IPrincipal m_savedPrincipal;        
        private readonly ILazy<string> m_lazyString;
        private int m_count;

        public LazyPrincipalTest()
        {
            m_savedPrincipal = Thread.CurrentPrincipal;
            m_lazyString = new LazyPrincipal<string>(user => { m_count++; return "test"; });
            Assert.Equal(0, m_count);
            Assert.False(m_lazyString.Loaded);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread.CurrentPrincipal = m_savedPrincipal;
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyPrincipal")]
        public void Object()
        {
            // Arrange

            // Act            
            var result = m_lazyString.Object;

            // Assert
            Assert.Equal("test", result);
            Assert.Equal(1, m_count);
            Assert.True(m_lazyString.Loaded);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyPrincipal")]
        public void Object_Principal_Changed()
        {
            // Arrange
            var result = m_lazyString.Object;
            Assert.Equal("test", result);
            Assert.Equal(1, m_count);
            Assert.True(m_lazyString.Loaded);

            // Act            
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("x"), new string[] { });
            Assert.False(m_lazyString.Loaded);
            result = m_lazyString.Object;

            // Assert
            Assert.Equal("test", result);
            Assert.Equal(2, m_count);
            Assert.True(m_lazyString.Loaded);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyPrincipal")]
        public void Object_Twice()
        {
            // Arrange
            var result = m_lazyString.Object;
            Assert.NotNull(result);
            Assert.True(m_lazyString.Loaded);

            // Act
            result = m_lazyString.Object;
            Assert.NotNull(result);
            Assert.True(m_lazyString.Loaded);

            // Assert
            Assert.Equal("test", result);
            Assert.Equal(1, m_count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyObject")]
        public void Reset()
        {
            // Arrange
            var result = m_lazyString.Object;
            Assert.True(m_lazyString.Loaded);

            // Act            
            m_lazyString.Reset();

            // Assert
            Assert.False(m_lazyString.Loaded);
            result = m_lazyString.Object;
            Assert.True(m_lazyString.Loaded);
            Assert.Equal("test", result);
            Assert.Equal(2, m_count);
        }
    }
}
