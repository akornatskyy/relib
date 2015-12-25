using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class Buffer<T>
    {
        private readonly double m_growFactor;
        private T[] m_data;

        public Buffer(int capacity)
            : this(capacity, 1.25)
        {
        }

        public Buffer(int capacity, double growFactor)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }

            if (growFactor <= 1.0)
            {
                throw new ArgumentOutOfRangeException("growFactor");
            }

            m_growFactor = growFactor;
            m_data = new T[capacity];
        }

        public T[] Array
        {
            get { return m_data; }
        }

        public int Capacity
        {
            get { return m_data.Length; }
        }

        public void EnsureCapacity(int count)
        {
            if (count <= m_data.Length)
            {
                return;
            }

            var length = (int)(m_data.Length * m_growFactor);
            if (count < length)
            {
                count = length;
            }

            System.Array.Resize<T>(ref m_data, count);
        }
    }
}
