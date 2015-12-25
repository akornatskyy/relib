using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;

namespace ReusableLibrary.Memcached.Protocol
{
    public class BinaryProtocolFactory : AbstractProtocolFactory
    {
        public BinaryProtocolFactory(IClientFactory clientFactory, ProtocolOptions options)
            : base("BinaryProtocol", clientFactory, options)
        {
        }

        #region IProtocolFactory Members

        public override IEncoder CreateEncoder()
        {
            return new TextEncoder(Encoding.UTF8);
        }

        public override IPacketBuilder CreatePacketBuilder(Buffer<byte> buffer)
        {
            return new BinaryPacketBuilder(buffer);
        }

        public override IPacketParser CreatePacketParser(IBinaryReader reader, Buffer<byte> buffer)
        {
            return new BinaryPacketParser(reader, buffer);
        }

        public override ICommandWriter CreateCommandWriter(IPacketBuilder builder)
        {
            return new BinaryCommandWriter(builder);
        }

        public override ICommandReader CreateCommandReader(IPacketParser parser)
        {
            return new BinaryCommandReader(parser);
        }

        #endregion
    }
}
