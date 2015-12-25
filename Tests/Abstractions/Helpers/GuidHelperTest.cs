using System;
using System.Collections.Generic;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class GuidHelperTest
    {
        private static readonly Random g_random = new Random();

        public static IEnumerable<object[]> RandomIdSequence
        {
            get
            {
                return EnumerableHelper.Translate(
                    RandomHelper.NextSequence(g_random, i => Guid.NewGuid()),
                    id => new object[] { id });
            }
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "GuidHelper")]
        public static void IsEmpty()
        {
            // Arrange

            // Act
            Assert.True(GuidHelper.IsEmpty(Guid.Empty));
            Assert.False(GuidHelper.IsEmpty(Guid.NewGuid()));

            // Assert
        }

        [Theory]
        [InlineData("a4af2f54-e988-4f5c-bfd6-351c79299b74", "VC-vpIjpXE-_1jUceSmbdA")]
        [InlineData("d17aba88-19c3-400e-adee-3ecf935db272", "iLp60cMZDkCt7j7Pk12ycg")]
        [InlineData("39ae13ee-202a-42d1-9117-6fb6fdd169a4", "7hOuOSog0UKRF2-2_dFppA")]
        [Trait(Constants.TraitNames.Helpers, "GuidHelper")]
        public static void Shrink(string id, string expected)
        {
            // Arrange

            // Act
            var result = GuidHelper.Shrink(new Guid(id));

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "GuidHelper")]
        public static void Shrink_Target_IsEmpty()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => GuidHelper.Shrink(Guid.Empty));

            // Assert
        }

        [Theory]
        [PropertyData("RandomIdSequence")]
        [Trait(Constants.TraitNames.Helpers, "GuidHelper")]
        public static void Random_Shrink_ToGuid(Guid id)
        {
            // Arrange

            // Act
            var result = StringHelper.ToGuid(GuidHelper.Shrink(id));

            // Assert
            Assert.Equal(id, result);
        }
    }
}
