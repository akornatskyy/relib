using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;

namespace ReusableLibrary.Web.Routing
{
    public sealed class ChoiceRouteConstraint : IRouteConstraint
    {
        private readonly string[] m_choices;

        public ChoiceRouteConstraint(string[] choices)
        {
            m_choices = choices;
            Array.Sort(m_choices, StringComparer.InvariantCultureIgnoreCase);
        }

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, 
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            string input = Convert.ToString(values[parameterName], CultureInfo.InvariantCulture);
            return Array.BinarySearch(m_choices, input, StringComparer.InvariantCultureIgnoreCase) >= 0;
        }

        #endregion
    }
}
