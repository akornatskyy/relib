using System;
using System.Web.Mvc;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class ControllerContextHelper
    {
        public const int AjaxRedirectStatusCode = 207;

        public static void AjaxRedirect(ControllerContext context, string url)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.IsChildAction)
            {
                throw new InvalidOperationException(Properties.Resources.RedirectAction_CannotRedirectInChildAction);
            }

            context.Controller.TempData.Keep();            
            AjaxRedirectHelper.AjaxRedirect(context.HttpContext, url, false);
        }
    }
}
