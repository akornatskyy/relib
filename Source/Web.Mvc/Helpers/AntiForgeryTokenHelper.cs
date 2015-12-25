using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc.Helpers
{
    public static class AntiForgeryTokenHelper
    {
        public const string Separator = ":";

        public static string GetAntiForgeryTokenName(string appPath)
        {
            if (string.IsNullOrEmpty(appPath))
            {
                return "__RequestVerificationToken";
            }

            return "__RequestVerificationToken_" + Base64EncodeForCookieName(appPath);
        }

        public static MvcHtmlString AntiForgeryTokenPatch(this HtmlHelper helper, string salt)
        {
            return AntiForgeryTokenPatch(helper, salt, null, null);
        }

        public static MvcHtmlString AntiForgeryTokenPatch(this HtmlHelper helper, string salt, string domain, string path)
        {
            var request = helper.ViewContext.HttpContext.Request;
            var name = GetAntiForgeryTokenName(HttpRuntime.AppDomainAppVirtualPath);
            var cookie = request.Cookies[name];
            if (cookie != null && string.IsNullOrEmpty(cookie.Value))
            {
                request.Cookies.Remove(name);
            }

            return helper.AntiForgeryToken(salt, domain, path);
        }

        public static MvcHtmlString AntiForgeryToken(this HtmlHelper helper, string[] salts)
        {
            return helper.AntiForgeryTokenPatch(string.Join(Separator, salts));
        }

        public static MvcHtmlString AntiForgeryToken(this HtmlHelper helper, string[] salts, string domain, string path)
        {
            return helper.AntiForgeryTokenPatch(string.Join(Separator, salts), domain, path);
        }

        private static string Base64EncodeForCookieName(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s))
                .Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }
    }
}
