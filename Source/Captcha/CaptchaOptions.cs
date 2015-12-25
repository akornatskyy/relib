using System;
using System.Collections.Specialized;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Captcha
{
    [Serializable]
    public class CaptchaOptions
    {
        public static string ResourceClassKey { get; set; }

        public CaptchaOptions()
            : this(string.Empty)
        {
        }

        public CaptchaOptions(string options)
        {
            var items = StringHelper.ParseOptions(options);

            Path = items[CaptchaOptionNames.Path] ?? CaptchaOptionDefaults.Path;

            MinTimeout = TimeSpan.FromSeconds(NameValueCollectionHelper.ConvertToInt32(items,
                CaptchaOptionNames.MinTimeout, CaptchaOptionDefaults.MinTimeout));
            MaxTimeout = TimeSpan.FromSeconds(NameValueCollectionHelper.ConvertToInt32(items,
                CaptchaOptionNames.MaxTimeout, CaptchaOptionDefaults.MaxTimeout));

            VaryByCacheLifetime = TimeSpan.FromSeconds(NameValueCollectionHelper.ConvertToInt32(items, 
                CaptchaOptionNames.VaryByCacheLifetime, CaptchaOptionDefaults.VaryByCacheLifetime));

            ChallengeQueryKey = items[CaptchaOptionNames.ChallengeQueryKey] 
                ?? CaptchaOptionDefaults.ChallengeQueryKey;
            ValidationStateKey = items[CaptchaOptionNames.ValidationStateKey] 
                ?? CaptchaOptionDefaults.ValidationStateKey;

            Width = NameValueCollectionHelper.ConvertToInt32(items,
                CaptchaOptionNames.Width, CaptchaOptionDefaults.Width);
            Height = NameValueCollectionHelper.ConvertToInt32(items,
                CaptchaOptionNames.Height, CaptchaOptionDefaults.Height);

            Items = items;
        }

        public NameValueCollection Items { get; private set; }

        public string Path { get; set; }

        public string ValidationStateKey { get; set; }

        public TimeSpan MinTimeout { get; set; }

        public TimeSpan MaxTimeout { get; set; }

        public TimeSpan VaryByCacheLifetime { get; set; }

        public string ChallengeQueryKey { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
