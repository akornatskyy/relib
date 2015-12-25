using System;
using System.Web;
using ReusableLibrary.Web.Integration;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Integration
{
    public sealed class HttpExceptionHandlerTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Integration, "HttpExceptionHandler")]
        public static void Exception_Is_Null()
        {
            // Arrange
            var handler = new HttpExceptionHandler();

            // Act
            var result = handler.HandleException(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "HttpExceptionHandler")]
        public static void Ignore_Is_Not_Set()
        {
            // Arrange
            var handler = new HttpExceptionHandler();

            // Act
            var result = handler.HandleException(new HttpException());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(InvalidTimeZoneException))]
        [Trait(Constants.TraitNames.Integration, "HttpExceptionHandler")]
        public static void NoMatch(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type);
            var handler = new HttpExceptionHandler();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(400)]
        [InlineData(404)]
        [Trait(Constants.TraitNames.Integration, "HttpExceptionHandler")]
        public static void Handles_Http_Code(int code)
        {
            // Arrange
            var handler = new HttpExceptionHandler();
            handler.Ignore = new[] { 400, 404 };

            // Act
            var result = handler.HandleException(new HttpException(code, "test"));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(403)]
        [Trait(Constants.TraitNames.Integration, "HttpExceptionHandler")]
        public static void Does_Not_Handle_Http_Code(int code)
        {
            // Arrange
            var handler = new HttpExceptionHandler();
            handler.Ignore = new[] { 400, 404 };

            // Act
            var result = handler.HandleException(new HttpException(code, "test"));

            // Assert
            Assert.False(result);
        }
    }
}
