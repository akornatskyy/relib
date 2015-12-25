using System;
using System.Web;

namespace ReusableLibrary.Web
{
    public sealed class NoServerHeaderModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += new EventHandler(OnPreSendRequestHeaders);
        }

        #endregion

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            context.Response.Headers.Remove("Server");
        }
    }
}
