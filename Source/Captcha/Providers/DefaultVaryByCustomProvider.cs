using System.Globalization;
using System.Threading;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Captcha.Providers
{
    public sealed class DefaultVaryByCustomProvider : IVaryByCustomProvider
    {
        private int m_varyByIndex;
        private int m_maxVaryCacheSize;        

        public DefaultVaryByCustomProvider(int maxVaryCacheSize)
        {
            m_maxVaryCacheSize = maxVaryCacheSize;
        }

        public DefaultVaryByCustomProvider(CaptchaOptions options)
            : this(NameValueCollectionHelper.ConvertToInt32(options.Items, 
                CaptchaOptionNames.MaxVaryCacheSize, 
                CaptchaOptionDefaults.MaxVaryCacheSize))
        {
        }

        #region IVaryByCustomProvider Members

        public string NextVaryByCustomString()
        {
            // Loop index from 0 to MaxVaryCacheSize
            Interlocked.CompareExchange(ref m_varyByIndex, 0, m_maxVaryCacheSize);
            var index = Interlocked.Increment(ref m_varyByIndex);
            return index.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
