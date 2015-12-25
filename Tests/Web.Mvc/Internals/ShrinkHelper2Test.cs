using System;
using System.Text;
using ReusableLibrary.Web.Mvc.Internals;
using Xunit;
using Xunit.Extensions;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Supplemental.System;

namespace ReusableLibrary.Web.Mvc.Tests.Internals
{
    public sealed class ShrinkHelper2Test
    {
        private static readonly Random g_random = new Random();

        [Theory(Skip = "Not ready yet")]
        [InlineData(">x", ">x")]
        [InlineData("> x", "> x")]
        [InlineData("> x", ">  x")]
        [InlineData("><", "><")]
        [InlineData("><", ">  <")]
        [InlineData("x<", "x<")]
        [InlineData("x <", "x <")]
        [InlineData("x <", "x  <")]
        [InlineData("<>", "<>")]
        [InlineData("<>", "<  >")]
        [InlineData("<a>", "  <a>  ")]
        [InlineData("<a href=' '> text here </a>", " <a href=' '>\r\n  text here  </a>  ")]
        [InlineData("<br />", " <br />\t ")]
        [InlineData(" text </i>", " text </i>  ")]
        [InlineData("<", "\r\n<")]
        [InlineData("<", "  <")]
        [InlineData(">", ">\r\n")]
        [InlineData(">", ">  ")]
        [InlineData(" test(); test();", " test(); test();  ")]
        [InlineData("{ test(); test();}", " {\r\ntest(); test(); \r\n}  ")]
        [Trait(Constants.TraitNames.Mvc, "ShrinkHelper2")]
        public static void Shrink(string expected, string html)
        {
            // Arrange
            var buffer = Encoding.UTF8.GetBytes(html);

            // Act
            var length = ShrinkHelper2.Shrink(buffer, 0, buffer.Length);

            // Assert
            Assert.Equal(expected, Encoding.UTF8.GetString(buffer, 0, length));
        }

        [Theory]
        [InlineData(" <a href=' '>\r\n  text here  </a>  ")]
        [Trait(Constants.TraitNames.Mvc, "ShrinkHelper2")]
        public static void Shrink0(string html)
        {
            // Arrange
            ShrinkHelper.LeaveAtLeast = 0;

            // Act
            for (int i = 0; i < 100; i++)
            {
                var buffer = Encoding.UTF8.GetBytes(html.Repeat(1000));
                var length = ShrinkHelper.Shrink(buffer, 0, buffer.Length);
            }

            // Assert
        }

        [Theory]
        [InlineData(" <a href=' '>\r\n  text here  </a>  ")]
        [Trait(Constants.TraitNames.Mvc, "ShrinkHelper2")]
        public static void Shrink1(string html)
        {
            // Arrange
            ShrinkHelper.LeaveAtLeast = 1;

            // Act
            for (int i = 0; i < 100; i++)
            {                
                var buffer = Encoding.UTF8.GetBytes(html.Repeat(1000));
                var length = ShrinkHelper.Shrink(buffer, 0, buffer.Length);
            }

            // Assert
        }

        [Theory]
        [InlineData(" <a href=' '>\r\n  text here  </a>  ")]
        [Trait(Constants.TraitNames.Mvc, "ShrinkHelper2")]
        public static void Shrink2(string html)
        {
            // Arrange            

            // Act
            for (int i = 0; i < 100; i++)
            {                
                var buffer = Encoding.UTF8.GetBytes(html.Repeat(1000));
                var length = ShrinkHelper2.Shrink(buffer, 0, buffer.Length);                
            }

            // Assert
        }
    }
}
