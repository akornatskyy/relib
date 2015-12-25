using System;
using ReusableLibrary.Abstractions.Serialization.Formatters;

namespace ReusableLibrary.Abstractions.Serialization
{
    public interface IObjectState
    {
        void Initialize(IObjectFormatter formatter);

        bool HasValue { get; }

        void Load(ArraySegment<byte> segment, int flags);

        ArraySegment<byte> Save(out int flags);
    }
}
