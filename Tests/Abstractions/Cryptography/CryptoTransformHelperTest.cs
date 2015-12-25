using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Cryptography;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public sealed class CryptoTransformHelperTest : IDisposable
    {
        private static readonly Random g_random = new Random();
        private readonly SymmetricAlgorithm m_algorithm;

        public CryptoTransformHelperTest()
        {
            var provider = new SymmetricAlgorithmProvider<RC2CryptoServiceProvider>(
                    new RC2KeyVectorProvider("P@ssw0rd", 40));
            m_algorithm = provider.Create();
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_algorithm.Clear();
        }

        #endregion

        [Theory]
        [InlineData("qXXOjreGLzs=", "test")]
        [InlineData("w9L5ytcMjfK/rSvUt8/WCQ==", "encrypt me")]
        [Trait(Constants.TraitNames.Cryptography, "CryptoTransformHelper")]
        public void Encrypt(string expected, string input)
        {
            // Arrange            
            using (var transform = m_algorithm.CreateEncryptor())
            {
                // Act
                var result = Convert.ToBase64String(CryptoTransformHelper.Encrypt(transform, 
                    Encoding.UTF8.GetBytes(input)));

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expected, result);
            }
        }

        [Theory]
        [InlineData("test", "qXXOjreGLzs=")]
        [InlineData("encrypt me", "w9L5ytcMjfK/rSvUt8/WCQ==")]
        [Trait(Constants.TraitNames.Cryptography, "CryptoTransformHelper")]
        public void Decrypt(string expected, string input)
        {
            // Arrange
            using (var transform = m_algorithm.CreateDecryptor())
            {
                // Act
                var result = Encoding.UTF8.GetString(CryptoTransformHelper.Decrypt(transform,
                    Convert.FromBase64String(input)));

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        [Trait(Constants.TraitNames.Cryptography, "CryptoTransformHelper")]
        public void Encrypt_Decrypt()
        {
            // Arrange            
            var input = RandomHelper.NextSentence(g_random, RandomHelper.NextInt(g_random, 20, 40));
            using (var encryptor = m_algorithm.CreateEncryptor())
            {
                using (var decryptor = m_algorithm.CreateDecryptor())
                {
                    // Act
                    var result = Encoding.UTF8.GetString(CryptoTransformHelper.Decrypt(decryptor,
                        CryptoTransformHelper.Encrypt(encryptor, Encoding.UTF8.GetBytes(input))));

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal(input, result);
                }
            }
        }
    }
}
