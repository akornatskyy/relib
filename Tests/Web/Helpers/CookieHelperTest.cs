using System;
using System.Web;
using ReusableLibrary.Web.Helpers;
using Xunit;

namespace ReusableLibrary.Web.Tests.Helpers
{
    public sealed class CookieHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "CookieHelper")]
        public static void ResetCookies()
        {
            // Arrange
            var requestCookies = new HttpCookieCollection();
            requestCookies.Add(new HttpCookie("x1"));
            requestCookies.Add(new HttpCookie("x2"));
            var responseCookies = new HttpCookieCollection();
            var context = MockHttpFactory.GetHttpContext();
            context.Setup(c => c.Request.Cookies).Returns(requestCookies);
            context.Setup(c => c.Response.Cookies).Returns(responseCookies);
            var request = context.Object.Request;
            var response = context.Object.Response;

            // Act
            CookieHelper.ResetCookies(context.Object);

            // Assert
            Assert.Equal(0, request.Cookies.Count);
            Assert.Equal(2, response.Cookies.Count);

            var x1 = response.Cookies[0];
            Assert.Equal("x1", x1.Name);
            Assert.True(x1.Expires <= DateTime.UtcNow.AddYears(-1));
            var x2 = response.Cookies[1];
            Assert.Equal("x2", x2.Name);
            Assert.True(x2.Expires <= DateTime.UtcNow.AddYears(-1));
        }
    }
}
