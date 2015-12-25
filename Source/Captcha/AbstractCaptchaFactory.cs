using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Captcha.Content;
using ReusableLibrary.Captcha.Internals;
using ReusableLibrary.Captcha.Providers;

namespace ReusableLibrary.Captcha
{
    public abstract class AbstractCaptchaFactory : ICaptchaFactory
    {
        private readonly Func<ICache> m_challengeCacheFactory;
        private readonly ITuringNumberProvider m_turingNumberProvider;
        private readonly IVaryByCustomProvider m_varyByCustomProvider;
        private readonly IChallengeCodeProvider m_challengeCodeProvider;
        private readonly ICaptchaInstrumentationProvider m_instrumentationProvider;

        protected AbstractCaptchaFactory(CaptchaOptions options)
            : this(options, () => new InnerWebCache())
        {
        }

        protected AbstractCaptchaFactory(CaptchaOptions options, Func<ICache> challengeCacheFactory)
        {
            Options = options;
            m_challengeCacheFactory = challengeCacheFactory;
            m_turingNumberProvider = new DefaultTuringNumberProvider(options);
            m_varyByCustomProvider = new DefaultVaryByCustomProvider(options);
            m_challengeCodeProvider = new DefaultChallengeCodeProvider(options.ChallengeQueryKey, new[]
            {
                GetAntiForgeryTokenName(HttpRuntime.AppDomainAppVirtualPath),
                GetAntiForgeryTokenName(null),
                AuthCookieName()
            });
            m_instrumentationProvider = new CaptchaInstrumentationProvider(options);
        }

        #region ICaptchaFactory Members

        public CaptchaOptions Options { get; set; }

        public IChallengeCodeProvider ChallengeCodeProvider()
        {
            return m_challengeCodeProvider;
        }

        public IVaryByCustomProvider VaryByCustomProvider()
        {
            return m_varyByCustomProvider;
        }

        public ITuringNumberProvider TuringNumberProvider()
        {
            return m_turingNumberProvider;
        }

        public ICache ChallengeCache()
        {
            return m_challengeCacheFactory();
        }

        public ICaptchaInstrumentationProvider InstrumentationProvider()
        {
            return m_instrumentationProvider;
        }

        public abstract IContentProvider ContentProvider();

        public abstract IErrorProvider ErrorProvider();

        #endregion

        protected static string AuthCookieName()
        {
            var section = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
            return section.Forms.Name;
        }

        protected static string GetAntiForgeryTokenName(string appPath)
        {
            if (string.IsNullOrEmpty(appPath))
            {
                return "__RequestVerificationToken";
            }

            return "__RequestVerificationToken_" + Base64EncodeForCookieName(appPath);
        }

        private static string Base64EncodeForCookieName(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s))
                .Replace('+', '.').Replace('/', '-').Replace('=', '_');
        }
    }
}
