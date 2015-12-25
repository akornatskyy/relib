using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class DecimalConverterTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Models, "DecimalConverter")]
        public static void CanConvertFromInt32()
        {
            // Arrange
            var converter = new DecimalConverter();

            // Act
            var result = converter.CanConvertFrom(typeof(int));

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecimalConverter")]
        public static void CanConvertToInt32()
        {
            // Arrange
            var converter = new DecimalConverter();

            // Act
            var result = converter.CanConvertTo(typeof(int));

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecimalConverter")]
        public static void ConvertFromInt32()
        {
            // Arrange
            var converter = new DecimalConverter();

            // Act
            var result = converter.ConvertFrom(100);

            // Assert
            Assert.Equal(100M, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecimalConverter")]
        public static void ConvertToInt32()
        {
            // Arrange
            var converter = new DecimalConverter();

            // Act
            var result = converter.ConvertTo(100M, typeof(int));

            // Assert
            Assert.Equal(100, result);
        }
    }
}
