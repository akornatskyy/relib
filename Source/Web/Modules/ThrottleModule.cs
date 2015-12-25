using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Web.Integration;

namespace ReusableLibrary.Web
{
    public sealed class ThrottleModule : IHttpModule
    {
        private const string KeyPrefix = "ThrottleModule-";

        static ThrottleModule()
        {
            ThrottlePeriod = TimeSpan.FromMinutes(1);
            RateQuota = 120;
            NoExceptionPathRegex = null;
        }

        public static TimeSpan ThrottlePeriod { get; set; }

        public static long RateQuota { get; set; }

        public static Regex NoExceptionPathRegex { get; set; }

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PostMapRequestHandler += new EventHandler(OnEnter);
        }

        #endregion

        private void OnEnter(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Error != null
                || context.CurrentHandler == null
                || context.CurrentHandler is DefaultHttpHandler)
            {
                return;
            }

            var cache = new WebCache(context.Cache);
            var request = context.Request;
            foreach (var ip in request.UserHosts())
            {
                var key = KeyPrefix + ip;
                if (cache.Increment(key, ThrottlePeriod) >= RateQuota)
                {
                    if (NoExceptionPathRegex == null
                        || !NoExceptionPathRegex.IsMatch(request.RawUrl))
                    {
                        throw new LimitExceededException(string.Format(CultureInfo.InvariantCulture,
                                "The maximum quota for incoming requests ({0} queries per {1}) has been exceeded.",
                                RateQuota,
                                TimeSpanHelper.ToLongTimeString(ThrottlePeriod).ToLowerInvariant()));
                    }
                }
            }
        }
    }
}
