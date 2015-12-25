using System;
using System.Diagnostics;
using System.Web;
using ReusableLibrary.Abstractions.IoC;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Web
{
    public sealed class ExceptionPolicyModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.Error += new EventHandler(OnError);
        }

        #endregion

        private static void OnError(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var ex = context.Server.GetLastError();
            var exceptionPolicy = DependencyResolver.Resolve<IExceptionHandler>();
            Trace.Assert(exceptionPolicy.HandleException(ex));
        }
    }
}
