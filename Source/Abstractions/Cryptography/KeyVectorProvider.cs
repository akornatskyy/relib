using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public class KeyVectorProvider : IKeyVectorProvider
    {
        private readonly Pair<byte[]> m_keyVector;

        public KeyVectorProvider(Pair<byte[]> keyVector)
        {
            m_keyVector = keyVector;
        }

        public KeyVectorProvider(byte[] key, byte[] vector)
            : this(new Pair<byte[]>(key, vector))
        {
        }

        #region IKeyVectorProvider Members

        public Pair<byte[]> CreateKeyVector()
        {
            return m_keyVector;
        }

        #endregion
    }
}
