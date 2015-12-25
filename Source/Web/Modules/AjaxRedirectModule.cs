using System;
using System.Web;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public sealed class AjaxRedirectModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PreSendRequestHeaders += new EventHandler(PreSendRequestHeaders);
        }        

        #endregion

        private static void PreSendRequestHeaders(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Error == null
                && context.Response.StatusCode == 302 
                && AjaxRedirectHelper.IsAjaxRequest(context.Request))
            {
                context.Response.StatusCode = AjaxRedirectHelper.AjaxRedirectStatusCode;
            }
        }
    }
}
