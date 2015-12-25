using System;
using System.Collections.Generic;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public static class BitConverterHelperTest
    {
        private static readonly Random g_random = new Random();

        public static IEnumerable<object[]> RandomSentenceSequence
        {
            get
            {
                return EnumerableHelper.Translate(
                    RandomHelper.NextSequence(g_random, i => 
                        RandomHelper.NextSentence(g_random, RandomHelper.NextInt(g_random, 20, 40 + i))),
                    s => new object[] { s });
            }
        }

        [Theory]
        [PropertyData("RandomSentenceSequence")]
        [Trait(Constants.TraitNames.Helpers, "BitConverterHelper")]
        public static void GetBytes_String_With_Dashes(string expected)
        {
            // Arrange
            var input = BitConverter.ToString(Encoding.UTF8.GetBytes(expected));

            // Act
            var result = Encoding.UTF8.GetString(BitConverterHelper.GetBytes(input));

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [PropertyData("RandomSentenceSequence")]
        [Trait(Constants.TraitNames.Helpers, "BitConverterHelper")]
        public static void GetBytes_String_HasNo_Dashes(string expected)
        {
            // Arrange
            var input = BitConverter.ToString(Encoding.UTF8.GetBytes(expected)).Replace("-", string.Empty);

            // Act
            var result = Encoding.UTF8.GetString(BitConverterHelper.GetBytes(input));

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("te-st")]
        [InlineData("abc")]
        [InlineData("abcde")]
        [InlineData("ab-c")]
        [InlineData("ab-cd-e")]
        [Trait(Constants.TraitNames.Helpers, "BitConverterHelper")]
        public static void GetBytes_Throws_FormatException(string input)
        {
            // Arrange

            // Act
            Assert.Throws<FormatException>(() => BitConverterHelper.GetBytes(input));

            // Assert
        }

        [Theory]
        [InlineData(-1, "00000000-0000-0000-0000-0000ffffffff")]
        [InlineData(0x0fffaabb, "00000000-0000-0000-0000-0000bbaaff0f")]
        [InlineData(0xff, "00000000-0000-0000-0000-0000ff000000")]
        [Trait(Constants.TraitNames.Helpers, "BitConverterHelper")]
        public static void Int_ToGuid(int input, string expected)
        {
            // Arrange

            // Act
            var result = BitConverterHelper.ToGuid(input).ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1, "00000000-0000-0000-ffff-ffffffffffff")]
        [InlineData(0x0fffaabb, "00000000-0000-0000-bbaa-ff0f00000000")]
        [InlineData(0xff, "00000000-0000-0000-ff00-000000000000")]
        [Trait(Constants.TraitNames.Helpers, "BitConverterHelper")]
        public static void Long_ToGuid(long input, string expected)
        {
            // Arrange

            // Act
            var result = BitConverterHelper.ToGuid(input).ToString();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
