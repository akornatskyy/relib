using System;
using System.Timers;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.HistoryLog.Models;
using ReusableLibrary.Supplemental.System;
using ReusableLibrary.Unity;
using Xunit;

namespace ReusableLibrary.HistoryLog.Tests
{
    public sealed class Program
    {
        public static void Main()
        {
            UnityBootstrapLoader.Initialize(UnityBootstrapLoader.LoadConfigFilesFromAppSettings());
            BootstrapLoader.Start();

            var queue = DependencyResolver.Resolve<HistoryLogQueue>("Default-HistoryLog");

            var timer = new Timer(TimeSpan.FromSeconds(1.5).TotalMilliseconds);
            timer.Elapsed += (s, e) =>
            {
                queue.Enqueue(DomainModelFactory.RandomHistoryLogItem());
            };
            timer.Start();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            timer.Stop();
            BootstrapLoader.End();
            Assert.True(queue.IsEmpty());
        }
    }
}
