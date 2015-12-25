using System;
using System.Web.Routing;
using ReusableLibrary.Web.Mvc.Routing;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Routing
{
    public sealed class RegisterDefaultRoutesTest : IDisposable
    {
        private readonly RegisterDefaultRoutes m_task = new RegisterDefaultRoutes();

        [Fact]
        [Trait(Constants.TraitNames.Routing, "RegisterDefaultRoutes")]
        public void Execute()
        {
            // Arrange            

            // Act
            m_task.Execute();

            // Assert
            Assert.Equal(1, RouteTable.Routes.Count);            
            var route = RouteTable.Routes["Default"] as Route;

            Assert.NotNull(route);
            Assert.Equal("{controller}/{action}/{id}", route.Url);
            Assert.Equal("Home", route.Defaults["controller"]);
            Assert.Equal("Index", route.Defaults["action"]);
            Assert.Equal(string.Empty, route.Defaults["id"]);
        }

        #region IDisposable Members

        public void Dispose()
        {
            RouteTable.Routes.Clear();
        }

        #endregion
    }
}
