using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ReusableLibrary.Abstractions.Cryptography
{
    public static class CryptoTransformHelper
    {
        public static byte[] Encrypt(ICryptoTransform transform, byte[] data)
        {
            var s = Encrypt(transform, new ArraySegment<byte>(data, 0, data.Length));
            var result = s.Array;
            if (result.Length != s.Count)
            {
                result = new byte[s.Count];
                Buffer.BlockCopy(s.Array, s.Offset, result, 0, s.Count);
            }

            return result;
        }

        public static ArraySegment<byte> Encrypt(ICryptoTransform transform, ArraySegment<byte> data)
        {
            using (var source = new MemoryStream(data.Count))
            {
                using (var cs = new CryptoStream(source, transform, CryptoStreamMode.Write))
                {
                    cs.Write(data.Array, data.Offset, data.Count);
                    cs.FlushFinalBlock();
                    return new ArraySegment<byte>(source.GetBuffer(), 0, (int)source.Position);
                }
            }
        }

        public static byte[] Decrypt(ICryptoTransform transform, byte[] data)
        {
            var s = Decrypt(transform, new ArraySegment<byte>(data, 0, data.Length));
            var result = s.Array;
            if (result.Length != s.Count)
            {
                result = new byte[s.Count];
                Buffer.BlockCopy(s.Array, s.Offset, result, 0, s.Count);
            }

            return result;
        }

        public static ArraySegment<byte> Decrypt(ICryptoTransform transform, ArraySegment<byte> data)
        {
            using (var source = new MemoryStream(data.Array, data.Offset, data.Count, false, true))
            {
                using (var cs = new CryptoStream(source, transform, CryptoStreamMode.Read))
                {
                    using (var destination = new MemoryStream(data.Count))
                    {
                        var tmp = new byte[data.Count];
                        int read;
                        while ((read = cs.Read(tmp, 0, tmp.Length)) != 0)
                        {
                            destination.Write(tmp, 0, read);
                        }

                        return new ArraySegment<byte>(destination.GetBuffer(), 0, (int)destination.Position);
                    }
                }
            }
        }
    }
}
