using System;

namespace ReusableLibrary.Host
{
    public interface IApplication
    {
        string Name { get; }

        string Version { get; }

        bool CanRun();

        void OnStart();

        void Run();

        void OnStop();
    }
}
