using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IPool<T> : IDisposable
        where T : class
    {
        string Name { get; }

        T Take(object state);

        bool Return(T item);

        bool Clear();

        int Size { get; }

        int Count { get; }
    }
}
