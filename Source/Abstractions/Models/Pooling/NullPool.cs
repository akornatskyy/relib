using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class NullPool<T> : IPool<T>
        where T : class
    {
        public NullPool(string name)
        {
            Name = name;
        }

        #region IPool<T> Members

        public string Name { get; set; }

        public T Take(object state)
        {
            return default(T);
        }

        public bool Return(T item)
        {
            return false;
        }

        public bool Clear()
        {
            return true;
        }

        public int Size
        {
            get { return 0; }
        }

        public int Count
        {
            get { return 0; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
