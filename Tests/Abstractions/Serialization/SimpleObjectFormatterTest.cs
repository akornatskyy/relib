using System;
using System.Text;
using Moq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Serialization.Formatters;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Serialization
{
    public sealed class SimpleObjectFormatterTest : IDisposable
    {
        private static readonly Random g_random = new Random();

        private readonly Mock<IObjectFormatter> m_mockInner;

        public SimpleObjectFormatterTest()
        {
            m_mockInner = new Mock<IObjectFormatter>(MockBehavior.Strict);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockInner.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Int32()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = RandomHelper.NextInt(g_random, Int32.MinValue, Int32.MaxValue);

            // Act
            var result = formatter.Serialize<int>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Int32, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Boolean()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = RandomHelper.NextBoolean(g_random);

            // Act
            var result = formatter.Serialize<bool>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Boolean, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_String()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = RandomHelper.NextString(g_random, 100, StringHelper.AlphabetLowerCase);

            // Act
            var result = formatter.Serialize<string>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.String, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(Encoding.UTF8.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_DateTime()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = RandomHelper.NextDate(g_random, 100);

            // Act
            var result = formatter.Serialize<DateTime>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.DateTime, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value.Ticks), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Int64()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (long)45642342343;

            // Act
            var result = formatter.Serialize<long>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Int64, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Enum()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = TypeCode.DBNull;

            // Act
            var result = formatter.Serialize<TypeCode>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Int32, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes((int)value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Decimal()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = 100M;

            // Act
            var result = formatter.Serialize<decimal>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Decimal, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(DecimalHelper.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Byte()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (byte)100;

            // Act
            var result = formatter.Serialize<byte>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Byte, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(new byte[] { value }, result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Char()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = 'x';

            // Act
            var result = formatter.Serialize<char>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Char, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_DBNull()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = DBNull.Value;

            // Act
            var result = formatter.Serialize<DBNull>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.DBNull, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Double()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = g_random.NextDouble() * 1234567890.987654321;

            // Act
            var result = formatter.Serialize<double>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Double, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Int16()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (short)23454;

            // Act
            var result = formatter.Serialize<short>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Int16, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_Single()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (float)(RandomHelper.NextInt(g_random, 100, 100000) * 2.3435);

            // Act
            var result = formatter.Serialize<float>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.Single, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_UInt16()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (ushort)RandomHelper.NextInt(g_random, 100, 23423);

            // Act
            var result = formatter.Serialize<ushort>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.UInt16, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_UInt32()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (uint)RandomHelper.NextInt(g_random, 100, 23423);

            // Act
            var result = formatter.Serialize<uint>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.UInt32, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_UInt64()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = (ulong)RandomHelper.NextInt(g_random, 100, 23423);

            // Act
            var result = formatter.Serialize<ulong>(value, out flags);

            // Assert
            Assert.Equal((int)TypeCode.UInt64, flags);
            Assert.NotNull(result.Array);
            Assert.Equal(BitConverter.GetBytes(value), result.Array);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Serialize_DoesNot_Handle()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = -1;
            var value = new object();
            m_mockInner.Setup(inner => inner.Serialize<object>(value, out flags)).Returns(new ArraySegment<byte>());

            // Act
            var result = formatter.Serialize<object>(value, out flags);

            // Assert
            Assert.Equal(-1, flags);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Int32()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Int32;
            var value = RandomHelper.NextInt(g_random, Int32.MinValue, Int32.MaxValue);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<int>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Boolean()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Boolean;
            var value = RandomHelper.NextBoolean(g_random);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<bool>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_String()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.String;
            var value = RandomHelper.NextString(g_random, 100, StringHelper.AlphabetLowerCase);
            var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(value));

            // Act
            var result = formatter.Deserialize<string>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_DateTime()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.DateTime;
            var value = RandomHelper.NextDate(g_random, 100);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value.Ticks));

            // Act
            var result = formatter.Deserialize<DateTime>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Int64()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Int64;
            var value = (long)12456456234243;
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<long>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Enum()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Int32;
            var value = TypeCode.DBNull;
            var data = new ArraySegment<byte>(BitConverter.GetBytes((int)value));

            // Act
            var result = formatter.Deserialize<TypeCode>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Decimal()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Decimal;
            var value = RandomHelper.NextInt(g_random, -10000, 10000) / 100M;
            var data = new ArraySegment<byte>(DecimalHelper.GetBytes(value));

            // Act
            var result = formatter.Deserialize<decimal>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Byte()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Byte;
            var value = (byte)RandomHelper.NextInt(g_random, 0, 100);
            var data = new ArraySegment<byte>(new byte[] { value });

            // Act
            var result = formatter.Deserialize<byte>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Char()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Char;
            var value = 'x';
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<char>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_DBNull()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.DBNull;
            var data = new ArraySegment<byte>(new byte[] { });

            // Act
            var result = formatter.Deserialize<DBNull>(data, flags);

            // Assert
            Assert.Equal(DBNull.Value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Double()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Double;
            var value = g_random.NextDouble();
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<double>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Int16()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Int16;
            var value = (short)RandomHelper.NextInt(g_random, 0, 32032);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<short>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_Single()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.Single;
            var value = (float)(RandomHelper.NextInt(g_random, 100, 234234) * 3.0323);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<float>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_UInt16()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.UInt16;
            var value = (ushort)RandomHelper.NextInt(g_random, 100, 23424);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<ushort>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_UInt32()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.UInt32;
            var value = (uint)RandomHelper.NextInt(g_random, 100, 23424);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<uint>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_UInt64()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = (int)TypeCode.UInt64;
            var value = (ulong)RandomHelper.NextInt(g_random, 100, 23424);
            var data = new ArraySegment<byte>(BitConverter.GetBytes(value));

            // Act
            var result = formatter.Deserialize<ulong>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Serialization, "SimpleObjectFormatter")]
        public void Deserialize_DoesNot_Handle()
        {
            // Arrange
            var formatter = new SimpleObjectFormatter(Encoding.UTF8, m_mockInner.Object);
            var flags = 0;
            var value = new object();
            var data = new ArraySegment<byte>(new byte[] { });
            m_mockInner.Setup(inner => inner.Deserialize<object>(data, flags)).Returns(value);

            // Act
            var result = formatter.Deserialize<object>(data, flags);

            // Assert
            Assert.Equal(value, result);
        }
    }
}
