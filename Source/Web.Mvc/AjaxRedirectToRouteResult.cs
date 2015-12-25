using System;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Web.Mvc.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public class AjaxRedirectToRouteResult : RedirectToRouteResult
    {
        public AjaxRedirectToRouteResult(RouteValueDictionary routeValues)
            : base(routeValues)
        {
        }

        public AjaxRedirectToRouteResult(string routeName, RouteValueDictionary routeValues)
            : base(routeName, routeValues)
        {
        }

        public AjaxRedirectToRouteResult(RedirectToRouteResult result)
            : base(result.RouteName, result.RouteValues)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var helper = new UrlHelper(context.RequestContext);
            var url = helper.RouteUrl(RouteName, RouteValues);
            if (String.IsNullOrEmpty(url))
            {
                base.ExecuteResult(context);
                return;
            }

            ControllerContextHelper.AjaxRedirect(context, url);
        }
    }
}
