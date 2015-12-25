using System;
using System.Web;
using System.Web.Routing;

namespace ReusableLibrary.Web.Routing
{
    public sealed class DomainRouteConstraint : IRouteConstraint
    {
        public const string Name = "domain";
        public const string Ignore = "__ignore__";

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, 
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            var domain = values[Name] as string;
            return domain != null
                && (routeDirection == RouteDirection.UrlGeneration 
                    || Ignore.Equals(domain, StringComparison.Ordinal)
                    || domain.Equals(httpContext.Request.Host(), StringComparison.Ordinal));
        }

        #endregion
    }
}
