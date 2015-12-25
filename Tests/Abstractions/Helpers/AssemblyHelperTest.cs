using System;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class AssemblyHelperTest
    {
        [Theory]
        [InlineData(typeof(string), "4.0.0.0")]
        [Trait(Constants.TraitNames.Helpers, "AssemblyHelper")]
        public static void ToLongVersionString(Type type, string expected)
        {
            // Arrange
            var assembly = type.Assembly;

            // Act
            var formatted = AssemblyHelper.ToLongVersionString(assembly);

            // Assert
            Assert.Contains(expected, formatted, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(typeof(string), "4.0")]
        [Trait(Constants.TraitNames.Helpers, "AssemblyHelper")]
        public static void ToShortVersionString(Type type, string expected)
        {
            // Arrange
            var assembly = type.Assembly;

            // Act
            var formatted = AssemblyHelper.ToShortVersionString(assembly);

            // Assert
            Assert.Equal(expected, formatted);
        }
    }
}
