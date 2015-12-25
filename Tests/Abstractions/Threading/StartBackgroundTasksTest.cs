using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Threading;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    public sealed class StartBackgroundTasksTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Threading, "StartBackgroundTasks")]
        public static void Constructor_With_Null()
        {
            // Arrange

            // Act
            Assert.DoesNotThrow(() =>
            {
                var t = new StartBackgroundTasks(null);
                Assert.NotNull(t);
            });

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "StartBackgroundTasks")]
        public static void Execute()
        {
            // Arrange
            var taskMock1 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            var taskMock2 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            taskMock1.Setup(t1 => t1.Start());
            taskMock2.Setup(t2 => t2.Start());

            // Act
            var task = new StartBackgroundTasks(new[] { taskMock1.Object, taskMock2.Object });
            task.Execute();

            // Assert
            taskMock1.VerifyAll();
            taskMock2.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "StartBackgroundTasks")]
        public static void Initialization()
        {
            // Arrange
            var taskMock1 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            var taskMock2 = new Mock<IBackgroundTask>(MockBehavior.Strict);

            // Act
            Assert.DoesNotThrow(() =>
            { 
                var t = new StartBackgroundTasks(new[] { taskMock1.Object, taskMock2.Object });
                Assert.NotNull(t);
            });

            // Assert
            taskMock1.VerifyAll();
            taskMock2.VerifyAll();
        }
    }
}
