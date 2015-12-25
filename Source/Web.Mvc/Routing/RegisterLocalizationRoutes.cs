using System;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Web.Mvc.Integration;
using ReusableLibrary.Web.Routing;

namespace ReusableLibrary.Web.Mvc.Routing
{
    public sealed class RegisterLocalizationRoutes : IStartupTask
    {
        #region IBootstrapperTask Members

        public void Execute()
        {
            var defaults = new RouteValueDictionary();
            defaults.Add("language", Localization.DefaultLanguage);
            defaults.Add("controller", "Home");
            defaults.Add("action", "Index");
            defaults.Add("id", String.Empty);

            var constraints = new RouteValueDictionary();
            constraints.Add("language", new ChoiceRouteConstraint(Localization.Languages));

            var route = new Route("{language}/{controller}/{action}/{id}", new MvcRouteHandler())
            {
                Defaults = defaults,
                Constraints = constraints
            };

            RouteTable.Routes.Add("Localization", route);
        }

        #endregion
    }
}
