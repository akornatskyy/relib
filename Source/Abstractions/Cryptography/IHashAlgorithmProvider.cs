using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public interface IHashAlgorithmProvider
    {
        HashAlgorithm Create();
    }
}
