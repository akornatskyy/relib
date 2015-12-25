using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class MenuItemHelper
    {
        public static MvcHtmlString MenuItem(this HtmlHelper helper, string linkText, string actionName)
        {
            return MenuItem(helper, linkText, actionName, null, null);
        }

        public static MvcHtmlString MenuItem(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return MenuItem(helper, linkText, actionName, controllerName, null);
        }

        public static MvcHtmlString MenuItem(this HtmlHelper helper, string linkText, string actionName, string controllerName, string className)
        {
            TagBuilder builder = new TagBuilder("li");
            if (IsCurrentAction(helper, actionName, controllerName))
            {
                builder.MergeAttribute("class", className ?? "active");
            }

            builder.InnerHtml = helper.ActionLink(linkText, actionName, controllerName).ToHtmlString();
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static bool IsCurrentAction(this HtmlHelper helper, string actionName)
        {
            return IsCurrentAction(helper, actionName, null);
        }

        public static bool IsCurrentAction(this HtmlHelper helper, string actionName, string controllerName)
        {
            var routeValues = helper.ViewContext.RouteData.Values;
            var currentControllerName = (string)routeValues["controller"];
            if (currentControllerName == null)
            {
                return false;
            }

            var currentActionName = (string)routeValues["action"];
            if (currentActionName == null)
            {
                return false;
            }

            return currentActionName.Equals(actionName, StringComparison.OrdinalIgnoreCase)
                && (controllerName == null || currentControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase));                
        }
    }
}
