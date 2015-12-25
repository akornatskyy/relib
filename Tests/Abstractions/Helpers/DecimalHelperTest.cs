using System;
using System.Collections.Generic;
using System.Globalization;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class DecimalHelperTest
    {
        private static readonly Random g_random = new Random();

        public static IEnumerable<object[]> RandomDecimalSequence
        {
            get
            {
                return EnumerableHelper.Translate(
                    RandomHelper.NextSequence(g_random, i => (decimal)g_random.NextDouble() * 1000000M),
                    d => new object[] { d });
            }
        }

        [Theory]
        [PropertyData("RandomDecimalSequence")]
        [Trait(Constants.TraitNames.Helpers, "DecimalHelper")]
        public static void GetBytes_ToDecimal(decimal value)
        {
            // Arrange

            // Act
            var result = DecimalHelper.ToDecimal(DecimalHelper.GetBytes(value), 0);

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("fdHjhmqiAwAAAAAAAAAEAA==", "102300334344.2301")]
        [InlineData("oXEGOdViBAAAAAAAAAAEAA==", "123456783112.2337")]
        [Trait(Constants.TraitNames.Helpers, "DecimalHelper")]
        public static void GetBytes(string expected, string s)
        {
            // Arrange
            var value = decimal.Parse(s, CultureInfo.InvariantCulture);

            // Act
            var result = DecimalHelper.GetBytes(value);

            // Assert
            Assert.Equal(expected, Convert.ToBase64String(result));
        }

        [Theory]
        [InlineData("102300334344.2301", "fdHjhmqiAwAAAAAAAAAEAA==")]
        [InlineData("123456783112.2337", "oXEGOdViBAAAAAAAAAAEAA==")]
        [Trait(Constants.TraitNames.Helpers, "DecimalHelper")]
        public static void ToDecimal(string value, string s)
        {
            // Arrange
            var expected = decimal.Parse(value, CultureInfo.InvariantCulture);

            // Act
            var result = DecimalHelper.ToDecimal(Convert.FromBase64String(s), 0);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DecimalHelper")]
        public static void ToDecimal_Bytes_IsNull()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => DecimalHelper.ToDecimal(null, 0));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DecimalHelper")]
        public static void ToDecimal_StartIndex_OutOfRange()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentOutOfRangeException>(() => DecimalHelper.ToDecimal(new byte[] { }, 1));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "DecimalHelper")]
        public static void ToDecimal_BytesArray_TooSmall()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(() => DecimalHelper.ToDecimal(new byte[] { 1 }, 0));

            // Assert
        }
    }
}
