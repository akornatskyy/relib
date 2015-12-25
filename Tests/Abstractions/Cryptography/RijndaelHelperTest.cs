using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Cryptography;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public static class RijndaelHelperTest
    {
        [Theory]
        [InlineData("47DEQpj8HBSa+/TImW+5JA==", "J65B5GSbk0yklZkbeFK4VQ==", 128, "")]
        [InlineData("OQEhMubtASzQvBShp99X7M89Nb1yRiCV", "zz01vXJGIJW4SA3YH7Z6NA==", 192, "wkx@03l!*")]
        [InlineData("OQEhMubtASzQvBShp99X7M89Nb1yRiCVuEgN2B+2ejQ=", "zz01vXJGIJW4SA3YH7Z6NA==", 256, "wkx@03l!*")]        
        [Trait(Constants.TraitNames.Cryptography, "RijndaelHelper")]
        public static void CreateKeyVector(string key, string iv, int keySize, string passphrase)
        {
            // Arrange

            // Act
            var result = RijndaelHelper.CreateKeyVector(passphrase, keySize);

            // Assert
            Assert.Equal(key, Convert.ToBase64String(result.First));
            Assert.Equal(iv, Convert.ToBase64String(result.Second));
        }

        [Theory]
        [InlineData(127)]
        [InlineData(257)]
        [Trait(Constants.TraitNames.Cryptography, "RijndaelHelper")]
        public static void CreateKeyVector_KeySize_OutOfRange(int keySize)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(()
                => RijndaelHelper.CreateKeyVector("test", keySize));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "RijndaelHelper")]
        public static void CreateEncryptor()
        {
            // Arrange
            var provider = new RijndaelManaged();

            // Act
            var result = RijndaelHelper.CreateEncryptor(provider, "sdf2qw2@", 128);

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "RijndaelHelper")]
        public static void CreateDecryptor()
        {
            // Arrange
            var provider = new RijndaelManaged();

            // Act
            var result = RijndaelHelper.CreateDecryptor(provider, "sdf2qw2@", 256);

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }
    }
}
