using System;
using System.Text.RegularExpressions;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Integration
{
    public sealed class ThrottleModuleSettings : IStartupTask
    {
        public long RateQuota { get; set; }

        public TimeSpan ThrottlePeriod { get; set; }

        public string NoExceptionPathRegex { get; set; }

        #region IStartupTask Members

        public void Execute()
        {
            ThrottleModule.RateQuota = RateQuota;
            ThrottleModule.ThrottlePeriod = ThrottlePeriod;

            if (!string.IsNullOrEmpty(NoExceptionPathRegex))
            {
                ThrottleModule.NoExceptionPathRegex = new Regex(NoExceptionPathRegex, 
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }

        #endregion
    }
}
