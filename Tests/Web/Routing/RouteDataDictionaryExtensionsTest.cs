using System.Web.Routing;
using ReusableLibrary.Web.Routing;
using Xunit;

namespace ReusableLibrary.Web.Tests.Routing
{
    public sealed class RouteDataDictionaryExtensionsTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Extensions, "RouteDataDictionaryExtensions")]
        public static void AddRange()
        {
            // Arrange     
            var routeValues = new RouteValueDictionary();
            var that = new RouteValueDictionary(new { x = 1, y = 2 });

            // Act
            routeValues.AddRange(that);

            // Assert
            Assert.Equal(2, routeValues.Count);
            Assert.Equal(1, routeValues["x"]);
            Assert.Equal(2, routeValues["y"]);
        }
    }
}
