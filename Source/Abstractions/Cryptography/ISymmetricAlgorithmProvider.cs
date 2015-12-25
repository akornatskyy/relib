using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public interface ISymmetricAlgorithmProvider
    {
        SymmetricAlgorithm Create();
    }
}
