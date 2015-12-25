namespace ReusableLibrary.Abstractions.Cryptography
{
    public sealed class DESKeyVectorProvider : KeyVectorProvider
    {
        public DESKeyVectorProvider(string passphrase)
            : base(DESHelper.CreateKeyVector(passphrase))
        {
        }
    }
}
