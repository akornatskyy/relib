using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Integration
{
    public sealed class HttpContextExceptionHandlerTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Integration, "HttpContextExceptionHandler")]
        public static void ServerVariables_Is_Not_Set()
        {
            // Arrange
            var handler = new HttpContextExceptionHandler();

            // Act
            var result = handler.HandleException(new HttpException());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "HttpContextExceptionHandler")]
        public static void HttpContext_Is_Null()
        {
            // Arrange
            var handler = new HttpContextExceptionHandler();
            handler.ServerVariables = new[] { "URL", "REMOTE_ADDR" };

            // Act
            var result = handler.HandleException(new HttpException());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "HttpContextExceptionHandler")]
        public static void HandleException_But_No_Data_Available()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            var mockHandler = new Mock<HttpContextExceptionHandler>();
            mockHandler.SetupGet(h => h.RequestServerVariables).Returns(serverVariables);
            var handler = mockHandler.Object;
            handler.ServerVariables = new[] { "URL", "REMOTE_ADDR" };
            var ex = new InvalidOperationException();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
            Assert.Equal(0, ex.Data.Count);
            mockHandler.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "HttpContextExceptionHandler")]
        public static void HandleException()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("URL", "/Home/About");
            serverVariables.Add("REMOTE_ADDR", "127.0.0.1");
            var mockHandler = new Mock<HttpContextExceptionHandler>();
            mockHandler.SetupGet(h => h.RequestServerVariables).Returns(serverVariables);
            var handler = mockHandler.Object;
            handler.ServerVariables = new[] { "URL", "REMOTE_ADDR" };
            var ex = new InvalidOperationException();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
            Assert.Equal(2, ex.Data.Count);
            Assert.Equal("/Home/About", ex.Data["URL"]);
            Assert.Equal("127.0.0.1", ex.Data["REMOTE_ADDR"]);
            mockHandler.VerifyAll();
        }
    }
}
