using Moq;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class PooledTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "Pooled")]
        public static void Take_Returned_NotNull()
        {
            // Arrange
            var mockInnerPool = new Mock<IPool<string>>(MockBehavior.Strict);
            mockInnerPool.Setup(inner => inner.Take("state")).Returns("x");
            mockInnerPool.Setup(inner => inner.Return("x")).Returns(true);

            // Act
            var pooled = new Pooled<string>(mockInnerPool.Object, "state");

            // Assert
            Assert.Equal("x", pooled.Item);

            // Act            
            pooled.Dispose();

            // Assert
            Assert.Null(pooled.Item);
            mockInnerPool.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Pooled")]
        public static void Take_Returned_Null()
        {
            // Arrange
            var mockInnerPool = new Mock<IPool<string>>(MockBehavior.Strict);
            mockInnerPool.Setup(inner => inner.Take("state")).Returns((string)null);

            // Act
            var pooled = new Pooled<string>(mockInnerPool.Object, "state");

            // Assert
            Assert.Null(pooled.Item);

            // Act            
            pooled.Dispose();

            // Assert
            Assert.Null(pooled.Item);
            mockInnerPool.VerifyAll();
        }
    }
}
