using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class EncodingTest
    {
        private static readonly Random g_random = new Random();

        [Fact]
        [Trait(Constants.TraitNames.Models, "Encoding")]
        public static void TextEncoder()
        {
            // Arrange
            var value = RandomHelper.NextSentence(g_random, 20);
            var encoder = new TextEncoder(Encoding.ASCII);

            // Act
            var result = encoder.GetBytes(value);

            // Assert
            Assert.Equal(Encoding.ASCII.GetBytes(value), result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Encoding")]
        public static void Base64Encoder()
        {
            // Arrange
            var value = RandomHelper.NextSentence(g_random, 20);
            var encoder = new Base64Encoder(new TextEncoder(Encoding.UTF8));

            // Act
            var result = encoder.GetBytes(value);

            // Assert
            var expected = Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(value)));
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "Encoding")]
        public static void HashEncoder()
        {
            // Arrange
            var value = RandomHelper.NextSentence(g_random, 20);
            var encoder = new HashEncoder(new HashAlgorithmProvider<SHA1CryptoServiceProvider>(),
                new TextEncoder(Encoding.UTF8));

            // Act
            var result = encoder.GetBytes(value);

            // Assert
            var expected = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value));
            Assert.Equal(expected, result);
        }
    }
}
