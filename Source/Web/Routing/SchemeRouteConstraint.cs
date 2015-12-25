using System;
using System.Web;
using System.Web.Routing;

namespace ReusableLibrary.Web.Routing
{
    public sealed class SchemeRouteConstraint : IRouteConstraint
    {
        public const string Name = "scheme";
        public const string Ignore = "__ignore__";

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, 
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            var scheme = values[Name] as string;
            return scheme != null
                && (routeDirection == RouteDirection.UrlGeneration
                    || Ignore.Equals(scheme, StringComparison.Ordinal)
                    || httpContext.Request.Scheme().Equals(scheme, StringComparison.Ordinal));
        }

        #endregion
    }
}
