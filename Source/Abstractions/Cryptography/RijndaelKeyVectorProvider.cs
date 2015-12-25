namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class RijndaelKeyVectorProvider : KeyVectorProvider
    {
        public RijndaelKeyVectorProvider(string passphrase, int keySize)
            : base(RijndaelHelper.CreateKeyVector(passphrase, keySize))
        {
        }
    }
}
