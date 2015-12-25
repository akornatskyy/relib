using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class RangeHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RangeHelper")]
        public static void Exchange_From_IsGreater_To()
        {
            // Arrange
            var range = new Range<int>(12, 10);

            // Act
            var result = RangeHelper.ExchangeIfFromGreaterTo(range, (from, to) => from.CompareTo(to));

            // Assert
            Assert.Equal(10, result.From);
            Assert.Equal(12, result.To);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "RangeHelper")]
        public static void Exchange_From_IsNotGreater_To()
        {
            // Arrange
            var range = new Range<int>(10, 12);

            // Act
            var result = RangeHelper.ExchangeIfFromGreaterTo(range, (from, to) => from.CompareTo(to));

            // Assert
            Assert.Equal(10, result.From);
            Assert.Equal(12, result.To);
        }
    }
}
