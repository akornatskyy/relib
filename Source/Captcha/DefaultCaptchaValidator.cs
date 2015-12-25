using System;
using System.Globalization;
using System.Web;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Captcha.Internals;

namespace ReusableLibrary.Captcha
{
    public sealed class DefaultCaptchaValidator : ICaptchaValidator
    {
        private readonly IValidationState m_state;

        public DefaultCaptchaValidator(IValidationState state)
        {
            m_state = state;
            Enabled = true;
        }

        #region ICaptchaValidator Members

        public bool Enabled { get; set; }

        public bool Validate()
        {
            return Validate(CaptchaOptionDefaults.Path);
        }

        public bool Validate(string path)
        {
            if (!Enabled)
            {
                return true;
            }

            var factory = CaptchaBuilder.Current.Factory(path);
            if (factory == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The path '{0}' has no setup in CaptchaBuilder", path));
            }

            var succeed = DoValidate(factory, path);
            factory.InstrumentationProvider().FireVerified(succeed);
            return succeed;
        }

        #endregion

        private bool DoValidate(ICaptchaFactory factory, string path)
        {
            var options = factory.Options;
            var request = HttpContext.Current.Request;
            var challengeCode = factory.ChallengeCodeProvider().ReadChallengeCode(request.Params);
            if (challengeCode == null)
            {
                m_state.AddError(options.ValidationStateKey, ResourceHelper.ValidationChallengeInvalid());
                return false;
            }

            var cache = factory.ChallengeCache();
            var datakey = new DataKey<Pair<DateTime, string>>(string.Concat(path, challengeCode));
            cache.Get(datakey);
            if (!datakey.HasValue)
            {
                m_state.AddError(options.ValidationStateKey,
                    string.Format(CultureInfo.InvariantCulture,
                        ResourceHelper.ValidationCodeExpired(), options.MaxTimeout.TotalSeconds));
                return false;
            }

            cache.Remove(datakey.Key);
            if (datakey.Value.First.Add(options.MinTimeout) > DateTime.UtcNow)
            {
                m_state.AddError(options.ValidationStateKey,
                    string.Format(CultureInfo.InvariantCulture, ResourceHelper.ValidationTooQuickly(), options.MinTimeout.TotalSeconds));
                return false;
            }

            var entry = request.Form[options.ValidationStateKey];
            if (!datakey.Value.Second.Equals(entry, StringComparison.OrdinalIgnoreCase))
            {
                m_state.AddError(options.ValidationStateKey, ResourceHelper.ValidationNoMatch());
                return false;
            }

            return true;
        }
    }
}
