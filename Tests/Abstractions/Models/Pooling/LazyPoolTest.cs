using Moq;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class LazyPoolTest : ManagedPoolTest
    {
        public LazyPoolTest()
        {
            DecoratedPool = new LazyPool<string>(MockInnerPool.Object, CreateFactory, ReleaseFactory);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyPool")]
        public static void Take_Inner_Returns_Null()
        {
            // Arrange
            var mockInnerPool = new Mock<IPool<string>>(MockBehavior.Strict);
            mockInnerPool.Setup(inner => inner.Name).Returns("Mock");
            mockInnerPool.Setup(inner => inner.Take("state")).Returns((string)null);
            var decoratedPool = new LazyPool<string>(mockInnerPool.Object, state =>
            {
                Assert.Equal("state", (string)state);
                return "x";
            }, ReleaseFactory);

            // Act
            var result = decoratedPool.Take("state");

            // Assert
            Assert.Equal("x", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "LazyPool")]
        public override void Return_Already_Disposed()
        {
            // Arrange            
            MockInnerPool.Setup(inner => inner.Take(null)).Returns((string)null);
            MockInnerPool.Setup(inner => inner.Count).Returns(0);
            MockInnerPool.Setup(inner => inner.Dispose());
            DecoratedPool.Dispose();

            // Act
            var result = DecoratedPool.Return(null);

            // Assert
            Assert.False(result);
        }

        private static string CreateFactory(object state)
        {
            return "x";
        }
    }
}
