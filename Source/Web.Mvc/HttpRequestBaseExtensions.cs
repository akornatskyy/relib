using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Supplemental.System;
using ReusableLibrary.Web.Mvc.Integration;

namespace ReusableLibrary.Web.Mvc
{
    public static class HttpRequestBaseExtensions
    {
        public static ActionResult CheckAspxErrorPath(this HttpRequestBase request, RouteValueDictionary routeValues)
        {
            return CheckParamPath(request, routeValues, "aspxerrorpath");
        }

        public static ActionResult CheckReturnUrl(this HttpRequestBase request, RouteValueDictionary routeValues)
        {
            return CheckParamPath(request, routeValues, "returnurl");
        }

        public static ActionResult CheckUrlReferrer(this HttpRequestBase request, RouteValueDictionary routeValues)
        {
            string path = null;
            if (request.UrlReferrer != null)
            {
                path = request.UrlReferrer.AbsolutePath;
            }

            return CheckPath(routeValues, "referrer", path);
        }

        public static ActionResult CheckParamPath(this HttpRequestBase request, RouteValueDictionary routeValues, string urlParamName)
        {
            return CheckPath(routeValues, urlParamName, request.Params[urlParamName]);
        }

        private static ActionResult CheckPath(RouteValueDictionary routeValues, string urlParamName, string path)
        {            
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }

            var currentCulture = Thread.CurrentThread.CurrentCulture;
            if (path.StartsWith("/" + currentCulture.TwoLetterISOLanguageName + "/",
                StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (path.StartsWith("/" + currentCulture.Name + "/", 
                StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var nextSlash = path.IndexOf('/', 1) - 1;
            var language = nextSlash > 0 ? path.Substring(1, nextSlash).ToLowerInvariant() : null;
            if (language != null && language.In(Localization.Languages))
            {
                routeValues["language"] = language;
                routeValues[urlParamName] = path;
            }
            else
            {
                routeValues["language"] = Localization.DefaultLanguage;
            }

            return new RedirectToRouteResult(routeValues);
        }
    }
}
