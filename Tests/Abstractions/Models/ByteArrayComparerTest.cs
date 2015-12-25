using System.Text;
using ReusableLibrary.Abstractions.Models;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class ByteArrayComparerTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "ByteArrayComparer")]
        public static void CompareNulls()
        {
            // Arrange

            // Act
            Assert.Equal(0, ByteArrayComparer.Default.Compare(null, null));
            Assert.Equal(-1, ByteArrayComparer.Default.Compare(null, new byte[] { }));
            Assert.Equal(1, ByteArrayComparer.Default.Compare(new byte[] { }, null));

            // Assert            
        }

        [Theory]
        [InlineData(-1, "123", "234")]
        [InlineData(-1, "123", "124")]
        [InlineData(0, "123", "123")]
        [InlineData(1, "1234", "1233")]
        [InlineData(1, "234", "123")]
        [Trait(Constants.TraitNames.Models, "ByteArrayComparer")]
        public static void Compare(int expected, string x, string y)
        {
            // Arrange
            var encoding = Encoding.ASCII;

            // Act
            var result = ByteArrayComparer.Default.Compare(encoding.GetBytes(x), encoding.GetBytes(y));

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
