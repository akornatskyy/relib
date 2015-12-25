using System;
using System.Collections.Generic;
using System.Text;
using ReusableLibrary.Abstractions.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Helpers
{
    public sealed class StringHelperTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("N/A")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void NullSafe(string defaultValue)
        {
            // Arrange

            // Act
            Assert.Equal(defaultValue, StringHelper.NullSafe(null, defaultValue));
            Assert.Equal(string.Empty, StringHelper.NullSafe(string.Empty, defaultValue));
            Assert.Equal(string.Empty, StringHelper.NullSafe("  ", defaultValue));
            Assert.Equal("ABC", StringHelper.NullSafe(" ABC ", defaultValue));

            // Assert
        }

        [Theory]
        [InlineData("")]
        [InlineData("N/A")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void NullEmpty(string nullValue)
        {
            // Arrange

            // Act
            Assert.Equal(null, StringHelper.NullEmpty(null, nullValue));
            Assert.Equal(null, StringHelper.NullEmpty(nullValue, nullValue));
            Assert.Equal(null, StringHelper.NullEmpty(nullValue + " ", nullValue));
            Assert.Equal("ABC", StringHelper.NullEmpty(" ABC ", nullValue));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void WrapAt_InvalidArguments()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => StringHelper.WrapAt(null, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringHelper.WrapAt("X", 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringHelper.WrapAt("X", -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringHelper.WrapAt("X", 10, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringHelper.WrapAt("X", 10, -1));

            // Assert
        }

        [Theory]
        [InlineData("X", 5, "X")]
        [InlineData("123456", 5, "12...")]
        [InlineData("12345", 5, "12345")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void WrapAt(string target, int length, string expected)
        {
            // Arrange

            // Act
            var result = StringHelper.WrapAt(target, length);

            // Assert
            Assert.True(result.Length <= length);
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void AllOf_InvalidArguments()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => StringHelper.AllOf(null, StringHelper.Numeric.ToCharArray()));
            Assert.Throws<ArgumentNullException>(() => StringHelper.AllOf("X", null));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringHelper.AllOf(string.Empty, StringHelper.Numeric.ToCharArray()));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => StringHelper.AllOf("X", StringHelper.Numeric.ToCharArray(), 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => StringHelper.AllOf("X", StringHelper.Numeric.ToCharArray(), 0, 100));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => StringHelper.AllOf("X", StringHelper.Numeric.ToCharArray(), 100, 1));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void AllOf()
        {
            // Arrange

            // Act
            Assert.True(StringHelper.AllOf(StringHelper.Numeric, StringHelper.Numeric.ToCharArray()));            
            Assert.False(StringHelper.AllOf("1X", StringHelper.Numeric.ToCharArray()));

            // Assert
        }

        [Theory]
        [InlineData("VC-vpIjpXE-_1jUceSmbdA", "a4af2f54-e988-4f5c-bfd6-351c79299b74")]
        [InlineData("iLp60cMZDkCt7j7Pk12ycg", "d17aba88-19c3-400e-adee-3ecf935db272")]
        [InlineData("7hOuOSog0UKRF2-2_dFppA", "39ae13ee-202a-42d1-9117-6fb6fdd169a4")]
        [InlineData("?|?|?|?|?|?|?|?|?|?|?|", "00000000-0000-0000-0000-000000000000")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void ToGuid(string target, string expected)
        {
            // Arrange

            // Act
            var result = StringHelper.ToGuid(target);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("<b>ABC</b>", "ABC")]
        [InlineData("<b> ABC </b>", " ABC ")]
        [InlineData("<pre><b>ABC</b>DE</pre>", "ABCDE")]
        [InlineData("<b>ABC</b><i>DE</i>", "ABCDE")]
        [InlineData("DE</i>", "DE")]
        [InlineData("<b>ABC", "ABC")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void StripHtml(string target, string expected)
        {
            // Arrange

            // Act
            var result = StringHelper.StripHtml(target);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("\rABC\n", "ABC")]
        [InlineData("\n\r ABC \r\n", " ABC ")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void StripNewLine(string target, string expected)
        {
            // Arrange

            // Act
            var result = StringHelper.StripNewLine(target);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("\rAB C\n", "AB C")]
        [InlineData("\n\r  AB  \r\nC", "AB C")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void StripWhitespace(string target, string expected)
        {
            // Arrange

            // Act
            var result = StringHelper.StripWhitespace(target);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Theory]
        [InlineData("ABC", "ABC")]
        [InlineData("A\r\nBC", "A<br />BC")]
        [InlineData("A\n\rBC", "A<br />BC")]
        [InlineData("A\n\nBC", "A<br /><br />BC")]
        [InlineData("A\rBC", "A<br />BC")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void NewLineToBr(string target, string expected)
        {
            // Arrange

            // Act
            var result = StringHelper.NewLineToBr(target);

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Capitalize()
        {
            // Arrange
            var str = "hello World";

            // Act
            var result = StringHelper.Capitalize(str);

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Theory]
        [InlineData("Hello World")]
        [InlineData("hello")]
        [InlineData("world")]
        [InlineData("or")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Contains(string value)
        {
            // Arrange
            var str = "Hello World";

            // Act
            var result = StringHelper.Contains(str, value, StringComparison.OrdinalIgnoreCase);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("yes")]
        [InlineData("no")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Does_Not_Contain(string value)
        {
            // Arrange
            var str = "Hello World";

            // Act
            var result = StringHelper.Contains(str, value, StringComparison.OrdinalIgnoreCase);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Left_NullOrEmpty(string value)
        {
            // Arrange

            // Act
            var result = StringHelper.Left(value, 10);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Left_More_Than_Exist()
        {
            // Arrange

            // Act
            var result = StringHelper.Left("x", 10);

            // Assert
            Assert.Equal("x", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Left_Less_Than_Exist()
        {
            // Arrange

            // Act
            var result = StringHelper.Left("123", 1);

            // Assert
            Assert.Equal("1", result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Right_NullOrEmpty(string value)
        {
            // Arrange

            // Act
            var result = StringHelper.Right(value, 10);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Right_More_Than_Exist()
        {
            // Arrange

            // Act
            var result = StringHelper.Left("x", 10);

            // Assert
            Assert.Equal("x", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Right_Less_Than_Exist()
        {
            // Arrange

            // Act
            var result = StringHelper.Right("123", 1);

            // Assert
            Assert.Equal("3", result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Repeat_NullOrEmpty(string value)
        {
            // Arrange

            // Act
            var result = StringHelper.Repeat(value, 10);

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(1)]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Repeat_Count_LessOrEqualOne(int count)
        {
            // Arrange

            // Act
            var result = StringHelper.Repeat("x", count);

            // Assert
            Assert.Equal("x", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Repeat()
        {
            // Arrange

            // Act
            var result = StringHelper.Repeat("x-", 2);

            // Assert
            Assert.Equal("x-x-", result);
        }

        [Theory]
        [InlineData("dlroW olleH", "Hello World")]
        [InlineData("olleh", "hello")]
        [InlineData("", "")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Reverse(string expected, string value)
        {
            // Arrange

            // Act
            var result = StringHelper.Reverse(value);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Hello World")]
        [InlineData("hello")]
        [InlineData("")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Join(string value)
        {
            // Arrange
            var list = new List<string>(value.Split(' '));

            // Act
            var result = StringHelper.Join(" ", list);

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("Server = localhost;")]
        [InlineData("Server = localhost")]
        [Trait(Constants.TraitNames.Models, "StringHelper")]
        public static void ParseOptions_SingleOption(string options)
        {
            // Arrange

            // Act
            var result = StringHelper.ParseOptions(options);

            // Assert
            Assert.Equal("localhost", result["server"]);
            Assert.Equal(1, result.Count);
        }

        [Theory]
        [InlineData("Server = localhost; Port = 11211")]
        [InlineData("server=localhost;port=11211")]
        [Trait(Constants.TraitNames.Models, "StringHelper")]
        public static void ParseOptions(string options)
        {
            // Arrange

            // Act
            var result = StringHelper.ParseOptions(options);

            // Assert
            Assert.Equal("localhost", result["server"]);
            Assert.Equal("11211", result["port"]);
            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData("Server ")]
        [InlineData("server;port")]
        [InlineData("server = x = y")]
        [InlineData("server = x = y;port= 7=0")]
        [Trait(Constants.TraitNames.Models, "StringHelper")]
        public static void ParseOptions_InvalidOptions(string options)
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(() => StringHelper.ParseOptions(options));

            // Assert
        }

        [Theory]
        [InlineData("Hello World", "6fc6VDBYCOw1DDu+PbNxuQ==", "MD5")]
        [InlineData("hello", "/Rht1JoWsb8r0vROSV4UyQ==", "MD5")]
        [InlineData("hello", "tteV+9WMx1ktlVohk3QzmjI4Aak=", "SHA1")]
        [InlineData("hello", "BuRNwblcRp9Dqsy0npPDaCdiYmbu1VdeztdK+aAWyc0=", "SHA256")]
        [InlineData("hello", "rtiqtMA5XVYgALkOGDiWOEMmINJ2nFg18Kv9HEUt5ivNnGw31QtT4Ym1LJ8yYCp0", "SHA384")]
        [InlineData("hello", "UWXVkqav5Z+A0HQ241vVE7MFVCmRZAChbBrfpJnFqM4Do3Cs3U3Hh9BDUEc76nHqg0V0hXj8Y6yR+PlbbBQLkw==", "SHA512")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Hash(string value, string expected, string hashName)
        {
            // Arrange

            // Act
            var result = Convert.ToBase64String(StringHelper.Hash(value, hashName));

            // Assert
            Assert.True(result.Length > 0);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Hello World", "sQqNsWTgdUEFt6mb5y4/5Q==", "MD5", "ASCII")]
        [InlineData("Hello World", "sQqNsWTgdUEFt6mb5y4/5Q==", "MD5", "UTF-8")]
        [InlineData("Hello World", "sQqNsWTgdUEFt6mb5y4/5Q==", "MD5", "UTF-7")]
        [InlineData("Hello World", "AMHpNWxnkEbGDw2BLG7TFg==", "MD5", "UTF-32")]
        [InlineData("hello", "/Rht1JoWsb8r0vROSV4UyQ==", "MD5", "Unicode")]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Hash_By_Encoding(string value, string expected, string hashName, string encoding)
        {
            // Arrange

            // Act
            var result = Convert.ToBase64String(StringHelper.Hash(value, hashName, Encoding.GetEncoding(encoding)));

            // Assert
            Assert.True(result.Length > 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "StringHelper")]
        public static void Hash_NullOrEmpty()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => StringHelper.Hash(null, "MD5"));
            Assert.Throws<ArgumentNullException>(() => StringHelper.Hash(string.Empty, "MD5"));
            Assert.Throws<ArgumentNullException>(() => StringHelper.Hash("test", null));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringHelper.Hash("test", "XXX"));

            // Assert
        }
    }
}
