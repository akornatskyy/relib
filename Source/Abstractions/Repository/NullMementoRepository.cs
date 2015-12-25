using System;

namespace ReusableLibrary.Abstractions.Repository
{
    public sealed class NullMementoRepository : IMementoRepository
    {
        #region IMementoRepository Members

        public T Retrieve<T>(string id) where T : new()
        {
            return new T();
        }

        public bool Store<T>(string id, T value) where T : new()
        {
            return true;
        }

        #endregion
    }
}
