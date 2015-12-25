using System;
using System.Security.Cryptography;
using ReusableLibrary.Abstractions.Cryptography;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public sealed class SymmetricObjectFormatter : EncryptedObjectFormatter
    {
        private readonly ICryptoTransform m_encryptor;
        private readonly ICryptoTransform m_decryptor;

        public SymmetricObjectFormatter(ISymmetricAlgorithmProvider symmetricAlgorithmProvider, IObjectFormatter inner)
            : base(inner)
        {
            var algorithm = symmetricAlgorithmProvider.Create();
            m_encryptor = algorithm.CreateEncryptor();
            m_decryptor = algorithm.CreateDecryptor();
        }

        public override ArraySegment<byte> Encrypt(ArraySegment<byte> data)
        {
            return CryptoTransformHelper.Encrypt(m_encryptor, data);
        }

        public override ArraySegment<byte> Decrypt(ArraySegment<byte> data)
        {
            return CryptoTransformHelper.Decrypt(m_decryptor, data);
        }
    }
}
