using System;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Serialization.Formatters
{
    public sealed class SimpleObjectFormatter : DecoratedObjectFormatter
    {
        private static readonly byte[] Empty = new byte[] { };

        private readonly Encoding m_encoding;

        public SimpleObjectFormatter(Encoding encoding, IObjectFormatter inner)
            : base(inner)
        {
            m_encoding = encoding;
        }

        public override T Deserialize<T>(ArraySegment<byte> data, int flags)
        {
            TypeCode typeCode = (TypeCode)(flags & TypeCodeMask);
            object value;
            switch (typeCode)
            {
                case TypeCode.Int32:
                    value = BitConverter.ToInt32(data.Array, data.Offset);
                    break;
                case TypeCode.Boolean:
                    value = BitConverter.ToBoolean(data.Array, data.Offset);
                    break;
                case TypeCode.String:
                    value = m_encoding.GetString(data.Array, data.Offset, data.Count);
                    break;
                case TypeCode.DateTime:
                    value = new DateTime(BitConverter.ToInt64(data.Array, data.Offset));
                    break;
                case TypeCode.Int64:
                    value = BitConverter.ToInt64(data.Array, data.Offset);
                    break;
                case TypeCode.Decimal:
                    value = DecimalHelper.ToDecimal(data.Array, data.Offset);
                    break;
                case TypeCode.Byte:
                    value = data.Array[data.Offset];
                    break;
                case TypeCode.Char:
                    value = BitConverter.ToChar(data.Array, data.Offset);
                    break;
                case TypeCode.DBNull:
                    value = DBNull.Value;
                    break;
                case TypeCode.Double:
                    value = BitConverter.ToDouble(data.Array, data.Offset);
                    break;
                case TypeCode.Int16:
                    value = BitConverter.ToInt16(data.Array, data.Offset);
                    break;
                case TypeCode.SByte:
                    value = data.Array[data.Offset];
                    break;
                case TypeCode.Single:
                    value = BitConverter.ToSingle(data.Array, data.Offset);
                    break;
                case TypeCode.UInt16:
                    value = BitConverter.ToUInt16(data.Array, data.Offset);
                    break;
                case TypeCode.UInt32:
                    value = BitConverter.ToUInt32(data.Array, data.Offset);
                    break;
                case TypeCode.UInt64:
                    value = BitConverter.ToUInt64(data.Array, data.Offset);
                    break;
                default:
                    return base.Deserialize<T>(data, flags);
            }

            return (T)value;
        }

        public override ArraySegment<byte> Serialize<T>(T value, out int flags)
        {
            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            byte[] data;
            switch (typeCode)
            {
                case TypeCode.Int32:
                    data = BitConverter.GetBytes((int)(object)value);
                    break;
                case TypeCode.Boolean:
                    data = BitConverter.GetBytes((bool)(object)value);
                    break;
                case TypeCode.String:
                    data = m_encoding.GetBytes((string)(object)value);
                    break;
                case TypeCode.DateTime:
                    data = BitConverter.GetBytes(((DateTime)(object)value).Ticks);
                    break;
                case TypeCode.Int64:
                    data = BitConverter.GetBytes((long)(object)value);
                    break;
                case TypeCode.Decimal:
                    data = DecimalHelper.GetBytes(((decimal)(object)value));
                    break;
                case TypeCode.Byte:
                    data = new byte[] { ((byte)(object)value) };
                    break;
                case TypeCode.Char:
                    data = BitConverter.GetBytes((char)(object)value);
                    break;
                case TypeCode.DBNull:
                    data = Empty;
                    break;
                case TypeCode.Double:
                    data = BitConverter.GetBytes((double)(object)value);
                    break;
                case TypeCode.Int16:
                    data = BitConverter.GetBytes((short)(object)value);
                    break;
                case TypeCode.Single:
                    data = BitConverter.GetBytes((float)(object)value);
                    break;
                case TypeCode.UInt16:
                    data = BitConverter.GetBytes((ushort)(object)value);
                    break;
                case TypeCode.UInt32:
                    data = BitConverter.GetBytes((uint)(object)value);
                    break;
                case TypeCode.UInt64:
                    data = BitConverter.GetBytes((ulong)(object)value);
                    break;
                default:
                    return base.Serialize<T>(value, out flags);
            }

            flags = (int)typeCode;
            return new ArraySegment<byte>(data, 0, data.Length);
        }
    }
}
