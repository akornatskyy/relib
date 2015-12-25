using System;
using System.Linq;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Captcha.Providers
{
    public sealed class DefaultTuringNumberProvider : ITuringNumberProvider
    {
        private readonly string m_characterGroup;
        private readonly int m_min;
        private readonly int m_max;

        public DefaultTuringNumberProvider()
            : this(StringHelper.Numeric, 4)
        {
        }

        public DefaultTuringNumberProvider(string characterGroup, int max)
            : this(characterGroup, max, max)
        {
        }

        public DefaultTuringNumberProvider(string characterGroup, int min, int max)
        {
            m_characterGroup = characterGroup;
            m_min = min;
            m_max = max;
        }

        public DefaultTuringNumberProvider(CaptchaOptions options)
        {
            var items = options.Items;
            m_characterGroup = items[CaptchaOptionNames.Chars] ?? CaptchaOptionDefaults.Chars;
            m_max = NameValueCollectionHelper.ConvertToInt32(items, CaptchaOptionNames.MaxChars, CaptchaOptionDefaults.MaxChars);
            m_min = NameValueCollectionHelper.ConvertToInt32(items, CaptchaOptionNames.MinChars, m_max);
        }

        #region ITuringNumberProvider Members

        public string NextTuringNumber()
        {
            var random = new Random(RandomHelper.Seed());
            return new string(RandomHelper.NextSequence(random, m_min, m_max,
                i => RandomHelper.NextChar(random, m_characterGroup)).ToArray());
        }

        #endregion
    }
}
