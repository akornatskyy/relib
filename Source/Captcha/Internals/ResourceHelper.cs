using System;
using System.Globalization;
using System.Web;

namespace ReusableLibrary.Captcha.Internals
{
    internal static class ResourceHelper
    {
        public static string ImgTitle()
        {
            return GetString("HtmlHelperImgTitle");
        }

        public static string ValidationChallengeInvalid()
        {
            return GetString("ValidationChallengeInvalid");
        }

        public static string ValidationCodeExpired()
        {
            return GetString("ValidationCodeExpired");
        }

        public static string ValidationTooQuickly()
        {
            return GetString("ValidationTooQuickly");
        }

        public static string ValidationNoMatch()
        {
            return GetString("ValidationNoMatch");
        }

        private static string GetString(string resourceKey)
        {
            string resourceValue = null;
            if (!String.IsNullOrEmpty(CaptchaOptions.ResourceClassKey))
            {
                resourceValue = HttpContext.GetGlobalResourceObject(CaptchaOptions.ResourceClassKey, resourceKey, CultureInfo.CurrentUICulture) as string;
            }

            return resourceValue ?? Properties.Resources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture);
        }
    }
}
