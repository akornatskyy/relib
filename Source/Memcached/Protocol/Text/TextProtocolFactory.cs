using System.Text;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Net;

namespace ReusableLibrary.Memcached.Protocol
{
    public class TextProtocolFactory : AbstractProtocolFactory
    {
        public TextProtocolFactory(IClientFactory clientFactory, ProtocolOptions options)
            : base("TextProtocol", clientFactory, options)
        {
        }

        #region IProtocolFactory Members

        public override IEncoder CreateEncoder()
        {
            return new Base64Encoder(new TextEncoder(Encoding.UTF8));
        }

        public override IPacketBuilder CreatePacketBuilder(Buffer<byte> buffer)
        {
            return new TextPacketBuilder(buffer);
        }

        public override IPacketParser CreatePacketParser(IBinaryReader reader, Buffer<byte> buffer)
        {
            return new TextPacketParser(reader, buffer);
        }

        public override ICommandWriter CreateCommandWriter(IPacketBuilder builder)
        {
            return new TextCommandWriter(builder);
        }

        public override ICommandReader CreateCommandReader(IPacketParser parser)
        {
            return new TextCommandReader(parser);
        }

        #endregion
    }
}
