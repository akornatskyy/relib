using System;
using System.Diagnostics;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Host
{
    public sealed class OpenServiceHostTask : IStartupTask
    {
        private static readonly TraceInfo g_traceInfo = new TraceInfo(new TraceSource(Properties.Resources.TraceSourceBootstrapper));

        private readonly IServiceHost m_host;

        public OpenServiceHostTask(IServiceHost host)
        {
            m_host = host;
        }

        #region IBootstrapperTask Members

        public void Execute()
        {
            try
            {
                if (g_traceInfo.IsInfoEnabled)
                {
                    TraceHelper.TraceInfo(g_traceInfo, "Opening ServiceHost...");
                    foreach (var address in m_host.BaseAddresses)
                    {
                        TraceHelper.TraceInfo(g_traceInfo, "Base Address: {0}", address);
                    }
                }

                m_host.Open();
            }
            catch (SystemException sex) 
            {
                var handler = DependencyResolver.Resolve<IExceptionHandler>();
                handler.HandleException(sex);
            }
        }

        #endregion
    }
}
