using System;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Cryptography;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Cryptography
{
    public sealed class SymmetricAlgorithmContextTest : IDisposable
    {
        private readonly ISymmetricAlgorithmContext m_context;

        public SymmetricAlgorithmContextTest()
        {
            m_context = new SymmetricAlgorithmContext(
                new SymmetricAlgorithmProvider<RC2CryptoServiceProvider>(
                    new RC2KeyVectorProvider("P@ssw0rd", 40)));
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_context.Dispose();
        }

        #endregion

        [Theory]
        [InlineData("qXXOjreGLzs=", "test")]
        [InlineData("w9L5ytcMjfK/rSvUt8/WCQ==", "encrypt me")]
        [Trait(Constants.TraitNames.Cryptography, "SymmetricAlgorithmContext")]
        public void EncryptorContext(string expected, string input)
        {
            // Arrange
            string result = null;

            // Act
            m_context.EncryptorContext(transform =>
            {
                result = Convert.ToBase64String(CryptoTransformHelper.Encrypt(transform,
                    Encoding.UTF8.GetBytes(input)));
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test", "qXXOjreGLzs=")]
        [InlineData("encrypt me", "w9L5ytcMjfK/rSvUt8/WCQ==")]
        [Trait(Constants.TraitNames.Cryptography, "SymmetricAlgorithmContext")]
        public void DecryptorContext(string expected, string input)
        {
            // Arrange
            string result = null;

            // Act
            m_context.DecryptorContext(transform =>
            {
                result = Encoding.UTF8.GetString(CryptoTransformHelper.Decrypt(transform, 
                    Convert.FromBase64String(input)));
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }
    }
}
