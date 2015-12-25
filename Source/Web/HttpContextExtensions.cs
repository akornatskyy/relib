using System.Web;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public static class HttpContextExtensions
    {
        public static void ResetCookies(this HttpContext context)
        {
            CookieHelper.ResetCookies(new HttpContextWrapper(context));
        }
    }
}
