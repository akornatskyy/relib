using System;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public abstract class EncryptedObjectFormatter : DecoratedObjectFormatter
    {
        public const int EncryptedFlag = 0x400;

        protected EncryptedObjectFormatter(IObjectFormatter inner)
            : base(inner)
        {
        }

        public override T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            if ((flags & EncryptedFlag) != EncryptedFlag)
            {
                return base.Deserialize<T>(data, flags);
            }

            return base.Deserialize<T>(Decrypt(data), flags & ~EncryptedFlag);
        }

        public override ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            var segment = base.Serialize<T>(value, out flags);
            var encrypted = Encrypt(segment);
            flags |= EncryptedFlag;
            return encrypted;
        }

        public abstract ArraySegment<byte> Encrypt(ArraySegment<byte> data);

        public abstract ArraySegment<byte> Decrypt(ArraySegment<byte> data);
    }
}
