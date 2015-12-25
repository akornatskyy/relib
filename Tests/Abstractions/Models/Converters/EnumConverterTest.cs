using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Models
{
    public sealed class EnumConverterTest
    {
        private enum Answer 
        {
            None,
            Yes,
            No
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "EnumConverter")]
        public static void CanConvertFromInt32()
        {
            // Arrange
            var converter = new EnumConverter(typeof(Answer));

            // Act
            var result = converter.CanConvertFrom(typeof(int));

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "EnumConverter")]
        public static void CanConvertToInt32()
        {
            // Arrange
            var converter = new EnumConverter(typeof(Answer));

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
            var converter = new EnumConverter(typeof(Answer));

            // Act
            var result = converter.ConvertFrom(1);

            // Assert
            Assert.Equal(Answer.Yes, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Models, "DecimalConverter")]
        public static void ConvertToInt32()
        {
            // Arrange
            var converter = new EnumConverter(typeof(Answer));

            // Act
            var result = converter.ConvertTo(Answer.No, typeof(int));

            // Assert
            Assert.Equal(2, result);
        }
    }
}
