using System;
using Moq;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Threading;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    public sealed class WaitBackgroundTasksTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Threading, "WaitBackgroundTasks")]
        public static void Execute_Stop_ButNot_Running()
        {
            // Arrange
            var taskMock1 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            var taskMock2 = new Mock<IBackgroundTask>(MockBehavior.Strict);            
            taskMock1.SetupGet(t => t.IsRunning).Returns(false);
            taskMock2.SetupGet(t => t.IsRunning).Returns(false);

            // Act
            var task = new WaitBackgroundTasks(new[] { taskMock1.Object, taskMock2.Object }, 1);
            task.Execute();

            // Assert
            taskMock1.VerifyAll();
            taskMock2.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "WaitBackgroundTasks")]
        public static void Execute_Stop_Some_Running_But_Shutdown_During_Wait()
        {
            // Arrange
            var taskMock1 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            var taskMock2 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            taskMock1.SetupGet(t => t.IsRunning).Returns(false);
            taskMock2.SetupGet(t => t.IsRunning).Returns(true);
            taskMock2.Setup(t => t.Stop(false));
            taskMock2.Setup(t => t.Wait(TimeSpan.FromSeconds(5))).Returns(true);

            // Act
            var task = new WaitBackgroundTasks(new[] { taskMock1.Object, taskMock2.Object }, 5);
            task.Execute();

            // Assert
            taskMock1.VerifyAll();
            taskMock2.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "WaitBackgroundTasks")]
        public static void Execute_Stop_Some_Running_But_Shutdown_Dueing_Wait_TimedOut()
        {
            // Arrange
            var taskMock1 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            var taskMock2 = new Mock<IBackgroundTask>(MockBehavior.Strict);
            taskMock1.SetupGet(t => t.IsRunning).Returns(false);
            taskMock2.SetupGet(t => t.IsRunning).Returns(true);
            taskMock2.Setup(t => t.Stop(false));
            taskMock2.Setup(t => t.Wait(TimeSpan.FromSeconds(5))).Returns(false);
            taskMock2.Setup(t => t.Stop(true));

            // Act
            var task = new WaitBackgroundTasks(new[] { taskMock1.Object, taskMock2.Object }, 5);
            task.Execute();

            // Assert
            taskMock1.VerifyAll();
            taskMock2.VerifyAll();
        }
    }
}
