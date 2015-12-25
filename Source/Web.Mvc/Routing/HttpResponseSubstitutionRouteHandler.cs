using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Web.Mvc.Integration;

namespace ReusableLibrary.Web.Mvc.Routing
{
    public sealed class HttpResponseSubstitutionRouteHandler : IRouteHandler
    {
        public Func2<RequestContext, object> StateProvider { get; set; }

        public ParameterizedHttpResponseSubstitutionCallback StartCallback { get; set; }

        public ParameterizedHttpResponseSubstitutionCallback EndCallback { get; set; }

        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.IsHttpVerbGetOrHead())
            {
                var state = StateProvider != null ? StateProvider(requestContext) : null;
                return new HttpResponseSubstitutionHandler(requestContext) 
                { 
                    State = state,
                    StartCallback = StartCallback,
                    EndCallback = EndCallback
                };
            }
            else
            {
                return new MvcHandler(requestContext);
            }
        }

        #endregion
    }
}
