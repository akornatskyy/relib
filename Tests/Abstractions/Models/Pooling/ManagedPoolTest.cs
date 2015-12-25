using System;
using System.Collections.Generic;
using Moq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public class ManagedPoolTest : DecoratedPoolTest
    {
        private static readonly Random g_random = new Random();

        public ManagedPoolTest()
        {
            MockManagedPool = new Mock<ManagedPool<string>>(MockInnerPool.Object, new Action<string>(ReleaseFactory));
            MockManagedPool.CallBase = true;
            DecoratedPool = MockManagedPool.Object;
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "ManagedPool")]
        public static void Disposing()
        {
            // Arrange
            var mockInnerPool = new Mock<IPool<IDisposable>>(MockBehavior.Strict);
            var decoratedPool = new Mock<ManagedPool<IDisposable>>(mockInnerPool.Object, Disposable.CreateReleaseFactory());

            mockInnerPool.Setup(inner => inner.Name).Returns("Mock");
            mockInnerPool.Setup(inner => inner.Dispose());
            decoratedPool.CallBase = true;
            decoratedPool.Setup(p => p.Clear());

            // Act
            decoratedPool.Object.Dispose();

            // Assert
            mockInnerPool.VerifyAll();
            decoratedPool.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "ManagedPool")]
        public virtual void Return_Already_Disposed()
        {
            // Arrange            
            MockInnerPool.Setup(inner => inner.Dispose());
            MockManagedPool.Setup(managed => managed.Clear()).Returns(true);
            DecoratedPool.Dispose();

            // Act
            var result = DecoratedPool.Return(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "ManagedPool")]
        public void Return_Inner_Return_True()
        {
            // Arrange
            var item = RandomHelper.NextWord(g_random);
            MockInnerPool.Setup(inner => inner.Return(item)).Returns(true);

            // Act
            var result = DecoratedPool.Return(item);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "ManagedPool")]
        public void Return_Inner_Return_False()
        {
            // Arrange
            var item = RandomHelper.NextWord(g_random);
            MockInnerPool.Setup(inner => inner.Return(item)).Returns(false);

            // Act
            var result = DecoratedPool.Return(item);

            // Assert
            Assert.False(result);
        }

        public override void Return()
        {
            // The default behavior is overriden
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "ManagedPool")]
        public void Clear_Inner_Take_Until_Returns_Null()
        {
            // Arrange
            var items = new Queue<string>(new string[] { "x", "y", null });
            MockInnerPool.Setup(inner => inner.Take(null)).Returns(() => items.Dequeue());
            MockInnerPool.Setup(inner => inner.Count).Returns(0);
            var decoratedPool = new Mock<ManagedPool<string>>(MockInnerPool.Object, new Action<string>(ReleaseFactory));
            decoratedPool.CallBase = true;

            // Act
            var result = decoratedPool.Object.Clear();

            // Assert            
            Assert.True(result);
            Assert.Equal(0, items.Count);
        }

        public override void Clear()
        {
            // The default behavior is overriden
        }

        protected static void ReleaseFactory(string item)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                MockManagedPool.VerifyAll();
            }
        }

        private Mock<ManagedPool<string>> MockManagedPool { get; set; }
    }
}
