using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class ExceptionHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ExceptionHelper")]
        public static void Errors_NoInner()
        {
            // Arrange
            var ex = new FormatException();

            // Act
            var errors = ExceptionHelper.Errors(ex);

            // Assert
            Assert.NotNull(errors);
            Assert.Equal(1, errors.Length);
            Assert.Equal(ex, errors[0]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ExceptionHelper")]
        public static void Errors_WithInner()
        {
            // Arrange
            var ex = new FormatException("x", new InvalidOperationException());

            // Act
            var errors = ExceptionHelper.Errors(ex);

            // Assert
            Assert.NotNull(errors);
            Assert.Equal(2, errors.Length);
            Assert.Equal(ex, errors[0]);
            Assert.Equal(ex.InnerException, errors[1]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ExceptionHelper")]
        public static void Find()
        {
            // Arrange
            var ex = new FormatException();

            // Act
            var result = ExceptionHelper.Find<FormatException>(ex);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ex);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ExceptionHelper")]
        public static void Find_Inner()
        {
            // Arrange
            var ex = new FormatException("x", new InvalidOperationException());

            // Act
            var result = ExceptionHelper.Find<InvalidOperationException>(ex);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, ex.InnerException);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ExceptionHelper")]
        public static void Find_NotFound()
        {
            // Arrange
            var ex = new FormatException("x", new InvalidOperationException());

            // Act
            var result = ExceptionHelper.Find<ArgumentNullException>(ex);

            // Assert
            Assert.Null(result);
        }
    }
}
