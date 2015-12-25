using System;
using Moq;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class DisposableTest : IDisposable
    {
        private readonly Mock<Disposable> m_disposableMock;

        public DisposableTest()
        {
            m_disposableMock = new Mock<Disposable>();
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Disposable")]
        public void Ensure_Call_With_Using()
        {
            // Arrange
            var disposable = m_disposableMock.Object;

            // Act
            using (disposable)
            {
                // Dummy statement
                Assert.NotNull(disposable);
            }

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Disposable")]
        public void Dispose_Twice_DoesNotThrow()
        {
            // Arrange
            var disposable = m_disposableMock.Object;

            // Act
            disposable.Dispose();

            // Assert
            Assert.DoesNotThrow(() => disposable.Dispose());
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_disposableMock.VerifyAll();
        }

        #endregion
    }
}
