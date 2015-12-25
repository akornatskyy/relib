using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Supplemental.System;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public class LocalizationControllerFactory : ControllerFactory
    {
        public LocalizationControllerFactory()
        {
            var factories = ValueProviderFactories.Factories;
            var qsf = factories.Where(vpf => vpf.GetType() == typeof(System.Web.Mvc.QueryStringValueProviderFactory))
                .SingleOrDefault();
            if (qsf != null)
            {
                factories.Remove(qsf);
                factories.Add(new QueryStringValueProviderFactory2());
            }
        }

        public static bool TryLocalizeContext(RouteValueDictionary routeValues)
        {
            var language = routeValues["language"] as string;
            if (language == null || !language.In(Localization.Languages))
            {
                return false;
            }

            var culture = CultureInfo.GetCultureInfo(language);
            var thread = Thread.CurrentThread;
            thread.CurrentUICulture = culture;
            thread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture.Name);
            return true;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var routeData = requestContext.RouteData;
            if (routeData != null)
            {
                TryLocalizeContext(routeData.Values);
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }

        internal sealed class QueryStringValueProviderFactory2 : ValueProviderFactory
        {
            public override IValueProvider GetValueProvider(ControllerContext controllerContext)
            {
                if (controllerContext == null)
                {
                    throw new ArgumentNullException("controllerContext");
                }

                return new QueryStringValueProvider2(controllerContext);
            }
        }

        internal sealed class QueryStringValueProvider2 : NameValueCollectionValueProvider
        {
            public QueryStringValueProvider2(ControllerContext controllerContext)
                : base(controllerContext.HttpContext.Request.QueryString, CultureInfo.CurrentCulture)
            {
                /*
                 * Default implementation uses CultureInfo.InvariantCulture, this has been replaced
                 * by CultureInfo.CurrentCulture.
                 */
            }
        }
    }
}
