using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;
using Moq;

namespace ReusableLibrary.Web.Tests
{
    public static class MockHttpFactory
    {
        public static readonly string AppPathModifier = string.Empty;

        public static Mock<HttpContextBase> GetHttpContext()
        {
            return GetHttpContext("/", null, null);
        }

        public static Mock<HttpContextBase> GetHttpContext(string appPath, string requestPath, string httpMethod)
        {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp.ToString(), -1);
        }

        public static Mock<HttpContextBase> GetHttpContext(string appPath, string requestPath, string httpMethod, string protocol, int port)
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            if (!String.IsNullOrEmpty(appPath))
            {
                mockHttpContext.SetupGet(c => c.Request.ApplicationPath).Returns(appPath);
            }

            if (!String.IsNullOrEmpty(requestPath))
            {
                mockHttpContext.SetupGet(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri = new Uri(protocol + "://localhost" + ":" + Convert.ToString(port >= 0 ? port : 80, CultureInfo.InvariantCulture));
            mockHttpContext.SetupGet(c => c.Request.Url).Returns(uri);
            mockHttpContext.SetupGet(c => c.Request.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod))
            {
                mockHttpContext.SetupGet(c => c.Request.HttpMethod).Returns(httpMethod);
            }

            mockHttpContext.SetupGet(c => c.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            return mockHttpContext;
        }
    }
}
