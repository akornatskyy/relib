using System;
using System.Globalization;
using System.Web;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha
{
    public sealed class CaptchaHandler
    {
        private readonly HttpContextBase m_context;
        private readonly ICaptchaFactory m_factory;
        private readonly string m_path;
        private readonly TimeSpan m_varyByCacheLifetime;
        private readonly TimeSpan m_maxTimeout;

        public CaptchaHandler(HttpContextBase context)
            : this(context, CaptchaBuilder.Current.Factory(context.Request.Path))
        {
        }

        private CaptchaHandler(HttpContextBase context, ICaptchaFactory factory)
        {
            if (factory == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The path '{0}' has no setup in CaptchaBuilder", context.Request.Path));
            }

            m_context = context;
            m_factory = factory;
            var options = m_factory.Options;
            m_path = options.Path;
            m_maxTimeout = options.MaxTimeout;
            m_varyByCacheLifetime = options.VaryByCacheLifetime;            
        }

        public static string GetVaryByCustomString(HttpContext context, string custom)
        {
            var factory = CaptchaBuilder.Current.Factory(context.Request.Path);
            if (factory == null)
            {
                return null;
            }

            var handler = new CaptchaHandler(new HttpContextWrapper(context), factory);
            return handler.GetVaryByCustomString(custom);
        }

        public void RenderContent()
        {
            var ci = EnsureCaptchaInfo();
            var response = m_context.Response;
            m_factory.InstrumentationProvider().FireRendered(ci.Valid);
            if (!ci.Valid)
            {
                ApplyCachePolicy(TimeSpan.FromDays(1));
                var error = m_factory.ErrorProvider();
                response.ContentType = error.ContentType();
                error.WriteTo(response.OutputStream);
                return;
            }

            ApplyCachePolicy(m_varyByCacheLifetime);
            var content = m_factory.ContentProvider();
            response.ContentType = content.ContentType();
            content.WriteTo(response.OutputStream, ci.TuringNumber);
        }

        private string GetVaryByCustomString(string custom)
        {
            if (!m_path.Equals(custom, StringComparison.Ordinal))
            {
                return null;
            }

            var ci = EnsureCaptchaInfo();
            return ci.VaryByCustom;
        }

        private CaptchaInfo EnsureCaptchaInfo()
        {
            const string Key = "C";
            var ci = m_context.Items[Key] as CaptchaInfo;
            if (ci != null)
            {
                return ci;
            }

            var challengeCode = m_factory.ChallengeCodeProvider().ReadChallengeCode(m_context.Request.Params);
            if (challengeCode != null)
            {
                ci = new CaptchaInfo();
                ci.ChallengeCode = challengeCode;
                ci.VaryByCustom = m_factory.VaryByCustomProvider().NextVaryByCustomString();
                ci.TuringNumber = EnsureTuringNumber(ci.VaryByCustom);

                // Map Challenge with TuringNumber
                if (!m_factory.ChallengeCache().Store(new DataKey<Pair<DateTime, string>>(string.Concat(m_path, challengeCode))
                {
                    Value = new Pair<DateTime, string>(DateTime.UtcNow, ci.TuringNumber)
                }, m_maxTimeout))
                {
                    ci = CaptchaInfo.Invalid;
                }
            }
            else
            {
                ci = CaptchaInfo.Invalid;
            }

            m_context.Items.Add(Key, ci);
            m_factory.InstrumentationProvider().FireIssued(ci.Valid);
            return ci;
        }

        private string EnsureTuringNumber(string varyByCustom)
        {
            var dataKey = new DataKey<string>(string.Concat(m_path, varyByCustom));
            var cache = new InnerWebCache(HttpRuntime.Cache);
            cache.Get(dataKey);
            if (!dataKey.HasValue)
            {
                dataKey.Value = m_factory.TuringNumberProvider().NextTuringNumber();
                cache.Store(dataKey, m_varyByCacheLifetime);
            }

            return dataKey.Value;
        }

        private void ApplyCachePolicy(TimeSpan validFor)
        {
            var cache = m_context.Response.Cache;
            cache.SetVaryByCustom(m_path);
            cache.SetCacheability(HttpCacheability.Server);
            cache.SetExpires(m_context.Timestamp.Add(validFor));
            cache.SetValidUntilExpires(true);
            cache.VaryByParams.IgnoreParams = true;
        }

        #region private class CaptchaInfo

        private class CaptchaInfo
        {
            public static readonly CaptchaInfo Invalid = new CaptchaInfo()
            {
                VaryByCustom = "i"
            };

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

            public string VaryByCustom { get; set; }

            public string ChallengeCode { get; set; }

            public string TuringNumber { get; set; }
        }

        #endregion
    }
}
