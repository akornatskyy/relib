using System;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Abstractions.Bootstrapper;

namespace ReusableLibrary.Web.Mvc.Routing
{
    public sealed class RegisterDefaultRoutes : IStartupTask
    {
        #region IBootstrapperTask Members

        public void Execute()
        {
            var defaults = new RouteValueDictionary();
            defaults.Add("controller", "Home");
            defaults.Add("action", "Index");
            defaults.Add("id", String.Empty);
            RouteTable.Routes.MapRoute("Default", "{controller}/{action}/{id}", defaults);
        }

        #endregion
    }
}
