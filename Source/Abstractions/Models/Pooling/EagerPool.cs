using System;

namespace ReusableLibrary.Abstractions.Models
{
    public sealed class EagerPool<T> : ManagedPool<T>
        where T : class
    {
        public EagerPool(IPool<T> inner, Func2<T> createfactory, Action<T> releasefactory)
            : base(inner, releasefactory)
        {
            for (int i = 0; i < inner.Size; i++)
            {
                var succeed = Inner.Return(createfactory());
                if (!succeed)
                {
                    throw new InvalidOperationException("Unable to load items");
                }
            }
        }
    }
}
