using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Threading;

namespace ReusableLibrary.Abstractions.Caching
{
    public sealed class OnePassScope : Disposable, ILockScope
    {
        private readonly OnePass m_onePass;
        private readonly string m_key;
        private readonly bool m_aquired;

        public OnePassScope(OnePass onePass, string key)
        {
            m_onePass = onePass;
            m_key = key;
            m_aquired = m_onePass.TryEnter(key);
        }

        protected override void Dispose(bool disposing)
        {
            if (m_aquired)
            {
                m_onePass.TryLeave(m_key);
            }
        }

        #region ILockScope Members

        public bool Aquired
        {
            get { return m_aquired; }
        }

        #endregion
    }
}
