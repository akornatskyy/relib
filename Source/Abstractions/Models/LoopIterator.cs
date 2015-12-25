using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class LoopIterator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly IEnumerable<T> m_source;
        private readonly int m_max;
        private IEnumerator<T> m_enumerator;        
        private int m_iteration;

        public LoopIterator(IEnumerable<T> source, int max)
        {
            m_source = source;
            m_max = max;
            m_iteration = 0;
            m_enumerator = source.GetEnumerator();
        }

        #region IEnumerator<T> Members

        public T Current
        {
            get { return m_enumerator.Current; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            m_enumerator.Dispose();
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return (m_enumerator as System.Collections.IEnumerator).Current; }
        }

        public bool MoveNext()
        {
            if (m_iteration++ == m_max)
            {
                return false;
            }

            var hasMore = m_enumerator.MoveNext();
            if (!hasMore)
            {
                m_enumerator = m_source.GetEnumerator();
                hasMore = m_enumerator.MoveNext();
            }

            return hasMore;
        }

        public void Reset()
        {
            m_iteration = 0;
            m_enumerator.Reset();
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion
    }
}
