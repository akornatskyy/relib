using System;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Web.Mvc.Integration;

namespace ReusableLibrary.Web.Mvc.Tests.Integration
{
    internal sealed class MockControllerFactory : ControllerFactory
    {
        public RequestContext RequestContext { get; set; }

        public Controller GetControllerInstance2(Type controllerType)
        {
            return (Controller)GetControllerInstance(RequestContext, controllerType);
        }
    }
}
