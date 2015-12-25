using System;
using System.Web;

namespace ReusableLibrary.Web.Helpers
{
    public static class CookieHelper
    {
        public static void ResetCookies(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var request = context.Request;
            foreach (var cookieName in request.Cookies.AllKeys)
            {
                context.Response.Cookies.Add(new HttpCookie(cookieName)
                {
                    Expires = DateTime.UtcNow.AddYears(-1)
                });
            }

            request.Cookies.Clear();
        }
    }
}
