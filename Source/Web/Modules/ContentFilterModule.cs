using System;
using System.Web;
using ReusableLibrary.Web.Integration;

namespace ReusableLibrary.Web
{
    public abstract class ContentFilterModule : IHttpModule
    {
        private readonly string m_name;

        protected ContentFilterModule(string name)
        {
            m_name = name;
        }

        public abstract void InstallFilter(HttpApplication app);

        #region IHttpModule Members

        public virtual void Dispose()
        {
        }

        public virtual void Init(HttpApplication app)
        {
            app.ReleaseRequestState += new EventHandler(TryInstallFilterEventHandler);
            app.PreSendRequestHeaders += new EventHandler(TryInstallFilterEventHandler); 
        }

        #endregion

        protected virtual void TryInstallFilter(HttpApplication app)
        {
            var context = app.Context;
            if (context.Error != null
                || context.CurrentHandler == null
                || context.CurrentHandler is DefaultHttpHandler
                || context.Handler is IHttpFilterIgnore
                || context.Items.Contains(m_name))
            {
                return;
            }

            context.Items.Add(m_name, true);
            InstallFilter(app);
        }

        private void TryInstallFilterEventHandler(object sender, EventArgs e)
        {
            TryInstallFilter((HttpApplication)sender);
        }
    }
}
