using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public interface IKeyVectorProvider
    {
        Pair<byte[]> CreateKeyVector();
    }
}
