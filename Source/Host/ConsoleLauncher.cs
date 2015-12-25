using System;
using ReusableLibrary.Abstractions.IoC;

namespace ReusableLibrary.Host
{
    public static class ConsoleLauncher
    {
        public static void Run()
        {
            var app = DependencyResolver.Resolve<IApplication>();            
            Console.WriteLine(app.Version);
            Console.WriteLine();
            
            app.Run();
        }
    }
}
