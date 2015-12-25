using System;
using System.Security.Cryptography;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public interface ISymmetricAlgorithmContext : IDisposable
    {
        bool EncryptorContext(Action<ICryptoTransform> action);

        bool DecryptorContext(Action<ICryptoTransform> action);
    }
}
