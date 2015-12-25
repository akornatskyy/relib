using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Caching;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Services;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Web.Integration
{
    public sealed class ErrorThrottleExceptionHandler : IExceptionHandler
    {
        public const string KeyPrefix = "ErrorThrottle-";

        private readonly IMailService m_mailService;

        public ErrorThrottleExceptionHandler(IMailService mailService)
        {
            m_mailService = mailService;

            // If there are more than {ErrorRate} errors
            // during {ThrottlePeriod} than block all incoming
            // requests with HttpForbidden for {BlockPeriod}.
            ErrorRate = 10;
            ThrottlePeriod = TimeSpan.FromHours(1);
            BlockPeriod = TimeSpan.FromHours(2);            
        }

        public string Application { get; set; }

        public int ErrorRate { get; set; }

        public TimeSpan ThrottlePeriod { get; set; }

        public TimeSpan BlockPeriod { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            var timestamp = DateTime.UtcNow;
            var context = HttpContext.Current;
            if (context == null)
            {
                return false;
            }

            var deny = false;
            var hosts = context.Request.UserHosts();
            foreach (var ip in hosts)
            {
                if (TrackError(context.Cache, ip, timestamp) > ErrorRate)
                {
                    DenialModule.Block(context, ip, timestamp.Add(BlockPeriod));
                    deny = true;
                }
            }

            if (deny)
            {
                SendMail(timestamp, hosts);
                DenialModule.Deny(context, timestamp);
                return true;
            }

            return false;
        }

        #endregion

        private int TrackError(Cache cache, string ip, DateTime timestamp)
        {
            var key = KeyPrefix + ip;
            var item = cache.Get(key) as List<DateTime>;
            if (item == null)
            {
                lock (cache)
                {
                    item = cache.Get(key) as List<DateTime>;
                    if (item == null)
                    {
                        item = new List<DateTime>(ErrorRate);
                        item.Add(DateTime.UtcNow);
                        cache.Insert(key, item, null, Cache.NoAbsoluteExpiration, ThrottlePeriod, CacheItemPriority.BelowNormal, null);
                        return 1;
                    }
                }
            }

            lock (item)
            {
                if (item.Count >= ErrorRate)
                {
                    item.RemoveAll(t => (timestamp - t) > ThrottlePeriod);
                }

                item.Add(timestamp);
                return item.Count;
            }            
        }

        private bool SendMail(DateTime timestamp, string[] hosts)
        {            
            var mail = new MailMessage()
            {
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8,
                Subject = String.Format(CultureInfo.InvariantCulture, Properties.Resources.ErrorThrottleMailSubject,
                    Application, String.Join(", ", hosts)),
                BodyEncoding = Encoding.UTF8,
                Body = MergeErrorReportMessage(timestamp, hosts)
            };
            m_mailService.Send(mail);
            return false;
        }

        private string MergeErrorReportMessage(DateTime timestamp, string[] hosts)
        {
            return m_mailService.MergeTemplate(Properties.Resources.DeniedMailBody,
                "Application", Application,
                /* 2010/9/15 7:53:30 PM UTC */
                "DateSubmitted", timestamp.ToString("yyyy/M/d h:mm:ss tt UTC", CultureInfo.InvariantCulture),
                "HostEnvironment", Environment.MachineName,
                "ErrorRate", ErrorRate.ToString(CultureInfo.InvariantCulture),
                "ThrottlePeriod", TimeSpanHelper.ToLongTimeString(ThrottlePeriod).ToLowerInvariant(),
                "Hosts", String.Join(", ", hosts),
                "BlockPeriod", TimeSpanHelper.ToLongTimeString(BlockPeriod).ToLowerInvariant(),
                "BlockExpiresAt", timestamp.Add(BlockPeriod).ToString("yyyy/M/d h:mm tt UTC", CultureInfo.InvariantCulture));
        }
    }
}
