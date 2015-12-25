using System;
using ReusableLibrary.Abstractions.Repository;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Repository
{
    public sealed class DbConnectionStringProviderTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [Trait(Constants.TraitNames.Repository, "DbConnectionStringProvider")]
        public static void Initialized_With_Null(string name)
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new DbConnectionStringProvider(name));
        }

        [Fact]
        [Trait(Constants.TraitNames.Repository, "DbConnectionStringProvider")]
        public static void ConnectionString()
        {
            // Arrange
            var provider = new DbConnectionStringProvider("LocalSqlServer");

            // Act
            var result = provider.ConnectionString;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(@"data source=.\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true", result);
        }
    }
}
