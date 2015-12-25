using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Cryptography;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public static class TripleDESHelperTest
    {
        [Theory]
        [InlineData("47DEQpj8HBSa+/TImW+5JA==", "pJWZG3hSuFU=", 128, "")]
        [InlineData("OQEhMubtASzQvBShp99X7M89Nb1yRiCV", "uEgN2B+2ejQ=", 192, "wkx@03l!*")]
        [Trait(Constants.TraitNames.Cryptography, "TripleDESHelper")]
        public static void CreateKeyVector(string key, string iv, int keySize, string passphrase)
        {
            // Arrange

            // Act
            var result = TripleDESHelper.CreateKeyVector(passphrase, keySize);

            // Assert
            Assert.Equal(key, Convert.ToBase64String(result.First));
            Assert.Equal(iv, Convert.ToBase64String(result.Second));
        }

        [Theory]
        [InlineData(127)]
        [InlineData(193)]
        [Trait(Constants.TraitNames.Cryptography, "TripleDESHelper")]
        public static void CreateKeyVector_KeySize_OutOfRange(int keySize)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(()
                => TripleDESHelper.CreateKeyVector("test", keySize));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "TripleDESHelper")]
        public static void CreateEncryptor()
        {
            // Arrange
            var provider = new TripleDESCryptoServiceProvider();

            // Act
            var result = TripleDESHelper.CreateEncryptor(provider, "sdf2qw2@", 192);

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "TripleDESHelper")]
        public static void CreateDecryptor()
        {
            // Arrange
            var provider = new TripleDESCryptoServiceProvider();

            // Act
            var result = TripleDESHelper.CreateDecryptor(provider, "sdf2qw2@", 128);

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }
    }
}
