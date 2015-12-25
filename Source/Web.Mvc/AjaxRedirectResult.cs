using System;
using System.Web.Mvc;
using ReusableLibrary.Web.Mvc.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public class AjaxRedirectResult : RedirectResult
    {
        public AjaxRedirectResult(string url)
            : base(url)
        {
        }

        public AjaxRedirectResult(RedirectResult result)
            : base(result.Url)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            string url = UrlHelper.GenerateContentUrl(Url, context.HttpContext);
            ControllerContextHelper.AjaxRedirect(context, url);
        }
    }
}
