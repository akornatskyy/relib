using System.Diagnostics;
using System.Security.Permissions;
using ReusableLibrary.Abstractions.Models;
using WatiN.Core;

namespace ReusableLibrary.WatiN
{
    public class ApplicationLifeTimeContainer<TBrowser> : Disposable
        where TBrowser : Browser
    {
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public ApplicationLifeTimeContainer(string pathToWebSite, int webServerPort, bool manageProcess)
        {
            Application = Application.Attach<TBrowser>(pathToWebSite, webServerPort, manageProcess);
        }

        public Application Application { get; set; }

        #region DisposableModel Members

        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (disposing && Application != null)
            {
                Application.Dispose();
                Application = null;
            }
        }

        #endregion
    }
}
