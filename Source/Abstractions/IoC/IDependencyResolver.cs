using System;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.IoC
{
    public interface IDependencyResolver : IDisposable
    {
        T Resolve<T>();

        T Resolve<T>(string name);

        IEnumerable<T> ResolveAll<T>();

        T Resolve<T>(Type type);
    }
}
