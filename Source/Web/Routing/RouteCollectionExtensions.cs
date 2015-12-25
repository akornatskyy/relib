using System;
using System.Web.Routing;

namespace ReusableLibrary.Web.Routing
{
    public static class RouteCollectionExtensions
    {
        public static Route MapRoute(this RouteCollection routes, string name, string url, IRouteHandler handler)
        {
            return MapRoute(routes, name, url, null, null, null, handler);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, object defaults, IRouteHandler handler)
        {
            return MapRoute(routes, name, url, defaults, null, null, handler);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, IRouteHandler handler)
        {
            return MapRoute(routes, name, url, defaults, constraints, null, handler);
        }

        public static Route MapRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces, IRouteHandler handler)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var route = new Route(url, handler)
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };
            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }
            routes.Add(name, route);
            return route;
        }
    }
}
