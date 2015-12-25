using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Web.Routing;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName)
        {
            return AbsoluteRouteUrl(helper, routeName, null);
        }

        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName, object routeValues)
        {
            return AbsoluteRouteUrl(helper, routeName, new RouteValueDictionary(routeValues));
        }

        public static string AbsoluteRouteUrl(this UrlHelper helper, string routeName, RouteValueDictionary routeValues)
        {
            return AbsoluteRouteUrl(helper, new HttpContextWrapper(HttpContext.Current), routeName, routeValues);
        }

        public static string AbsoluteRouteUrl(UrlHelper helper, HttpContextBase context, string routeName, RouteValueDictionary routeValues)
        {
            return UrlHelperExtensions.AbsoluteRoute<string>(
                helper.RouteCollection, context, routeName, routeValues,
                (r, s, d) => helper.RouteUrl(routeName, r, s, d));
        }

        public static T AbsoluteRoute<T>(RouteCollection routes, HttpContextBase context,
            string routeName, RouteValueDictionary routeValues,
            Func<RouteValueDictionary, string, string, T> strategy)
        {
            var route = routes[routeName] as Route;
            if (route == null)
            {
                throw new ArgumentException("Undefined route name", routeName);
            }

            var defaults = new RouteValueDictionary();

            var defaultDomain = route.Defaults[DomainRouteConstraint.Name] as string;
            if (defaultDomain != null)
            {
                defaults.Add(DomainRouteConstraint.Name, defaultDomain);
            }

            var domain = Domain(context, defaultDomain);

            var defaultScheme = route.Defaults[SchemeRouteConstraint.Name] as string;
            if (defaultScheme != null)
            {
                defaults.Add(SchemeRouteConstraint.Name, defaultScheme);
            }

            var scheme = Scheme(context, defaultScheme);
            if (!string.IsNullOrEmpty(scheme) && string.IsNullOrEmpty(domain))
            {
                domain = context.Request.Host();
            }

            // If domain has been changed but scheme not, we need to
            // use route default scheme and if not present use request scheme
            // due to UrlHelper.RouteUrl uses http scheme if not set and doen't
            // take current into account
            if (!string.IsNullOrEmpty(domain) && string.IsNullOrEmpty(scheme))
            {
                scheme = context.Request.Scheme();
            }

            if (routeValues != null)
            {
                defaults.Merge(routeValues);
            }

            return strategy(defaults, scheme, domain);
        }

        public static string Domain(HttpContextBase context, string defaultDomain)
        {
            if (defaultDomain != null && !DomainRouteConstraint.Ignore.Equals(defaultDomain))
            {
                var host = context.Request.Host();
                if (!host.Equals(defaultDomain, StringComparison.Ordinal))
                {
                    return defaultDomain;
                }
            }

            return null;
        }

        public static string Scheme(HttpContextBase context, string defaultScheme)
        {
            if (defaultScheme != null && !SchemeRouteConstraint.Ignore.Equals(defaultScheme))
            {
                var requestScheme = context.Request.Scheme();
                if (!requestScheme.Equals(defaultScheme, StringComparison.Ordinal))
                {
                    return defaultScheme;                    
                }
            }

            return null;
        }
    }
}
