using System;

namespace ReusableLibrary.Abstractions.Net
{
    public interface IRemoteLocationProvider
    {
        RemoteLocation RemoteLocation { get; }
    }
}
