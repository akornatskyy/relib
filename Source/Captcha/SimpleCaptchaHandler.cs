using System;
using System.Web;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Captcha
{
    public sealed class SimpleCaptchaHandler
    {
        private readonly HttpContextBase m_context;
        private readonly ICaptchaFactory m_factory;
        private readonly string m_path;
        private readonly TimeSpan m_maxTimeout;

        public SimpleCaptchaHandler(HttpContextBase context)
        {
            m_factory = CaptchaBuilder.Current.Factory(context.Request.Path);

            m_context = context;
            var options = m_factory.Options;
            m_path = options.Path;
            m_maxTimeout = options.MaxTimeout;
        }

        public void RenderContent()
        {
            var ci = EnsureCaptchaInfo();
            var response = m_context.Response;
            m_factory.InstrumentationProvider().FireRendered(ci.Valid);
            if (!ci.Valid)
            {
                ApplyCachePolicy();
                var error = m_factory.ErrorProvider();
                response.ContentType = error.ContentType();
                error.WriteTo(response.OutputStream);
                return;
            }

            ApplyCachePolicy();
            var content = m_factory.ContentProvider();
            response.ContentType = content.ContentType();
            content.WriteTo(response.OutputStream, ci.TuringNumber);            
        }

        private CaptchaInfo EnsureCaptchaInfo()
        {
            var challengeCode = m_factory.ChallengeCodeProvider().ReadChallengeCode(m_context.Request.Params);
            if (challengeCode == null)
            {
                return CaptchaInfo.Invalid;
            }

            var ci = new CaptchaInfo()
            {
                ChallengeCode = challengeCode,
                TuringNumber = m_factory.TuringNumberProvider().NextTuringNumber()
            };

            // Map Challenge with TuringNumber
            if (!m_factory.ChallengeCache().Store(new DataKey<Pair<DateTime, string>>(string.Concat(m_path, challengeCode))
            {
                Value = new Pair<DateTime, string>(DateTime.UtcNow, ci.TuringNumber)
            }, m_maxTimeout))
            {
                ci = CaptchaInfo.Invalid;
            }

            return ci;
        }

        private void ApplyCachePolicy()
        {
            var cache = m_context.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.VaryByParams.IgnoreParams = true;
        }

        #region private class CaptchaInfo

        private class CaptchaInfo
        {
            public static readonly CaptchaInfo Invalid = new CaptchaInfo();

            public CaptchaInfo()
            {
            }

            public bool Valid
            {
                get
                {
                    return ChallengeCode != null;
                }
            }

            public string ChallengeCode { get; set; }

            public string TuringNumber { get; set; }
        }

        #endregion
    }
}
