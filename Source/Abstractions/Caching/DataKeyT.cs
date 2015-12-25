using System;

namespace ReusableLibrary.Abstractions.Caching
{
    public class DataKey<T> : DataKey
    {
        private T m_value;

        public DataKey(string key)
            : base(key)
        {
        }

        public virtual T Value
        {
            get
            {
                return m_value;
            }

            set
            {
                m_value = value;
                HasValue = true;
            }
        }

        public override void Load(ArraySegment<byte> segment, int flags)
        {
            Value = Formatter.Deserialize<T>(segment, flags);
        }

        public override ArraySegment<byte> Save(out int flags)
        {
            return Formatter.Serialize<T>(Value, out flags);
        }
    }
}
