using System.Threading;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Threading;

namespace ReusableLibrary.Abstractions.Tests.Threading
{
    internal class MockAbstractBackgroundTask : AbstractBackgroundTask
    {
        public int RunCount { get; set; }

        public bool WorkDone { get; set; }

        public bool HasMoreWork { get; set; }

        public string ThreadName { get; private set; }

        public override bool DoWork(Func2<bool> shutdownCallback)
        {
            Thread.Sleep(25);
            ThreadName = Thread.CurrentThread.Name;
            WorkDone = true;
            RunCount++;
            return HasMoreWork;
        }
    }
}
