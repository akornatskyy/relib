using System;
using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class MessageHelper
    {
        public static MvcHtmlString ShowModelError(this HtmlHelper htmlHelper)
        {
            var message = htmlHelper.ViewData.ModelState.ModelErrorMessage();
            if (message == null)
            {
                return MvcHtmlString.Empty;
            }

            return ShowMessage(htmlHelper, message, "error-message");
        }

        public static MvcHtmlString NoRecordsFound(this HtmlHelper helper)
        {
            return ShowMessage(helper, GlobalResourceHelper.ErrorNoRecordsFound(), "warning-message");
        }

        public static MvcHtmlString ShowMessage(this HtmlHelper helper, string message)
        {
            return ShowMessage(helper, message, null);
        }

        public static MvcHtmlString ShowMessage(this HtmlHelper helper, string message, string cssClass)
        {
            if (String.IsNullOrEmpty(message))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder builder = new TagBuilder("span");
            builder.AddCssClass(cssClass ?? "info-message");
            builder.InnerHtml = helper.Encode(message);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
    }
}
