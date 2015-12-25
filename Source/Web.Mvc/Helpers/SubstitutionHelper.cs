using System.Web.Mvc;
using ReusableLibrary.Web.Mvc.Integration;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class SubstitutionHelper
    {
        public static MvcHtmlString RenderSubstitution(this HtmlHelper helper, string name)
        {
            return RenderSubstitution(helper, name, null);
        }

        public static MvcHtmlString RenderSubstitution(this HtmlHelper helper, string name, object state)
        {
            var context = helper.ViewContext.HttpContext;
            return MvcHtmlString.Create(
                context.Handler is HttpResponseSubstitutionHandler
                ? HttpResponseSubstitutionHandler.Token(name)
                : HttpResponseSubstitutionHandler.Execute(context, name, state));
        }
    }
}
