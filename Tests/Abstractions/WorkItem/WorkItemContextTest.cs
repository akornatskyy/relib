using System;
using Moq;
using ReusableLibrary.Abstractions.WorkItem;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.WorkItem
{
    public sealed class WorkItemContextTest
    {
        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "WorkItemContext")]
        public static void Current()
        {
            // Arrange

            // Act
            var context = WorkItemContext.Current;

            // Assert
            Assert.NotNull(context);
            Assert.NotNull(context.Items);
        }

        [Fact]
        [Trait(Constants.TraitNames.WorkItem, "WorkItemContext")]
        public static void Dispose()
        {
            // Arrange
            var disposableMock = new Mock<IDisposable>(MockBehavior.Strict);
            disposableMock.Setup(disposable => disposable.Dispose());
            var context = WorkItemContext.Current;
            context.Items.Add("key", disposableMock.Object);

            // Act            
            context.Dispose();

            // Assert
            Assert.Null(context.Items);
        }
    }
}
