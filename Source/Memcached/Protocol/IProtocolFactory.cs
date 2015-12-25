using System;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;
using ReusableLibrary.Abstractions.Serialization.Formatters;

namespace ReusableLibrary.Memcached.Protocol
{
    public interface IProtocolFactory : IDisposable
    {
        Func2<Action2<IClientConnection, object>, bool> Context(object state);

        Pooled<IProtocol> AquireProtocol();

        IEncoder CreateEncoder();

        IPacketBuilder CreatePacketBuilder(Buffer<byte> buffer);

        IPacketParser CreatePacketParser(IBinaryReader reader, Buffer<byte> buffer);

        IObjectFormatter CreateObjectFormatter();

        ICommandWriter CreateCommandWriter(IPacketBuilder builder);

        ICommandReader CreateCommandReader(IPacketParser parser);
    }
}
