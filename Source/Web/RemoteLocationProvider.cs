using System.Web;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public sealed class RemoteLocationProvider : IRemoteLocationProvider
    {
        public RemoteLocationProvider()
        {
            RemoteLocation = new RemoteLocation()
            {
                Hosts = UserHostsHelper.UserHosts(HttpContext.Current.Request.ServerVariables)
            };
        }

        #region IRemoteLocationProvider Members

        public RemoteLocation RemoteLocation
        {
            get;
            private set;
        }

        #endregion
    }
}
