using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class CompressAttributeTest
    {
        private readonly ResultExecutedContext m_filterContext;

        public CompressAttributeTest()
        {
            var mockHttpContext = MockHttpFactory.GetHttpContext();
            var mockFilterStream = new Mock<Stream>(MockBehavior.Strict);
            mockHttpContext.SetupGet(context => context.Request.Headers).Returns(new NameValueCollection());
            mockHttpContext.SetupProperty(context => context.Response.Filter, mockFilterStream.Object);
            mockFilterStream.SetupGet(filter => filter.CanWrite).Returns(true);

            ControllerContext controllerContext = new ControllerContext(new RequestContext(mockHttpContext.Object, new RouteData()), new Mock<AbstractController>().Object);
            m_filterContext = new ResultExecutedContext(controllerContext, new ContentResult(), false, null);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "CompressAttribute")]
        public void OnResultExecuted_Gzip()
        {
            // Arrange
            var headers = m_filterContext.HttpContext.Request.Headers;
            headers.Add("Accept-Encoding", "gzip");
            var filter = m_filterContext.HttpContext.Response.Filter;
            Assert.IsNotType<GZipStream>(filter);
            var attr = new CompressAttribute();

            // Act
            attr.OnResultExecuted(m_filterContext);

            // Assert
            filter = m_filterContext.HttpContext.Response.Filter;
            Assert.NotNull(filter);
            Assert.IsType<GZipStream>(filter);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "CompressAttribute")]
        public void OnResultExecuted_Deflate()
        {
            // Arrange
            var headers = m_filterContext.HttpContext.Request.Headers;
            headers.Add("Accept-Encoding", "deflate");            
            var filter = m_filterContext.HttpContext.Response.Filter;
            Assert.IsNotType<DeflateStream>(filter);
            var attr = new CompressAttribute();

            // Act
            attr.OnResultExecuted(m_filterContext);

            // Assert
            filter = m_filterContext.HttpContext.Response.Filter;
            Assert.NotNull(filter);
            Assert.IsType<DeflateStream>(filter);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "CompressAttribute")]
        public void OnResultExecuted_ExceptionUnhandled()
        {
            // Arrange
            var filter = m_filterContext.HttpContext.Response.Filter;
            m_filterContext.Exception = new InvalidOperationException();
            m_filterContext.ExceptionHandled = false;
            var attr = new CompressAttribute();            

            // Act
            attr.OnResultExecuted(m_filterContext);

            // Assert
            var filterAfter = m_filterContext.HttpContext.Response.Filter;
            Assert.Same(filter, filterAfter);
        }
    }
}
