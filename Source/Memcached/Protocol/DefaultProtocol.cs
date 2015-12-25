using System;
using System.Globalization;
using ReusableLibrary.Abstractions.Caching;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Serialization.Formatters;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class DefaultProtocol : IProtocol
    {
        private readonly Buffer<byte> m_buffer;
        private readonly IEncoder m_keyEncoder;
        private readonly IPacketBuilder m_builder;
        private readonly IObjectFormatter m_formatter;
        private readonly ICommandWriter m_commandWriter;
        private readonly IProtocolFactory m_factory;

        public DefaultProtocol(IProtocolFactory factory,
            ProtocolOptions options)
        {
            m_buffer = new Buffer<byte>(options.BufferSize);
            m_factory = factory;
            Options = options;

            m_keyEncoder = factory.CreateEncoder();
            m_builder = factory.CreatePacketBuilder(m_buffer);
            m_formatter = factory.CreateObjectFormatter();
            m_commandWriter = factory.CreateCommandWriter(m_builder);
        }

        #region IProtocol Members

        public ProtocolOptions Options { get; private set; }

        public bool Store<T>(StoreOperation operation, T datakey, int expires)
            where T : DataKey
        {
            datakey.Initialize(m_formatter);
            int flags;
            var bytes = datakey.Save(out flags);
            var key = m_keyEncoder.GetBytes(datakey.Key);
            m_commandWriter.Store(new StorePacket()
            {
                Operation = operation,
                Key = key,
                Flags = flags,
                Expires = expires,
                Value = bytes,
                Version = datakey.Version
            }, Options.NoReply);

            var succeed = true;
            return m_factory.Context(key)((connection, state) =>
            {
                m_builder.WriteTo(connection.Writer);
                if (!Options.NoReply)
                {
                    var commandReader = m_factory.CreateCommandReader(
                        m_factory.CreatePacketParser(connection.Reader, m_buffer));
                    succeed = commandReader.ReadStored();
                }
            }) && succeed;
        }

        public bool Get<T>(GetOperation operation, T datakey) where T : DataKey
        {
            var includeVersion = operation == GetOperation.Gets;
            var key = m_keyEncoder.GetBytes(datakey.Key);
            m_commandWriter.Get(operation, key);

            return m_factory.Context(key)((connection, state) =>
            {
                m_builder.WriteTo(connection.Writer);

                var commandReader = m_factory.CreateCommandReader(
                        m_factory.CreatePacketParser(connection.Reader, m_buffer));
                var response = commandReader.ReadValue(includeVersion);
                if (response != null)
                {
                    datakey.Initialize(m_formatter);
                    datakey.Version = response.Version;
                    datakey.Load(response.Value, response.Flags);
                    commandReader.ReadSucceed();
                }
            });
        }

        public bool GetMany<T>(GetOperation operation, T[] datakeys)
            where T : DataKey
        {
            var includeVersion = operation == GetOperation.Gets;
            var allkeys = new byte[datakeys.Length][];
            for (int i = 0; i < datakeys.Length; i++)
            {
                allkeys[i] = m_keyEncoder.GetBytes(datakeys[i].Key);
            }

            return m_factory.Context(allkeys)((connection, state) =>
            {
                m_commandWriter.GetMany(operation, (byte[][])state);
                m_builder.WriteTo(connection.Writer);

                var commandReader = m_factory.CreateCommandReader(
                        m_factory.CreatePacketParser(connection.Reader, m_buffer));
                ValuePacket response;
                while ((response = commandReader.ReadValue(includeVersion)) != null)
                {
                    var datakey = datakeys[Array.BinarySearch<byte[]>(allkeys, response.Key, ByteArrayComparer.Default)];
                    datakey.Initialize(m_formatter);
                    datakey.Version = response.Version;
                    datakey.Load(response.Value, response.Flags);
                }
            });
        }

        public bool Delete(string key)
        {
            var keybytes = m_keyEncoder.GetBytes(key);
            m_commandWriter.Delete(keybytes, Options.NoReply);

            var succeed = true;
            return m_factory.Context(keybytes)((connection, state) =>
            {
                m_builder.WriteTo(connection.Writer);
                if (!Options.NoReply)
                {
                    var commandReader = m_factory.CreateCommandReader(
                        m_factory.CreatePacketParser(connection.Reader, m_buffer));
                    succeed = commandReader.ReadDeleted();
                }
            }) && succeed;
        }

        public bool Increment(DataKey<long> datakey, long delta, int expires)
        {
            var keybytes = m_keyEncoder.GetBytes(datakey.Key);
            m_commandWriter.Incr(new IncrPacket()
            {
                Key = keybytes,
                Delta = delta,
                InitialValue = datakey.Value,
                Expires = expires
            }, Options.NoReply);

            var succeed = true;
            return m_factory.Context(keybytes)((connection, state) =>
            {
                m_builder.WriteTo(connection.Writer);
                if (!Options.NoReply)
                {
                    var commandReader = m_factory.CreateCommandReader(
                        m_factory.CreatePacketParser(connection.Reader, m_buffer));

                    var increment = commandReader.ReadIncrement();
                    if (increment >= 0L)
                    {
                        datakey.Value = increment;
                        return;
                    }

                    int flags;
                    var bytes = m_formatter.Serialize(datakey.Value.ToString(CultureInfo.InvariantCulture), out flags);
                    m_commandWriter.Store(new StorePacket()
                    {
                        Operation = StoreOperation.Set, //// TODO: TextProtocol
                        Key = keybytes,
                        Flags = flags,
                        Expires = expires,
                        Value = bytes
                    }, false);
                    m_builder.WriteTo(connection.Writer);
                    succeed = commandReader.ReadStored();
                }
                else
                {
                    datakey.Value += delta;
                }
            }) && succeed;
        }

        /*
        public bool Increment1(DataKey<long> datakey, long delta, int expires)
        {
            var keybytes = m_keyEncoder.GetBytes(datakey.Key);
            m_commandWriter.Incr(new IncrPacket()
            {
                Key = keybytes,
                Delta = delta,
                InitialValue = datakey.Value,
                Expires = expires
            }, Options.NoReply);

            var increment = -1L;
            var succeed = m_factory.Context(keybytes)((connection, state) =>
            {
                m_builder.WriteTo(connection.Writer);
                if (!Options.NoReply)
                {
                    var commandReader = m_factory.CreateCommandReader(
                        m_factory.CreatePacketParser(connection.Reader, m_buffer));

                    increment = commandReader.ReadIncrement();
                }
            });

            if (succeed)
            {
                if (increment == -1L)
                {
                    increment = datakey.Value;
                    if (!Store(StoreOperation.Set, new DataKey<string>(datakey.Key)
                    {
                        Value = increment.ToString(CultureInfo.InvariantCulture)
                    }, expires))
                    {
                        increment = -1L;
                    }
                }
            }

            datakey.Value = increment;
            return succeed;
        } */

        #endregion
    }
}
