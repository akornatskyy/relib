using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Cryptography;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public static class RC2HelperTest
    {
        [Theory]
        [InlineData("2jmj7l4=", "lWAYkK/YBwk=", 40, "")]
        [InlineData("a3pggA/CNWs=", "qV5GchjoxQM=", 64, "wkx@03l!*")]
        [InlineData("a3pggA/CNWuIkHW6qV5Gcg==", "qV5GchjoxQM=", 128, "wkx@03l!*")]        
        [Trait(Constants.TraitNames.Cryptography, "RC2Helper")]
        public static void CreateKeyVector(string key, string iv, int keySize, string passphrase)
        {
            // Arrange

            // Act
            var result = RC2Helper.CreateKeyVector(passphrase, keySize);

            // Assert
            Assert.Equal(key, Convert.ToBase64String(result.First));
            Assert.Equal(iv, Convert.ToBase64String(result.Second));
        }

        [Theory]
        [InlineData(39)]
        [InlineData(129)]
        [Trait(Constants.TraitNames.Cryptography, "RC2Helper")]
        public static void CreateKeyVector_KeySize_OutOfRange(int keySize)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() 
                => RC2Helper.CreateKeyVector("test", keySize));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "RC2Helper")]
        public static void CreateEncryptor()
        {
            // Arrange
            var provider = new RC2CryptoServiceProvider();

            // Act
            var result = RC2Helper.CreateEncryptor(provider, "sdf2qw2@", 128);

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "RC2Helper")]
        public static void CreateDecryptor()
        {
            // Arrange
            var provider = new RC2CryptoServiceProvider();

            // Act
            var result = RC2Helper.CreateDecryptor(provider, "sdf2qw2@", 128);

            // Assert
            Assert.NotNull(result);
            result.Dispose();
            provider.Clear();
        }
    }
}
