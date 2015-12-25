using System;
using System.Collections.Generic;
using System.Text;

namespace ReusableLibrary.Abstractions.Repository
{
    public abstract class DecoratedMementoRepository : IMementoRepository
    {
        private readonly IMementoRepository m_innerRepository;

        protected DecoratedMementoRepository(IMementoRepository innerRepository)
        {
            m_innerRepository = innerRepository;
        }

        #region IMementoRepository Members

        public virtual T Retrieve<T>(string id) where T : new()
        {
            return m_innerRepository.Retrieve<T>(id);
        }

        public virtual bool Store<T>(string id, T value) where T : new()
        {
            return m_innerRepository.Store<T>(id, value);
        }

        #endregion
    }
}
