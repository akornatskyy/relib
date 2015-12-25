using ReusableLibrary.Abstractions.Tracing;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Captcha.Providers
{
    public sealed class CaptchaInstrumentationProvider : ICaptchaInstrumentationProvider
    {
        private readonly IPerformanceCounter m_issuedCounter;
        private readonly IPerformanceCounter m_failedCounter;
        private readonly IPerformanceCounter m_renderedCounter;
        private readonly IPerformanceCounter m_verificationCounter;
        private readonly IPerformanceCounter m_verificationFailedCounter;

        public CaptchaInstrumentationProvider(string category, string instanceNameSuffix, bool enabled)
        {
            var factory = new PerformanceCounterFactory(category, instanceNameSuffix, enabled);

            m_issuedCounter = factory.Create("Total Captcha Requests", "Captcha Requests/sec");
            m_failedCounter = factory.Create("Total Captcha Failed Requests", "Captcha Failed Requests/sec");
            m_renderedCounter = factory.Create("Total Captcha Render Requests", "Captcha Render Requests/sec");
            m_verificationCounter = factory.Create("Total Captcha Verification Requests", "Captcha Verification Requests/sec");
            m_verificationFailedCounter = factory.Create("Total Captcha Verification Failed Requests", "Captcha Verification Failed Requests/sec");
        }

        public CaptchaInstrumentationProvider(CaptchaOptions options)
            : this(options.Items[CaptchaOptionNames.InstrumentationCategory] ?? CaptchaOptionDefaults.InstrumentationCategory,
                   options.Items[CaptchaOptionNames.InstrumentationInstanceNameSuffix] ?? options.Path,
                   NameValueCollectionHelper.ConvertToBoolean(options.Items, CaptchaOptionNames.InstrumentationEnabled, CaptchaOptionDefaults.InstrumentationEnabled))
        {
        }

        #region ICaptchaInstrumentationProvider Members

        public void FireIssued(bool succeed)
        {
            if (succeed)
            {
                m_issuedCounter.Increment();
            }
            else
            {
                m_failedCounter.Increment();
            }
        }

        public void FireRendered(bool succeed)
        {
            if (succeed)
            {
                m_renderedCounter.Increment();
            }
            else
            {
                m_failedCounter.Increment();
            }
        }

        public void FireVerified(bool succeed)
        {
            if (succeed)
            {
                m_verificationCounter.Increment();
            }
            else
            {
                m_verificationFailedCounter.Increment();
            }
        }

        #endregion
    }
}
