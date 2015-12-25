using System;
using ReusableLibrary.Abstractions.Caching;

namespace ReusableLibrary.Abstractions.Repository
{
    public sealed class CachingMementoRepository : DecoratedMementoRepository
    {
        public static readonly TimeSpan DefaultLifetime = TimeSpan.FromMinutes(15);
        private readonly ICache m_cache;

        public CachingMementoRepository(IMementoRepository innerRepository, ICache cache)
            : base(innerRepository)
        {
            m_cache = cache;
            Prefix = "Memento:";
            Lifetime = DefaultLifetime;
        }

        public string Prefix { get; set; }

        public TimeSpan Lifetime { get; set; }
        
        public override T Retrieve<T>(string id) 
        {
            var key = Prefix + id;
            var datakey = new DataKey<T>(key);
            if (!m_cache.Get(datakey))
            {
                return base.Retrieve<T>(id);
            }

            if (!datakey.HasValue)
            {
                return base.Retrieve<T>(id);
            }

            return datakey.Value;
        }

        public override bool Store<T>(string id, T value) 
        {
            if (!base.Store<T>(id, value))
            {
                return false;
            }

            var key = Prefix + id;
            return m_cache.Store<T>(key, value, Lifetime);
        }
    }
}
