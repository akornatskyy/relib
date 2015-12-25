using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class LinkExtentions
    {
        public static MvcHtmlString AbsoluteRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName)
        {
            return AbsoluteRouteLink(htmlHelper, linkText, routeName, null, null);
        }

        public static MvcHtmlString AbsoluteRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues)
        {
            return AbsoluteRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), null);
        }

        public static MvcHtmlString AbsoluteRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, RouteValueDictionary routeValues)
        {
            return AbsoluteRouteLink(htmlHelper, linkText, routeName, routeValues, null);
        }

        public static MvcHtmlString AbsoluteRouteLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        {
            return AbsoluteRouteLink(htmlHelper, linkText, routeName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString AbsoluteRouteLink(this HtmlHelper htmlHelper,
            string linkText, string routeName, RouteValueDictionary routeValues, 
            IDictionary<string, object> htmlAttributes)
        {            
            return UrlHelperExtensions.AbsoluteRoute<MvcHtmlString>(
                htmlHelper.RouteCollection, 
                new HttpContextWrapper(HttpContext.Current), 
                routeName, routeValues,
                (r, s, d) => htmlHelper.RouteLink(linkText, routeName, s, d, null, r, htmlAttributes));
        }
    }
}
