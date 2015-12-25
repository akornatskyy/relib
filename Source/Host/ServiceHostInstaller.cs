using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ReusableLibrary.Host
{
    [RunInstaller(true)]
    public class ServiceHostInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ServiceHostInstaller(string serviceName, string displayName)
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = serviceName;
            service.DisplayName = displayName;
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
