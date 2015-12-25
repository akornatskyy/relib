using System;
using System.Text;
using ReusableLibrary.Supplemental.System;
using ReusableLibrary.Web.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Internals
{
    public sealed class ShrinkHelperTest
    {
        private const byte NotMatch = (byte)'x';
        private const byte IgnoreMatch = (byte)'\t';
        private static readonly byte[] Match = Encoding.ASCII.GetBytes("<");        
        private static readonly Random g_random = new Random();

        [Theory]
        [InlineData(" <a> ", "  <a>  ")]
        [InlineData(" <a href=' '>\rtext here </a> ", " <a href=' '>\r\n  text here  </a>  ")]
        [InlineData(" <br />\t", " <br />\t ")]
        [InlineData(" text </i> ", " text </i>  ")]
        [InlineData("\n<", "\r\n<")]
        [InlineData(" <", "  <")]
        [InlineData(">\r", ">\r\n")]
        [InlineData("> ",  ">  ")]
        [InlineData(" test(); test(); ", " test(); test();  ")]
        [InlineData(" {\rtest(); test();\n} ", " {\r\ntest(); test(); \r\n}  ")]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void Shrink_LeaveAtLeast_1(string expected, string html)
        {
            // Arrange
            ShrinkHelper.LeaveAtLeast = 1;
            var buffer = Encoding.UTF8.GetBytes(html);

            // Act
            var length = ShrinkHelper.Shrink(buffer, 0, buffer.Length);

            // Assert
            Assert.Equal(expected, Encoding.UTF8.GetString(buffer, 0, length));
        }

        [Theory]
        [InlineData("<a>", "  <a>  ")]
        [InlineData("<a href=' '>text here</a>", " <a href=' '>\r\n  text here  </a>  ")]
        [InlineData("<br />", " <br />\t ")]
        [InlineData(" text</i>", " text </i>  ")]
        [InlineData("<", "\r\n<")]
        [InlineData("<", "  <")]
        [InlineData(">", ">\r\n")]
        [InlineData(">", ">  ")]
        [InlineData(" test();test();", " test(); test();  ")]
        [InlineData("{test();test();}", " {\r\ntest(); test(); \r\n}  ")]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void Shrink_LeaveAtLeast_0(string expected, string html)
        {
            // Arrange
            ShrinkHelper.LeaveAtLeast = 0;
            var buffer = Encoding.UTF8.GetBytes(html);

            // Act
            var length = ShrinkHelper.Shrink(buffer, 0, buffer.Length);

            // Assert
            Assert.Equal(expected, Encoding.UTF8.GetString(buffer, 0, length));
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void IgnoreCurrent_HasMatch()
        {
            // Arrange
            var state = new ShrinkState()
            {
                InsideTag = g_random.NextBoolean(),
                IgnoreCount = g_random.NextInt(0, 4),
                Current = Match[0]
            };

            // Act
            var result = ShrinkHelper.IgnoreCurrent(Match, state);

            // Assert
            Assert.False(result);
            Assert.True(state.InsideTag);
            Assert.Equal(0, state.IgnoreCount);
            Assert.Equal(Match[0], state.Current);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void IgnoreCurrent_HasNoMatch_NotInsideTag()
        {
            // Arrange
            var state = new ShrinkState()
            {
                InsideTag = false,
                IgnoreCount = 10,
                Current = NotMatch
            };

            // Act
            var result = ShrinkHelper.IgnoreCurrent(Match, state);

            // Assert
            Assert.False(result);
            Assert.False(state.InsideTag);
            Assert.Equal(10, state.IgnoreCount);
            Assert.Equal(NotMatch, state.Current);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void IgnoreCurrent_InsideTag_HasIgnoreMatch_AtLeast_One()
        {
            // Arrange
            var state = new ShrinkState()
            {
                InsideTag = true,
                IgnoreCount = 4,
                Current = IgnoreMatch
            };

            // Act
            var result = ShrinkHelper.IgnoreCurrent(Match, state);

            // Assert
            Assert.True(result);
            Assert.True(state.InsideTag);
            Assert.Equal(5, state.IgnoreCount);
            Assert.Equal(IgnoreMatch, state.Current);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void IgnoreCurrent_InsideTag_HasIgnoreMatch_FirstTime()
        {
            // Arrange
            ShrinkHelper.LeaveAtLeast = 1;
            var state = new ShrinkState()
            {
                InsideTag = true,
                IgnoreCount = 0,
                Current = IgnoreMatch
            };

            // Act
            var result = ShrinkHelper.IgnoreCurrent(Match, state);

            // Assert
            Assert.False(result);
            Assert.True(state.InsideTag);
            Assert.Equal(1, state.IgnoreCount);
            Assert.Equal(IgnoreMatch, state.Current);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "ShrinkHelper")]
        public static void IgnoreCurrent_InsideTag_HasNoIgnoreMatch()
        {
            // Arrange
            var state = new ShrinkState()
            {
                InsideTag = true,
                IgnoreCount = 4,
                Current = NotMatch
            };

            // Act
            var result = ShrinkHelper.IgnoreCurrent(Match, state);

            // Assert
            Assert.False(result);
            Assert.False(state.InsideTag);
            Assert.Equal(4, state.IgnoreCount);
            Assert.Equal(NotMatch, state.Current);
        }
    }
}
