using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class SymmetricObjectFormatterTest
    {
        private static readonly Random g_random = new Random();

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SymmetricObjectFormatter")]
        public static void DES_Encrypt_Decrypt()
        {
            // Arrange
            var algorithmProvider = new SymmetricAlgorithmProvider<DESCryptoServiceProvider>(
                new DESKeyVectorProvider("sDE0#2x.4"));

            // Act
            Encrypt_Decrypt(algorithmProvider);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SymmetricObjectFormatter")]
        public static void RC2_Encrypt_Decrypt()
        {
            // Arrange
            var algorithmProvider = new SymmetricAlgorithmProvider<RC2CryptoServiceProvider>(
                new RC2KeyVectorProvider("sDE0#2x.4", 128));

            // Act
            Encrypt_Decrypt(algorithmProvider);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SymmetricObjectFormatter")]
        public static void Rijndael_Encrypt_Decrypt()
        {
            // Arrange
            var algorithmProvider = new SymmetricAlgorithmProvider<RijndaelManaged>(
                new RijndaelKeyVectorProvider("sDE0#2x.4", 256));

            // Act
            Encrypt_Decrypt(algorithmProvider);

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SymmetricObjectFormatter")]
        public static void TripleDES_Encrypt_Decrypt()
        {
            // Arrange
            var algorithmProvider = new SymmetricAlgorithmProvider<TripleDESCryptoServiceProvider>(
                new TripleDESKeyVectorProvider("sDE0#2x.4", 192));

            // Act
            Encrypt_Decrypt(algorithmProvider);

            // Assert
        }    

        private static void Encrypt_Decrypt(ISymmetricAlgorithmProvider provider)
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes(RandomHelper.NextSentence(g_random, RandomHelper.NextInt(g_random, 10, 200)));

            var formatter = new SymmetricObjectFormatter(provider, null);

            // Act
            var decrypted = formatter.Decrypt(formatter.Encrypt(new ArraySegment<byte>(data)));
            var result = new byte[decrypted.Count];
            Buffer.BlockCopy(decrypted.Array, decrypted.Offset, result, 0, decrypted.Count);

            // Assert
            Assert.Equal(data, result);
        }
    }
}
