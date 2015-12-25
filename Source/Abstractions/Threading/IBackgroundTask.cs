using System;
using System.Threading;

namespace ReusableLibrary.Abstractions.Threading
{
    public interface IBackgroundTask
    {
        EventWaitHandle WaitHandle { get; }

        bool IsRunning
        {
            get;
        }

        void Start();

        bool Wait(TimeSpan timeout);

        void Stop(bool forceShutdown);
    }
}
