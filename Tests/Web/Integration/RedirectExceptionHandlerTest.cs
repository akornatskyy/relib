using System;
using ReusableLibrary.Web.Integration;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Integration
{
    public sealed class RedirectExceptionHandlerTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Integration, "RedirectExceptionHandler")]
        public static void Exception_Is_Null()
        {
            // Arrange
            var handler = new RedirectExceptionHandler<InvalidOperationException>();

            // Act
            var result = handler.HandleException(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "RedirectExceptionHandler")]
        public static void Exception_HasNoMatch()
        {
            // Arrange
            var handler = new RedirectExceptionHandler<InvalidOperationException>();

            // Act
            var result = handler.HandleException(new ArgumentException());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "RedirectExceptionHandler")]
        public static void Exception_HasNoMatch_CheckInner()
        {
            // Arrange
            var handler = new RedirectExceptionHandler<InvalidOperationException>() 
            { 
                CheckInner = true
            };

            // Act
            var result = handler.HandleException(new ArgumentException("x", new NotSupportedException()));

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [Trait(Constants.TraitNames.Integration, "RedirectExceptionHandler")]
        public static void RedirectUrl_Is_NullOrEmpty(string url)
        {
            // Arrange
            var handler = new RedirectExceptionHandler<InvalidOperationException>();
            handler.RedirectUrl = url;

            // Act
            var result = handler.HandleException(new InvalidOperationException());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("http://localhost:82/en/error/http400?aspxerrorpath=/en/home/about", "http://localhost:82/en/home/about?id=5")]
        [InlineData("http://localhost:82/en/error/http400?aspxerrorpath=/ru/home/about", "http://localhost:82/ru/home/about")]
        [Trait(Constants.TraitNames.Integration, "RedirectExceptionHandler")]
        public static void BuildAspxErrorPath(string expected, string errorUrl)
        {
            // Arrange
            var handler = new RedirectExceptionHandler<InvalidOperationException>();
            handler.RedirectUrl = "/en/error/http400";

            // Act
            var result = handler.BuildAspxErrorPath(new Uri(errorUrl));

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
