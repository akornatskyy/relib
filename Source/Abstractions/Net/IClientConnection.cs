using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Net
{
    public interface IClientConnection : IIdleStateProvider
    {
        ConnectionOptions Options { get; set; }

        bool TryConnect();

        IBinaryReader Reader { get; }

        IBinaryWriter Writer { get; }

        void Close();
    }
}
