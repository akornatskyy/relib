using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Bootstrapper
{
    public static class BootstrapLoader
    {
        public static void Start()
        {
            foreach (var task in DependencyResolver.ResolveAll<IStartupTask>())
            {
                task.Execute();
            }
        }

        public static void End()
        {
            foreach (var task in DependencyResolver.ResolveAll<IShutdownTask>())
            {
                task.Execute();
            }

            DependencyResolver.Reset();
        }
    }
}
