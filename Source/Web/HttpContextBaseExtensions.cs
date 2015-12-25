using System.Web;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public static class HttpContextBaseExtensions
    {
        public static void ResetCookies(this HttpContextBase context)
        {
            CookieHelper.ResetCookies(context);
        }
    }
}
