using System;

namespace ReusableLibrary.Abstractions.Net
{
    public interface IClientFactory : IDisposable
    {
        IClient Client();
    }
}
