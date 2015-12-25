using System;
using System.Reflection;
using Moq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public class DecoratedPoolTest : Disposable
    {
        private static readonly Random g_random = new Random();

        public DecoratedPoolTest()
        {
            MockInnerPool = new Mock<IPool<string>>(MockBehavior.Strict);
            MockInnerPool.Setup(inner => inner.Name).Returns("Mock");

            MockDecoratedPool = new Mock<DecoratedPool<string>>(MockInnerPool.Object);
            MockDecoratedPool.CallBase = true;
            DecoratedPool = MockDecoratedPool.Object;
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public static void Constructor_InnerPool_Is_Null()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<TargetInvocationException>(() => new Mock<DecoratedPool<string>>((IPool<string>)null).Object);

            // Assert
            Assert.IsType<ArgumentNullException>(ex.InnerException);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public static void Constructor_InnerPool_Name_Is_Null()
        {
            // Arrange
            var name = RandomHelper.NextBoolean(g_random) ? string.Empty : (string)null;
            var mockInnerPool = new Mock<IPool<string>>(MockBehavior.Strict);
            mockInnerPool.Setup(inner => inner.Name).Returns(name);

            // Act
            var ex = Assert.Throws<TargetInvocationException>(() => new Mock<DecoratedPool<string>>(mockInnerPool.Object).Object);

            // Assert
            Assert.IsType<ArgumentNullException>(ex.InnerException);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public void Name()
        {
            // Arrange

            // Act
            var result = DecoratedPool.Name;

            // Assert
            Assert.Equal("Mock", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public virtual void Take()
        {
            // Arrange
            MockInnerPool.Setup(inner => inner.Take("state")).Returns("data");

            // Act
            var result = DecoratedPool.Take("state");

            // Assert
            Assert.Equal("data", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public virtual void Return()
        {
            // Arrange
            var succeed = RandomHelper.NextBoolean(g_random);
            MockInnerPool.Setup(inner => inner.Return("data")).Returns(succeed);

            // Act
            var result = DecoratedPool.Return("data");

            // Assert
            Assert.Equal(succeed, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public virtual void Clear()
        {
            // Arrange
            var succeed = RandomHelper.NextBoolean(g_random);
            MockInnerPool.Setup(inner => inner.Clear()).Returns(succeed);

            // Act
            var result = DecoratedPool.Clear();

            // Assert
            Assert.Equal(succeed, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public void Size()
        {
            // Arrange
            var expected = g_random.Next();
            MockInnerPool.Setup(inner => inner.Size).Returns(expected);

            // Act
            var result = DecoratedPool.Size;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecoratedPool")]
        public void Count()
        {
            // Arrange
            var expected = g_random.Next();
            MockInnerPool.Setup(inner => inner.Count).Returns(expected);

            // Act
            var result = DecoratedPool.Count;

            // Assert
            Assert.Equal(expected, result);
        }

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MockInnerPool.VerifyAll();
                MockDecoratedPool.VerifyAll();
            }
        }

        #endregion

        protected Mock<IPool<string>> MockInnerPool { get; set; }

        protected IPool<string> DecoratedPool { get; set; }

        private Mock<DecoratedPool<string>> MockDecoratedPool { get; set; }
    }
}
