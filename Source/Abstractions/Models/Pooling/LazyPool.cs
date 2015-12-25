using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class LazyPool<T> : ManagedPool<T>
        where T : class
    {
        private readonly Func2<object, T> m_createfactory;

        public LazyPool(IPool<T> inner, Func2<object, T> createfactory, Action<T> releasefactory)
            : base(inner, releasefactory)
        {
            m_createfactory = createfactory;
        }

        public override T Take(object state)
        {
            var item = Inner.Take(state);
            if (item != default(T))
            {
                return item;
            }

            return m_createfactory(state);
        }
    }
}
