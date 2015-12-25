using System.Web.Mvc;
using System.Web.Routing;

namespace ReusableLibrary.Web.Mvc
{
    public static class RouteCollectionExtensions
    {
        public static Route MapRoute(this RouteCollection routes, string name, string match, RouteValueDictionary defaults)
        {
            var route = new Route(match, new MvcRouteHandler())
            {
                Defaults = defaults
            };
            routes.Add(name, route);
            return route;
        }

        public static Route IgnoreRoute(this RouteCollection routes, string name, string match, RouteValueDictionary constraints)
        {
            var route = new Route(match, new StopRoutingHandler())
            {
                Constraints = constraints
            };
            routes.Add(name, route);
            return route;
        }
    }
}
