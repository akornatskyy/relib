using System;
using ReusableLibrary.Abstractions.Repository;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class RepositoryGuardAreaExceptionHandlerTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardAreaExceptionHandler")]
        public static void Exception_Is_Null()
        {
            // Arrange
            var handler = new RepositoryGuardAreaExceptionHandler();

            // Act
            var result = handler.HandleException(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardAreaExceptionHandler")]
        public static void Ignore_Is_Not_Set()
        {
            // Arrange
            var handler = new RepositoryGuardAreaExceptionHandler();

            // Act
            var result = handler.HandleException(new RepositoryGuardAreaException());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(RepositoryFailureException))]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardAreaExceptionHandler")]
        public static void NoMatch(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type);
            var handler = new RepositoryGuardAreaExceptionHandler();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardAreaExceptionHandler")]
        public static void Handles_Area(string area)
        {
            // Arrange
            var handler = new RepositoryGuardAreaExceptionHandler();
            handler.Ignore = new[] { "A", "B" };

            // Act
            var result = handler.HandleException(new RepositoryGuardAreaException(area, 100));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("X")]
        [InlineData("Y")]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardAreaExceptionHandler")]
        public static void Does_Not_Handle_Area(string area)
        {
            // Arrange
            var handler = new RepositoryGuardAreaExceptionHandler();
            handler.Ignore = new[] { "A", "B" };

            // Act
            var result = handler.HandleException(new RepositoryGuardAreaException(area, 100));

            // Assert
            Assert.False(result);
        }
    }
}
