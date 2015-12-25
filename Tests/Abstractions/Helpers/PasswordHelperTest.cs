using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public static class PasswordHelperTest
    {
        private static readonly Pair<int, string>[] PasswordChars = new[] 
        { 
            new Pair<int, string>(8, StringHelper.AlphabetLowerCase),
            new Pair<int, string>(5, StringHelper.AlphabetUpperCase),
            new Pair<int, string>(4, StringHelper.Numeric),
            new Pair<int, string>(3, StringHelper.Special)
        };

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "PasswordHelper")]
        public static void NextPassword()
        {
            // Arrange
            var random = new Random(RandomHelper.Seed());

            // Act
            var result = PasswordHelper.NextPassword(random, PasswordChars);

            // Assert
            Assert.Equal(8, CountAnyOf(result, StringHelper.AlphabetLowerCase.ToCharArray()));
            Assert.Equal(5, CountAnyOf(result, StringHelper.AlphabetUpperCase.ToCharArray()));
            Assert.Equal(4, CountAnyOf(result, StringHelper.Numeric.ToCharArray()));
            Assert.Equal(3, CountAnyOf(result, StringHelper.Special.ToCharArray()));
        }

        private static int CountAnyOf(string source, char[] chars)
        {
            int count = 0;
            int index = 0;
            while ((index = source.IndexOfAny(chars, index)) != -1)
            {
                index++;
                count++;
            }

            return count;
        }
    }
}
