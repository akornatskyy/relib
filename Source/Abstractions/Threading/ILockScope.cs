using System;

namespace ReusableLibrary.Abstractions.Threading
{
    public interface ILockScope : IDisposable
    {
        bool Aquired { get; }
    }
}
