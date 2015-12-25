using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Memcached.Protocol
{
    public sealed class ValidKeyEncoder : IEncoder
    {
        private readonly IEncoder m_inner;

        public ValidKeyEncoder(IEncoder inner)
        {
            m_inner = inner;
        }

        public byte[] GetBytes(string s)
        {
            var bytes = m_inner.GetBytes(s);
            for (var i = 0; i < bytes.Length; i++)
            {
                var value = bytes[i];
                if (value == 32 || value == 13 || value == 10)
                {
                    bytes[i] = (byte)'_';
                }
            }

            return bytes;
        }
    }
}
