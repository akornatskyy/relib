using ReusableLibrary.Abstractions.Threading;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    public sealed class WaitAsyncResultTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.TraitNames.Threading, "WaitAsyncResult")]
        public static void Wait_TimedOut(bool callbackSucceed)
        {
            // Arrange
            var waitAsyncResult = new WaitAsyncResult((w, r) => callbackSucceed);

            // Act
            var result = waitAsyncResult.Wait(0);
            waitAsyncResult.Callback(null);

            // Assert
            Assert.False(result);
            Assert.True(waitAsyncResult.TimedOut);
            Assert.Equal(callbackSucceed, waitAsyncResult.Succeed);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.TraitNames.Threading, "WaitAsyncResult")]
        public static void Wait_Completed(bool callbackSucceed)
        {
            // Arrange
            var waitAsyncResult = new WaitAsyncResult((w, r) => callbackSucceed);

            // Act
            waitAsyncResult.Callback(null);
            var result = waitAsyncResult.Wait(0);

            // Assert
            Assert.Equal(callbackSucceed, result);
            Assert.False(waitAsyncResult.TimedOut);
            Assert.Equal(callbackSucceed, waitAsyncResult.Succeed);
        }
    }
}
