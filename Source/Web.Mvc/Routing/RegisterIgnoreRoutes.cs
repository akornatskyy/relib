using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Mvc.Routing
{
    public sealed class RegisterIgnoreRoutes : IStartupTask
    {
        #region IBootstrapperTask Members

        public void Execute()
        {
            var routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var constraints = new RouteValueDictionary();
            constraints.Add("allaspx", @".*\.aspx(/.*)?");
            routes.IgnoreRoute("allaspx", "{*allaspx}", constraints);

            constraints = new RouteValueDictionary();
            constraints.Add("favicon", @"(.*/)?favicon.ico(/.*)?");
            routes.IgnoreRoute("favicon", "{*favicon}", constraints);
        }

        #endregion
    }
}
