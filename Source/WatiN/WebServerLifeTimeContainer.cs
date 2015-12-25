using System.Diagnostics;
using System.Security.Permissions;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.WatiN
{
    public class WebServerLifeTimeContainer : Disposable
    {
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public WebServerLifeTimeContainer(string pathToWebSite, int webServerPort, bool manageProcess)
        {
            WebServer = WebServer.Attach(pathToWebSite, webServerPort, manageProcess);
        }

        public WebServer WebServer { get; set; }

        #region DisposableModel Members

        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (disposing && WebServer != null)
            {
                WebServer.Dispose();
                WebServer = null;
            }
        }

        #endregion
    }
}
