using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class SymmetricAlgorithmProvider<TSymmetricAlgorithm> : ISymmetricAlgorithmProvider
        where TSymmetricAlgorithm : SymmetricAlgorithm, new()
    {
        private readonly IKeyVectorProvider m_keyVectorProvider;

        public SymmetricAlgorithmProvider(IKeyVectorProvider keyVectorProvider)
        {
            m_keyVectorProvider = keyVectorProvider;
        }

        #region ISymmetricAlgorithmProvider Members

        public SymmetricAlgorithm Create()
        {
            var kv = m_keyVectorProvider.CreateKeyVector();
            var algorithm = new TSymmetricAlgorithm()
            {
                Key = kv.First,
                IV = kv.Second
            };
            return algorithm;
        }

        #endregion
    }
}
