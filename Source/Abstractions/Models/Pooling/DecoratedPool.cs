using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Models
{
    [DebuggerDisplay("Count = {Count}")]
    public abstract class DecoratedPool<T> : Disposable, IPool<T>
        where T : class
    {
        protected DecoratedPool(IPool<T> inner)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }

            if (String.IsNullOrEmpty(inner.Name))
            {
                throw new ArgumentNullException("inner", "Name must be non empty string");
            }

            Inner = inner;
        }

        #region IPool<T> Members

        public string Name 
        {
            get { return Inner.Name; }
        }

        public virtual T Take(object state)
        {
            return Inner.Take(state);
        }

        public virtual bool Return(T item)
        {
            return Inner.Return(item);
        }

        public virtual bool Clear()
        {
            return Inner.Clear();
        }

        public virtual int Size
        {
            get { return Inner.Size; }
        }

        public virtual int Count
        {
            get { return Inner.Count; }
        }

        #endregion

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Inner.Dispose();
            }
        }

        #endregion

        protected IPool<T> Inner { get; private set; }
    }
}
