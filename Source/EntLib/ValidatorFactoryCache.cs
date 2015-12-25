using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace ReusableLibrary.EntLib
{
    public sealed class ValidatorFactoryCache : IValidatorFactory
    {
        private static readonly IValidatorFactory g_instance = new ValidatorFactoryCache();

        private readonly IDictionary<Key, Validator> m_validatorCache = new Dictionary<Key, Validator>();

        public static IValidatorFactory Instance
        {
            get { return g_instance; }
        }

        private ValidatorFactoryCache()
        {
        }

        #region IValidatorFactory Members

        public Validator ProvideValidator(Type type, string ruleset)
        {
            Validator validator = null;
            var key = new Key(type, ruleset);
            if (!m_validatorCache.TryGetValue(key, out validator))
            {
                lock (m_validatorCache)
                {
                    if (!m_validatorCache.TryGetValue(key, out validator))
                    {
                        ValidationFactory.ResetCaches();
                        m_validatorCache[key] = validator = ValidationFactory.CreateValidator(type, ruleset);
                    }
                }
            }

            return validator;
        }

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        private struct Key : IEquatable<Key>
        {
            private int m_culture;
            private Type m_type;
            private string m_ruleset;            

            public Key(Type type, string ruleset)
            {
                m_culture = Thread.CurrentThread.CurrentCulture.LCID;
                m_type = type;
                m_ruleset = ruleset;                
            }

            #region IEquatable<Key> Members

            public bool Equals(Key other)
            {
                return this.m_culture == other.m_culture
                    && this.m_type == other.m_type
                    && String.Equals(this.m_ruleset, other.m_ruleset, StringComparison.Ordinal);
            }

            #endregion
        }
    }
}
