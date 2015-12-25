using ReusableLibrary.Abstractions.Helpers;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public static class ObjectHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ObjectHelper")]
        public static void PropertiesToNameValueCollection()
        {
            // Arrange
            var model = new { Test1 = 1, Test2 = "2" };

            // Act
            var result = ObjectHelper.PropertiesToNameValueCollection(model);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("1", result["Test1"]);
            Assert.Equal("2", result["Test2"]);
        }
    }
}
