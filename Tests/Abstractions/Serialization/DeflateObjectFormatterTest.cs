using System;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class DeflateObjectFormatterTest
    {
        private static readonly Random g_random = new Random();

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "DeflateObjectFormatter")]
        public static void Compress_Decompress()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes(RandomHelper.NextSentence(g_random, RandomHelper.NextInt(g_random, 10, 200)));
            var formatter = new DeflateObjectFormatter(null);

            // Act
            var decompressed = formatter.Decompress(formatter.Compress(new ArraySegment<byte>(data)));
            var result = new byte[decompressed.Count];
            Buffer.BlockCopy(decompressed.Array, decompressed.Offset, result, 0, decompressed.Count);

            // Assert
            Assert.Equal(data, result);
        }
    }
}
