using System;
using System.Web.Mvc;
using System.Web.Routing;
using IoC = ReusableLibrary.Abstractions.IoC;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }

            var viewDataAccessor = (ViewDataAccessor)IoC::DependencyResolver.Resolve<IViewDataAccessor>();
            var controller = IoC::DependencyResolver.Resolve<Controller>(controllerType);
            controller.TempDataProvider = new EmptyTempDataProvider();
            viewDataAccessor.Setup(controller);
            return controller;
        }
    }
}
