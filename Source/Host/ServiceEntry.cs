using System.ServiceProcess;

namespace ReusableLibrary.Host
{
    public sealed class ServiceEntry : ServiceBase
    {
        private readonly ServiceApplication m_app;

        public ServiceEntry(ServiceApplication app)
        {
            m_app = app;
        }

        protected override void OnStart(string[] args)
        {
            m_app.OnStart();
        }

        protected override void OnStop()
        {
            m_app.OnStop();
        }
    }
}
