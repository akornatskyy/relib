using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class LazyObject<T> : ILazy<T>
    {
        private readonly Func2<T> m_loader;
        private T m_object;

        public LazyObject(Func2<T> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException("loader");
            }

            m_loader = loader;
        }

        #region ILazy<T> Members

        public bool Loaded
        {
            get { return m_object != null; }
        }

        public void Reset()
        {
            m_object = default(T);
        }

        public T Object 
        {
            get 
            {
                if (m_object == null)
                {
                    m_object = m_loader();
                }

                return m_object;
            }
        }

        #endregion
    }
}
