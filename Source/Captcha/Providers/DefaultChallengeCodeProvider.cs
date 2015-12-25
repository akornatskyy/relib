using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Captcha.Providers
{
    public sealed class DefaultChallengeCodeProvider : IChallengeCodeProvider
    {
        private readonly string m_queryKey;
        private readonly string[] m_extraKeys;
        private readonly Func2<NameValueCollection, string> m_strategy;

        public DefaultChallengeCodeProvider(string queryKey)
            : this(queryKey, null)
        {
        }

        public DefaultChallengeCodeProvider(string queryKey, string[] extraKeys)
        {
            m_queryKey = queryKey;
            if (extraKeys == null || extraKeys.Length == 0)
            {
                m_strategy = ReadByQueryKey;
            }
            else
            {
                m_extraKeys = extraKeys;
                m_strategy = ReadByExtraKeys;
            }
        }

        #region IChallengeCodeProvider Members

        public bool HasChallengeCode(NameValueCollection @params)
        {
            return !string.IsNullOrEmpty(TryReadExtraKeys(@params));
        }

        public string ReadChallengeCode(NameValueCollection @params)
        {
            return m_strategy(@params);
        }

        #endregion

        private string ReadByQueryKey(NameValueCollection @params)
        {
            return @params[m_queryKey];
        }

        private string ReadByExtraKeys(NameValueCollection @params)
        {
            var val = TryReadExtraKeys(@params);
            if (string.IsNullOrEmpty(val))
            {
                return ReadByQueryKey(@params);
            }

            if (val.Length > 20)
            {
                using (var hashAlgorithm = HashAlgorithm.Create("SHA1"))
                {
                    val = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(val)));
                }
            }

            return val;
        }

        private string TryReadExtraKeys(NameValueCollection @params)
        {
            string val = null;
            foreach (var extraKey in m_extraKeys)
            {
                val = @params[extraKey];
                if (!string.IsNullOrEmpty(val))
                {
                    break;
                }
            }

            return val;
        }
    }
}
