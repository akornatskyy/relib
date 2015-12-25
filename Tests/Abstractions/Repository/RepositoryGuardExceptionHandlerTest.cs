using System;
using ReusableLibrary.Abstractions.Repository;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class RepositoryGuardExceptionHandlerTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardExceptionHandler")]
        public static void Exception_Is_Null()
        {
            // Arrange
            var handler = new RepositoryGuardExceptionHandler();

            // Act
            var result = handler.HandleException(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardExceptionHandler")]
        public static void Ignore_Is_Not_Set()
        {
            // Arrange
            var handler = new RepositoryGuardExceptionHandler();

            // Act
            var result = handler.HandleException(new RepositoryGuardException());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(RepositoryFailureException))]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardExceptionHandler")]
        public static void NoMatch(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type);
            var handler = new RepositoryGuardExceptionHandler();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(100400)]
        [InlineData(100201)]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardExceptionHandler")]
        public static void Handles_Code(int code)
        {
            // Arrange
            var handler = new RepositoryGuardExceptionHandler();
            handler.Ignore = new[] { 100400, 100201 };

            // Act
            var result = handler.HandleException(new RepositoryGuardException(code));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(100500)]
        [InlineData(452403)]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardExceptionHandler")]
        public static void Does_Not_Handle_Code(int code)
        {
            // Arrange
            var handler = new RepositoryGuardExceptionHandler();
            handler.Ignore = new[] { 100400, 100201 };

            // Act
            var result = handler.HandleException(new RepositoryGuardException(code));

            // Assert
            Assert.False(result);
        }
    }
}
