using System;

namespace ReusableLibrary.Memcached.Protocol
{
    public class BinaryCommandReader : ICommandReader
    {
        private readonly IPacketParser m_parser;

        public BinaryCommandReader(IPacketParser parser)
        {
            m_parser = parser;
        }

        #region ICommandReader Members

        public void ReadSucceed()
        {
        }

        public ValuePacket ReadValue(bool includeVersion)
        {
            var status = m_parser.ReadStatus();
            if (status == ResponseStatus.Value)
            {
                var key = m_parser.ReadKey();
                var flags = m_parser.ReadFlags();
                var length = m_parser.ReadLength();
                var version = includeVersion ? m_parser.ReadVersion() : 0;
                var value = m_parser.ReadValue(length);

                return new ValuePacket()
                {
                    Flags = flags,
                    Key = key,
                    Value = value,
                    Version = version
                };
            }

            if (status != ResponseStatus.NoError && status != ResponseStatus.KeyNotFound)
            {
                throw new InvalidOperationException();
            }

            return null;
        }
        
        public bool ReadStored()
        {
            var status = m_parser.ReadStatus();
            return status == ResponseStatus.NoError;
        }

        public bool ReadDeleted()
        {
            var status = m_parser.ReadStatus();
            return status == ResponseStatus.NoError || status == ResponseStatus.KeyNotFound;
        }

        public long ReadIncrement()
        {
            var status = m_parser.ReadStatus();
            if (status != ResponseStatus.NoError)
            {
                return -1L;
            }

            return m_parser.ReadIncrement();
        }

        #endregion
    }
}
