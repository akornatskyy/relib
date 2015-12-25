using System;
using ReusableLibrary.Abstractions.Repository;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class RepositoryGuardExceptionTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardException")]
        public static void Constructor_With_No_Parameters()
        {
            // Arrange

            // Act
            var ex = new RepositoryGuardException();

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test")]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardException")]
        public static void Constructor_With_Message(string message)
        {
            // Arrange            

            // Act
            var ex = new RepositoryGuardException(message);

            // Assert
            Assert.NotNull(ex);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentNullException))]
        [InlineData("test", null)]
        [InlineData("test", typeof(ArgumentNullException))]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardException")]
        public static void Constructor_With_Message_And_InnerException(string message, Type type)
        {
            // Arrange            
            Exception inner = type != null ? (Exception)Activator.CreateInstance(type) : null;

            // Act
            var ex = new RepositoryGuardException(message, inner);

            // Assert
            Assert.NotNull(ex);
            Assert.Equal(inner, ex.InnerException);
        }

        [Theory]
        [InlineData(12059)]
        [InlineData(10456)]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardException")]
        public static void Constructor_With_ErrorNumber(int number)
        {
            // Arrange            

            // Act
            var ex = new RepositoryGuardException(number);

            // Assert
            Assert.NotNull(ex.Message);
            Assert.Equal(number, ex.Number);
        }

        [Theory]
        [InlineData(100, typeof(ArgumentNullException))]
        [InlineData(101, null)]
        [Trait(Constants.TraitNames.Repository, "RepositoryGuardException")]
        public static void Constructor_With_FailureArea_And_InnerException(int number, Type type)
        {
            // Arrange            
            Exception inner = type != null ? (Exception)Activator.CreateInstance(type) : null;

            // Act
            var ex = new RepositoryGuardException(number, inner);

            // Assert
            Assert.NotNull(ex.Message);
            Assert.Equal(number, ex.Number);
            Assert.Equal(inner, ex.InnerException);
        }
    }
}
