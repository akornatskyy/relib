using System.Web.Routing;

namespace ReusableLibrary.Web.Mvc.Routing
{
    public static class SubstitutionStateProvider
    {
        public static object RouteValues(RequestContext context)
        {
            return context.RouteData.Values;
        }
    }
}
