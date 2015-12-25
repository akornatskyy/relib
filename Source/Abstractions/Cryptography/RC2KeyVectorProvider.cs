namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class RC2KeyVectorProvider : KeyVectorProvider
    {
        public RC2KeyVectorProvider(string passphrase, int keySize)
            : base(RC2Helper.CreateKeyVector(passphrase, keySize))
        {
        }
    }
}
