using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public class LazyDataKey<T> : DataKey<T>
    {
        private int m_flags;
        private ArraySegment<byte> m_segment;

        public LazyDataKey(string key)
            : base(key)
        {
        }

        public override T Value
        {
            get
            {
                var bytes = m_segment;
                if (HasValue && bytes.Array != null)
                {
                    base.Value = Formatter.Deserialize<T>(m_segment, m_flags);
                    m_segment = default(ArraySegment<byte>);
                }

                return base.Value;
            }
        }

        public override void Load(ArraySegment<byte> segment, int flags)
        {
            m_flags = flags;
            m_segment = segment;
            HasValue = true;
        }
    }
}
