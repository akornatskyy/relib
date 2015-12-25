using System;
using System.Diagnostics;
using System.Threading;

namespace ReusableLibrary.Abstractions.Models
{
    [DebuggerDisplay("Count = {Count}")]
    public sealed class StackPool<T> : Disposable, IPool<T>
        where T : class
    {
        private readonly T[] m_items;
        private int m_count;

        public StackPool(string name, int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            Name = name;
            m_items = new T[size];
        }

        #region IPool<T> Members

        public string Name { get; private set; }

        public T Take(object state)
        {
            if (m_count == 0)
            {
                return default(T);
            }

            Thread.BeginCriticalRegion();

            var item = m_items[--m_count];
            m_items[m_count] = default(T);
            
            Thread.EndCriticalRegion();
            return item;            
        }

        public bool Return(T item)
        {
            if (m_count >= m_items.Length)
            {
                return false;
            }

            Thread.BeginCriticalRegion();

            m_items[m_count++] = item;

            Thread.EndCriticalRegion();
            return true;            
        }

        public bool Clear()
        {
            Thread.BeginCriticalRegion();
            
            Array.Clear(m_items, 0, m_count);
            m_count = 0;
            
            Thread.EndCriticalRegion();
            return true;
        }

        public int Size
        {
            get { return m_items.Length; }
        }

        public int Count
        {
            get { return m_count; }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
        }
    }
}
