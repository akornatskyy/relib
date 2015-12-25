using System;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public interface IObjectFormatter
    {
        ArraySegment<byte> Serialize<T>(T value, out int flags);

        T Deserialize<T>(ArraySegment<byte> data, int flags);
    }
}
