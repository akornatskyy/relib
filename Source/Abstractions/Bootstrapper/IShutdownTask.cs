using System;

namespace ReusableLibrary.Abstractions.Bootstrapper
{
    public interface IShutdownTask
    {
        void Execute();
    }
}
