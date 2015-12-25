using System.Text;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Memcached.Protocol;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Memcached.Tests.Protocol
{
    public sealed class ValidKeyEncoderTest
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("a_b", "a b")]
        [InlineData("___c", "\r \nc")]
        [Trait(Constants.TraitNames.Protocol, "ValidKeyEncoder")]
        public static void GetBytes(string expected, string value)
        {
            // Arrange
            var encoder = new ValidKeyEncoder(new TextEncoder(Encoding.ASCII));

            // Act
            var result = encoder.GetBytes(value);

            // Assert
            Assert.Equal(expected, Encoding.ASCII.GetString(result));
        }
    }
}
