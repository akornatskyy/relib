using System;
using System.Collections.Generic;

namespace ReusableLibrary.Captcha
{
    public sealed class CaptchaBuilder
    {
        private static readonly CaptchaBuilder g_buidler = new CaptchaBuilder();

        private IDictionary<string, Func<ICaptchaFactory>> m_factories = new Dictionary<string, Func<ICaptchaFactory>>();

        public static CaptchaBuilder Current
        {
            get
            {
                return g_buidler;
            }
        }

        public void Setup(ICaptchaFactory factory)
        {
            m_factories[factory.Options.Path] = () => factory;
        }

        public void Setup<T>(CaptchaOptions options)
            where T : ICaptchaFactory, new()
        {
            m_factories[options.Path] = () => new T() { Options = options };
        }

        public void Setup(CaptchaOptions options, Type type)
        {
            if (!typeof(ICaptchaValidator).IsAssignableFrom(type))
            {
                throw new ArgumentException("The type parameter must be assignable from ICaptchaValidator interface", "type");
            }

            m_factories[options.Path] = () =>
            {
                var factory = (ICaptchaFactory)Activator.CreateInstance(type);
                factory.Options = options;
                return factory;
            };
        }

        public ICaptchaFactory Factory(string path)
        {
            Func<ICaptchaFactory> factory;
            if (!m_factories.TryGetValue(path, out factory))
            {
                return null;
            }

            return factory();
        }
    }
}
