using System;
using ReusableLibrary.Abstractions.Repository;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class RepositoryFailureExceptionTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Repository, "RepositoryFailureException")]
        public static void Constructor_With_No_Parameters()
        {
            // Arrange
            
            // Act
            var ex = new RepositoryFailureException();

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test")]
        [Trait(Constants.TraitNames.Repository, "RepositoryFailureException")]
        public static void Constructor_With_Message(string message)
        {
            // Arrange            

            // Act
            var ex = new RepositoryFailureException(message);

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData("test", null)]
        [InlineData("test", typeof(ArgumentNullException))]
        [Trait(Constants.TraitNames.Repository, "RepositoryFailureException")]
        public static void Constructor_With_Message_And_InnerException(string message, Type type)
        {
            // Arrange            
            Exception inner = type != null ? (Exception)Activator.CreateInstance(type) : null;

            // Act
            var ex = new RepositoryFailureException(message, inner);

            // Assert
            Assert.NotNull(ex);
            Assert.Equal(inner, ex.InnerException);
        }
    }
}
