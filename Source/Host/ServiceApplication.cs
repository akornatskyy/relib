using System;
using System.Reflection;
using System.ServiceProcess;

namespace ReusableLibrary.Host
{
    public sealed class ServiceApplication : SingletonApplication
    {
        public ServiceApplication(string name)
            : base(name)
        {
        }

        public override void Run()
        {
            var args = Environment.GetCommandLineArgs();

            bool consoleMode = false;
            if (args.Length > 1)
            {
                var mode = args[1];
                if (!"--console".Equals(mode, StringComparison.OrdinalIgnoreCase)
                    && !"-c".Equals(mode, StringComparison.OrdinalIgnoreCase))
                {
                    ShowUsage();
                    return;
                }

                consoleMode = true;
            }

            if (consoleMode)
            {
                base.Run();                
            }
            else
            {
                ServiceBase.Run(new ServiceEntry(this));
                ShowUsage();
            }
        }

        protected override void RunCore()
        {
            base.RunCore();
            Console.WriteLine("\r\nPress any key to shutdown application . . .\r\n");
            Console.ReadKey();
        }

        private static void ShowUsage()
        {
            var name = Assembly.GetEntryAssembly().GetName().Name.ToLowerInvariant();
            Console.WriteLine("Usage: {0} [-c | --console]", name);
            Console.WriteLine();
            Console.WriteLine("Type '{0} --console' to run the program in console mode", name);
            Console.WriteLine("  or '{0}' to run as a windows service.", name);
        }
    }
}
