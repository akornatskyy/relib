using System.Web.Routing;
using ReusableLibrary.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Routing
{
    public sealed class ChoiceRouteConstraintTest
    {
        [Theory]
        [InlineData("x", true)]
        [InlineData("y", true)]
        [InlineData("z", true)]
        [InlineData("a", false)]
        [InlineData("b", false)]
        [Trait(Constants.TraitNames.Routing, "ChoiceRouteConstraint")]
        public static void Match(string input, bool expected)
        {
            // Arrange          
            var constraint = new ChoiceRouteConstraint(new[] { "x", "y", "z" });
            var values = new RouteValueDictionary(new { p = input });

            // Act
            var result = constraint.Match(null, null, "p", values, RouteDirection.IncomingRequest);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "ChoiceRouteConstraint")]
        public static void Match_EmptyConstraint()
        {
            // Arrange          
            var constraint = new ChoiceRouteConstraint(new string[] { });
            var values = new RouteValueDictionary(new { p = "x" });

            // Act
            var result = constraint.Match(null, null, "p", values, RouteDirection.IncomingRequest);

            // Assert
            Assert.False(result);
        }
    }
}
