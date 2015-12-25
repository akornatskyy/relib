using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class RetryHelperTest
    {
        private static readonly Random g_random = new Random();

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RetryHelper")]
        public static void Execute_Succeed()
        {
            // Arrange
            var options = new RetryOptions();

            // Act
            var result = RetryHelper.Execute(options, () => true);

            // Assert
            Assert.Equal(result, 0);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RetryHelper")]
        public static void Execute_MaxRetryCount()
        {
            // Arrange
            var options = new RetryOptions()
            {
                MaxRetryCount = RandomHelper.NextInt(g_random, 1, 5),
                RetryTimeout = 1000
            };
            var attempt = 0;

            // Act
            var result = RetryHelper.Execute(options, () =>
            {
                attempt++;
                return false; 
            });

            // Assert
            Assert.Equal(attempt, result);
            Assert.Equal(options.MaxRetryCount, attempt - 1);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RetryHelper")]
        public static void Execute_RetryTimeout()
        {
            // Arrange
            var options = new RetryOptions()
            {
                MaxRetryCount = 2,
                RetryTimeout = 0
            };
            var attempt = 0;

            // Act
            var result = RetryHelper.Execute(options, () =>
            {
                attempt++;
                return false;
            });

            // Assert
            Assert.Equal(attempt, result);
            Assert.Equal(1, attempt);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RetryHelper")]
        public static void Execute_RetryFails()
        {
            // Arrange
            var options = new RetryOptions()
            {
                MaxRetryCount = 1,
                RetryFails = true
            };

            // Act
            Assert.Throws<TimeoutException>(() => RetryHelper.Execute(options, () => false));

            // Assert
        }
    }
}
