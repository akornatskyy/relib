using System;
using System.Threading;
using ReusableLibrary.Abstractions.Repository;

namespace ReusableLibrary.Abstractions.Services
{
    public sealed class MementoService : IMementoService
    {
        private readonly IMementoRepository m_repository;

        public MementoService(IMementoRepository repository)
        {
            m_repository = repository;
            Enabled = true;
        }

        public bool Enabled { get; set; }

        #region IMementoService Members

        public bool Save<T>(T value) 
            where T : new()
        {
            if (!Enabled)
            {
                return true;
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return m_repository.Store<T>(GetKey(typeof(T)), value);
        }

        public T Load<T>()
            where T : new()
        {
            if (!Enabled)
            {
                return new T();
            }

            return m_repository.Retrieve<T>(GetKey(typeof(T)));
        }

        #endregion

        private static string GetKey(Type type)
        {
            var identity = Thread.CurrentPrincipal.Identity;
            if (identity.IsAuthenticated)
            {
                return string.Concat(identity.Name.ToUpperInvariant(), ":", type.FullName);
            }

            return type.FullName;
        }
    }
}
