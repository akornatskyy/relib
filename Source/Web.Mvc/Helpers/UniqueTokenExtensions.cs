using System.Web.Mvc;
using System.Web.Mvc.Html;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class UniqueTokenExtensions
    {
        public static MvcHtmlString UniqueToken(this HtmlHelper helper)
        {
            return UniqueToken(helper, null);
        }

        public static MvcHtmlString UniqueToken(this HtmlHelper helper, string value)
        {
            return helper.Hidden(UniqueTokenHelper.UniqueTokenName, value ?? UniqueTokenHelper.UniqueToken());
        }
    }
}
