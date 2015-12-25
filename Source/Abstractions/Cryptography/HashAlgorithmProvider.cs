using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class HashAlgorithmProvider<THashAlgorithm> : IHashAlgorithmProvider
        where THashAlgorithm : HashAlgorithm, new()
    {
        #region IHashAlgorithmProvider Members

        public HashAlgorithm Create()
        {
            return new THashAlgorithm();
        }

        #endregion
    }
}
