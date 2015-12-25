using System.Collections.Generic;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class EagerPoolTest : ManagedPoolTest
    {
        public EagerPoolTest()
        {
            var calls = 0;
            MockInnerPool.Setup(inner => inner.Size).Returns(2);
            MockInnerPool.Setup(inner => inner.Return("x")).Callback(() => calls++).Returns(true);
            DecoratedPool = new EagerPool<string>(MockInnerPool.Object, CreateFactory, ReleaseFactory);
            Assert.Equal(2, calls);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "EagerPool")]
        public override void Return_Already_Disposed()
        {
            // Arrange            
            var items = new Queue<string>(new string[] { "x", "x", null });
            MockInnerPool.Setup(inner => inner.Take(null)).Returns(items.Dequeue);
            MockInnerPool.Setup(inner => inner.Count).Returns(0);
            MockInnerPool.Setup(inner => inner.Dispose());
            DecoratedPool.Dispose();

            // Act
            var result = DecoratedPool.Return(null);

            // Assert
            Assert.False(result);
        }

        private static string CreateFactory()
        {
            return "x";
        }
    }
}
