using System;
using System.Globalization;
using System.Web;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha
{
    public static class CaptchaHtmlHelper
    {
        private const string ImgFormat = "<img id=\"captcha\" src=\"{0}\" alt=\"{1}\" title=\"{1}\" />";
        private const string HiddenFormat = "<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />";

        public static string Render()
        {
            return Render(CaptchaOptionDefaults.Path, false);
        }

        public static string Render(bool forceChallengeCode)
        {
            return Render(CaptchaOptionDefaults.Path, forceChallengeCode);
        }

        public static string Render(string path)
        {
            return Render(path, false);
        }

        public static string Render(string path, bool forceChallengeCode)
        {
            var factory = CaptchaBuilder.Current.Factory(path);
            if (factory == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The path '{0}' has no setup in CaptchaBuilder", path));
            }

            var request = HttpContext.Current.Request;
            string hidden = null;
            if (forceChallengeCode || !factory.ChallengeCodeProvider()
                .HasChallengeCode(request.Params))
            {
                var key = factory.Options.ChallengeQueryKey;
                var challengeCode = GuidHelper.Shrink(Guid.NewGuid());
                hidden = string.Format(CultureInfo.InvariantCulture, HiddenFormat, key, challengeCode);
                path = string.Format(CultureInfo.InvariantCulture, "{0}?{1}={2}", path, key, challengeCode);
            }
            else if (IsAjaxRequest(request))
            {
                var random = new Random(RandomHelper.Seed());
                path = string.Format(CultureInfo.InvariantCulture, "{0}?r={1}", path, random.Next(1000));
            }

            var img = string.Format(CultureInfo.InvariantCulture, ImgFormat, path, ResourceHelper.ImgTitle());
            return string.Concat(img, hidden);
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return (request["X-Requested-With"] == "XMLHttpRequest")
                || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}
