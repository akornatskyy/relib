using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Cryptography;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public static class DESHelperTest
    {
        [Theory]
        [InlineData("2jmj7l5rSw0=", "MlW/75VgGJA=", "")]
        [InlineData("a3pggA/CNWs=", "iJB1uqleRnI=", "wkx@03l!*")]
        [Trait(Constants.TraitNames.Cryptography, "DESHelper")]
        public static void CreateKeyVector(string key, string iv, string passphrase)
        {
            // Arrange

            // Act
            var result = DESHelper.CreateKeyVector(passphrase);

            // Assert
            Assert.Equal(key, Convert.ToBase64String(result.First));
            Assert.Equal(iv, Convert.ToBase64String(result.Second));
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "DESHelper")]
        public static void CreateEncryptor()
        {
            // Arrange
            var provider = new DESCryptoServiceProvider();

            // Act
            var result = DESHelper.CreateEncryptor(provider, "sdf2qw2@");

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "DESHelper")]
        public static void CreateDecryptor()
        {
            // Arrange
            var provider = new DESCryptoServiceProvider();

            // Act
            var result = DESHelper.CreateDecryptor(provider, "sdf2qw2@");

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }
    }
}
