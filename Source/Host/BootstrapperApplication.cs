using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Host
{
    public abstract class BootstrapperApplication : Disposable, IApplication
    {
        protected BootstrapperApplication(string name)
        {
            Name = name;
            TraceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceBootstrapper));
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "HostThread";
            }
        }

        #region IApplication Members

        public string Name { get; private set; }

        public string Version
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} v{1}",
                    Name, AssemblyHelper.ToLongVersionString(Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()));
            }
        }

        public virtual bool CanRun()
        {
            return true;
        }

        public virtual void Run()
        {
            try
            {
                if (!CanRun())
                {
                    return;
                }

                OnStart();
                RunCore();
            }
            catch (Exception ex)
            {
                var handler = DependencyResolver.Resolve<IExceptionHandler>();
                handler.HandleException(ex);
            }
            finally
            {
                try
                {
                    OnStop();
                }
                catch (Exception ex)
                {
                    var handler = DependencyResolver.Resolve<IExceptionHandler>();
                    handler.HandleException(ex);
                }
            }
        }

        public virtual void OnStart()
        {
            BootstrapLoader.Start();
        }

        public virtual void OnStop()
        {
            BootstrapLoader.End();
        }

        #endregion

        protected TraceInfo TraceInfo { get; private set; }

        protected virtual void RunCore()
        {
            // Yields the rest of its current slice of processor time giving 
            // background threads to start work
            Thread.Sleep(1);
            if (TraceInfo.IsInfoEnabled)
            {
                TraceHelper.TraceInfo(TraceInfo, "Ready");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var handler = DependencyResolver.Resolve<IExceptionHandler>();
            handler.HandleException(e.ExceptionObject as Exception);
        }
    }
}
