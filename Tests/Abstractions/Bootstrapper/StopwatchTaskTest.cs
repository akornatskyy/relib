using ReusableLibrary.Abstractions.Bootstrapper;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Bootstrapper
{
    public sealed class StopwatchTaskTest
    {
        private readonly StopwatchTask m_task = new StopwatchTask();

        [Fact]
        [Trait(Constants.TraitNames.Bootstrapper, "StopwatchTask")]
        public void Startup()
        {
            // Arrange

            // Act
            Assert.DoesNotThrow(() => (m_task as IStartupTask).Execute());

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Bootstrapper, "StopwatchTask")]
        public void Shutdown()
        {
            // Arrange

            // Act
            Assert.DoesNotThrow(() => (m_task as IShutdownTask).Execute());

            // Assert
        }
    }
}
