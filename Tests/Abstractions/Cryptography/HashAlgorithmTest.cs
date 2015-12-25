using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Cryptography;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public sealed class HashAlgorithmTest
    {
        private static readonly Encoding g_encoding = Encoding.UTF8;

        [Theory]
        [InlineData("hZtbkg==", typeof(FNV32HashAlgorithm), "test-test")]
        [InlineData("gAmgtA==", typeof(FNV32ModifiedHashAlgorithm), "test-test")]
        [InlineData("X9I16w==", typeof(FNVa32HashAlgorithm), "test-test")]
        [InlineData("hSvKuBpgUsk=", typeof(FNV64HashAlgorithm), "test-test")]
        [Trait(Constants.TraitNames.Cryptography, "HashAlgorithm")]
        public static void ComputeHash(string expected, Type type, string value)
        {
            // Arrange
            var algorithm = (HashAlgorithm)Activator.CreateInstance(type);

            // Act
            var result = algorithm.ComputeHash(g_encoding.GetBytes(value));
            algorithm.Clear();

            // Assert
            Assert.Equal(expected, Convert.ToBase64String(result));
        }
    }
}
