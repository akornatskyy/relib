using System;
using System.Web.Routing;
using ReusableLibrary.Web.Mvc.Routing;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Routing
{
    public sealed class RegisterIgnoreRoutesTest : IDisposable
    {
        private readonly RegisterIgnoreRoutes m_task = new RegisterIgnoreRoutes();

        [Fact]
        [Trait(Constants.TraitNames.Routing, "RegisterIgnoreRoutes")]
        public void Execute()
        {
            // Arrange

            // Act
            m_task.Execute();

            // Assert
            Assert.Equal(3, RouteTable.Routes.Count);

            var route = RouteTable.Routes[0] as Route;
            Assert.NotNull(route);
            Assert.Equal("{resource}.axd/{*pathInfo}", route.Url);
            Assert.Null(route.Defaults);
            Assert.Equal(0, route.Constraints.Count);

            route = RouteTable.Routes[1] as Route;
            Assert.NotNull(route);
            Assert.Equal("{*allaspx}", route.Url);
            Assert.Null(route.Defaults);
            Assert.Equal(1, route.Constraints.Count);
            Assert.Equal(@".*\.aspx(/.*)?", route.Constraints["allaspx"]);

            route = RouteTable.Routes[2] as Route;
            Assert.NotNull(route);
            Assert.Equal("{*favicon}", route.Url);
            Assert.Null(route.Defaults);
            Assert.Equal(1, route.Constraints.Count);
            Assert.Equal(@"(.*/)?favicon.ico(/.*)?", route.Constraints["favicon"]);
        }

        #region IDisposable Members

        public void Dispose()
        {
            RouteTable.Routes.Clear();
        }

        #endregion
    }
}
