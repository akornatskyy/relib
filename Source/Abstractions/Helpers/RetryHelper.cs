using System;
using System.Globalization;
using System.Threading;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Helpers
{
    public static class RetryHelper
    {
        public static int Execute(RetryOptions options, Func2<bool> func)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var succeed = false;
            var started = DateTime.UtcNow;
            int attempt = 0;
            double remaining = 0.0;
            do
            {
                succeed = func();
                attempt++;
                if (succeed)
                {
                    break;
                }

                remaining = options.RetryTimeout - (DateTime.UtcNow - started).TotalMilliseconds;
                if (remaining <= 0.0)
                {
                    break;
                }

                Thread.Sleep(options.RetryDelay);
            }
            while (attempt <= options.MaxRetryCount);

            if (succeed)
            {
                return 0;
            }

            if (options.RetryFails)
            {
                var msg = (attempt > 1)
                    ? String.Format(CultureInfo.InvariantCulture,
                        Properties.Resources.RetryHelperTimeout, 
                        attempt - 1,
                        TimeSpanHelper.ToShortTimeString(TimeSpan.FromMilliseconds(options.RetryTimeout)))
                    : Properties.Resources.RetryHelperTimeoutNoRetry;

                throw new TimeoutException(msg);
            }

            return attempt;
        }
    }
}
