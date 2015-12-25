using System;
using System.Collections.Specialized;
using System.Web;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests
{
    public sealed class HttpRequestBaseExtensionsTest
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;

        public HttpRequestBaseExtensionsTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
        }

        [Theory]
        [InlineData("example.org")]
        [InlineData("example.org:8080")]
        [Trait(Constants.TraitNames.Extensions, "HttpRequestBaseExtensions")]
        public void Host_From_Headers(string host)
        {
            // Arrange            
            var headers = new NameValueCollection();
            headers.Add("Host", host);
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);

            // Act
            host = m_mockHttpContext.Object.Request.Host();

            // Assert
            Assert.Equal("example.org", host);
        }

        [Fact]
        [Trait(Constants.TraitNames.Extensions, "HttpRequestBaseExtensions")]
        public void Host_From_Url()
        {
            // Arrange            
            var headers = new NameValueCollection();
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            m_mockHttpContext.Setup(context => context.Request.Url).Returns(new Uri("http://example.org/index.html?x=1"));

            // Act
            var host = m_mockHttpContext.Object.Request.Host();

            // Assert
            Assert.Equal("example.org", host);
        }

        [Theory]
        [InlineData("GET", true)]
        [InlineData("HEAD", true)]
        [InlineData("POST", false)]
        [Trait(Constants.TraitNames.Extensions, "HttpRequestBaseExtensions")]
        public void IsHttpVerbGetOrHead(string method, bool expected)
        {
            // Arrange            
            m_mockHttpContext.Setup(context => context.Request.HttpMethod).Returns(method);

            // Act
            var result = m_mockHttpContext.Object.Request.IsHttpVerbGetOrHead();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
