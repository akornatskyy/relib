namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class TripleDESKeyVectorProvider : KeyVectorProvider
    {
        public TripleDESKeyVectorProvider(string passphrase, int keySize)
            : base(TripleDESHelper.CreateKeyVector(passphrase, keySize))
        {
        }
    }
}
