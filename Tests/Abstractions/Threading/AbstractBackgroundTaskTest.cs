using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ReusableLibrary.Abstractions.Threading;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    public sealed class AbstractBackgroundTaskTest : IDisposable
    {
        private readonly MockAbstractBackgroundTask m_task;

        public AbstractBackgroundTaskTest()
        {
            m_task = new MockAbstractBackgroundTask();
            Assert.False(m_task.IsRunning);
            Assert.False(m_task.WorkDone);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_task.Wait(TimeSpan.FromSeconds(1));
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Threading, "AbstractBackgroundTask")]
        public void Start()
        {
            // Arrange

            // Act
            m_task.Start();

            // Assert
            Assert.True(m_task.IsRunning);
            Assert.True(m_task.Wait(TimeSpan.FromSeconds(1)));
            Assert.False(m_task.IsRunning);
            Assert.True(m_task.WorkDone);
            Assert.Equal("MockAbstractBackgroundTaskThread", m_task.ThreadName);
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "AbstractBackgroundTask")]
        public void Start_When_Running()
        {
            // Arrange

            // Act
            m_task.Start();
            Assert.True(m_task.IsRunning);

            // Assert
            Assert.Throws<InvalidOperationException>(() => m_task.Start());
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "AbstractBackgroundTask")]
        public void Wait()
        {
            // Arrange

            // Act
            m_task.Start();

            // Assert
            Assert.True(m_task.IsRunning);
            Assert.True(m_task.Wait(TimeSpan.FromSeconds(1)));
            Assert.False(m_task.IsRunning);
            Assert.True(m_task.Wait(TimeSpan.FromSeconds(1)));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [Trait(Constants.TraitNames.Threading, "AbstractBackgroundTask")]
        public void Stop(bool forceShutdown)
        {
            // Arrange

            // Act
            m_task.Start();
            Assert.True(m_task.IsRunning);
            m_task.Stop(forceShutdown);

            // Assert
            if (!forceShutdown)
            {
                Assert.True(m_task.Wait(TimeSpan.FromSeconds(1)));
            }

            Assert.False(m_task.IsRunning);
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "AbstractBackgroundTask")]
        public void HasMoreWork()
        {
            // Arrange
            m_task.HasMoreWork = true;

            // Act
            m_task.Start();
            while (m_task.RunCount < 10)
            {
                Thread.Sleep(10);
            }
            
            // Assert
            Assert.True(m_task.IsRunning);
            m_task.Stop(false);
            Assert.True(m_task.Wait(TimeSpan.FromSeconds(1)));
            Assert.False(m_task.IsRunning);
        }
    }
}
