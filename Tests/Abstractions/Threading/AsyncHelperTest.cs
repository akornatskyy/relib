using System.Threading;
using ReusableLibrary.Abstractions.Threading;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    public sealed class AsyncHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Threading, "AsyncHelper")]
        public static void FireAndForget_SingleArgument()
        {
            // Arrange
            var waitHandle = new AutoResetEvent(false);
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var actionThreadId = -1;

            // Act
            AsyncHelper.FireAndForget(arg => 
            { 
                Assert.Equal(100, arg);
                actionThreadId = Thread.CurrentThread.ManagedThreadId;
                waitHandle.Set();
            }, 100);
            var result = waitHandle.WaitOne(500);

            // Assert
            Assert.True(result);
            Assert.NotEqual(-1, actionThreadId);
            Assert.NotEqual(threadId, actionThreadId);
        }

        [Fact]
        [Trait(Constants.TraitNames.Threading, "AsyncHelper")]
        public static void FireAndForget_TwoArguments()
        {
            // Arrange
            var waitHandle = new AutoResetEvent(false);
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var actionThreadId = -1;

            // Act
            AsyncHelper.FireAndForget((argA, argB) =>
            {
                Assert.Equal(100, argA);
                Assert.Equal(200, argB);
                actionThreadId = Thread.CurrentThread.ManagedThreadId;
                waitHandle.Set();
            }, 100, 200);
            var result = waitHandle.WaitOne(500);

            // Assert
            Assert.True(result);
            Assert.NotEqual(-1, actionThreadId);
            Assert.NotEqual(threadId, actionThreadId);
        }
    }
}
