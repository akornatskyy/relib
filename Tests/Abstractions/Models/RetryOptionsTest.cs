using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class RetryOptionsTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "RetryOptions")]
        public static void RetryOptions_Default()
        {
            // Arrange
            var optionsString = string.Empty;

            // Act
            var options = new RetryOptions(optionsString);

            // Assert            
            Assert.Equal(0, options.MaxRetryCount);
            Assert.Equal(0, options.RetryDelay);
            Assert.Equal(false, options.RetryFails);
            Assert.Equal(0, options.RetryTimeout);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "RetryOptions")]
        public static void RetryOptions_OptionsString()
        {
            // Arrange
            var optionsString = "Max Retry Count = 10;Retry Delay = 5;Retry Fails = true;Retry Timeout = 250";

            // Act
            var options = new RetryOptions(optionsString);

            // Assert            
            Assert.Equal(10, options.MaxRetryCount);
            Assert.Equal(5, options.RetryDelay);
            Assert.Equal(true, options.RetryFails);
            Assert.Equal(250, options.RetryTimeout);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "RetryOptions")]
        public static void RetryOptions_OptionsString_WithPrefix_NoMatch()
        {
            // Arrange
            var optionsString = "Max Retry Count = 10;Retry Delay = 5;Retry Fails = true;Retry Timeout = 250";

            // Act
            var options = new RetryOptions(optionsString, "Transfer");

            // Assert            
            Assert.Equal(0, options.MaxRetryCount);
            Assert.Equal(0, options.RetryDelay);
            Assert.Equal(false, options.RetryFails);
            Assert.Equal(0, options.RetryTimeout);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "RetryOptions")]
        public static void RetryOptions_OptionsString_WithPrefix()
        {
            // Arrange
            var optionsString = "Transfer Max Retry Count = 10;Transfer Retry Delay = 5;Transfer Retry Fails = true;Transfer Retry Timeout = 250";

            // Act
            var options = new RetryOptions(optionsString, "Transfer");

            // Assert            
            Assert.Equal(10, options.MaxRetryCount);
            Assert.Equal(5, options.RetryDelay);
            Assert.Equal(true, options.RetryFails);
            Assert.Equal(250, options.RetryTimeout);
        }
    }
}
