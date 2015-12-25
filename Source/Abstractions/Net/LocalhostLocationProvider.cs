using System;

namespace ReusableLibrary.Abstractions.Net
{
    public sealed class LocalhostLocationProvider : IRemoteLocationProvider
    {
        #region IRemoteLocationProvider Members

        public RemoteLocation RemoteLocation
        {
            get { return RemoteLocation.Localhost; }
        }

        #endregion
    }
}
