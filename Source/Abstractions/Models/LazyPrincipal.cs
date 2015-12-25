using System;
using System.Security.Principal;
using System.Threading;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class LazyPrincipal<T> : ILazy<T>
    {
        private readonly Func2<IPrincipal, T> m_loader;
        private T m_object;
        private IPrincipal m_principal;

        public LazyPrincipal(Func2<IPrincipal, T> loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException("loader");
            }

            m_loader = loader;
            m_principal = null;
        }

        #region ILazy<T> Members

        public bool Loaded
        {
            get { return m_object != null && Thread.CurrentPrincipal == m_principal; }
        }

        public void Reset()
        {
            m_object = default(T);
            m_principal = null;
        }

        public T Object 
        {
            get 
            {
                var principal = Thread.CurrentPrincipal;
                if (m_object == null || principal != m_principal)
                {
                    m_object = m_loader(principal);
                    m_principal = principal;
                }

                return m_object;
            }
        }

        #endregion
    }
}
