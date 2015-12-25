using System;
using System.Web.Mvc;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public sealed class RegisterControllerFactory : IStartupTask
    {
        private readonly IControllerFactory m_controllerFactory;

        public RegisterControllerFactory(IControllerFactory controllerFactory)
        {
            m_controllerFactory = controllerFactory;
        }

        public void Execute()
        {
            ControllerBuilder.Current.SetControllerFactory(m_controllerFactory);
        }
    }
}
