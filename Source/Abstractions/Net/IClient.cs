using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Net
{
    public interface IClient : IDisposable, IIdleStateProvider, IEquatable<IClient>
    {
        Func2<Action2<IClientConnection, object>, bool> Context(object state);
    }
}
