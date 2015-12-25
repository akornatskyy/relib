using System.Reflection;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class EnumHelperTest
    {
        [Theory]
        [InlineData(true, BindingFlags.CreateInstance, BindingFlags.CreateInstance)]
        [InlineData(true, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly, 
            BindingFlags.CreateInstance)]
        [InlineData(true, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly, 
            BindingFlags.CreateInstance | BindingFlags.DeclaredOnly)]
        [InlineData(true, BindingFlags.CreateInstance, BindingFlags.Default /* 0 */)]
        [InlineData(false, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly, 
            BindingFlags.ExactBinding)]
        [InlineData(false, BindingFlags.CreateInstance, 
            BindingFlags.ExactBinding | BindingFlags.DeclaredOnly)]
        [Trait(Constants.TraitNames.Helpers, "EnumHelper")]
        public static void HasAll(bool expected, BindingFlags source, BindingFlags flags)
        {
            // Arrange

            // Act
            var result = EnumHelper.HasAll(source, flags);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true, BindingFlags.CreateInstance, BindingFlags.CreateInstance)]
        [InlineData(true, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly,
            BindingFlags.CreateInstance)]
        [InlineData(true, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly,
            BindingFlags.CreateInstance | BindingFlags.DeclaredOnly)]
        [InlineData(true, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly,
            BindingFlags.ExactBinding | BindingFlags.DeclaredOnly)]
        [InlineData(true, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly,
            BindingFlags.ExactBinding | BindingFlags.CreateInstance)]
        [InlineData(false, BindingFlags.CreateInstance, BindingFlags.Default /* 0 */)]
        [InlineData(false, BindingFlags.CreateInstance | BindingFlags.DeclaredOnly,
            BindingFlags.ExactBinding)]
        [Trait(Constants.TraitNames.Helpers, "EnumHelper")]
        public static void HasAny(bool expected, BindingFlags source, BindingFlags flags)
        {
            // Arrange

            // Act
            var result = EnumHelper.HasAny(source, flags);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
