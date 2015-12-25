using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using ReusableLibrary.Web.Mvc.Routing;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Routing
{
    public sealed class HttpResponseSubstitutionRouteHandlerTest
    {
        private readonly Mock<HttpContextBase> m_mockContext;

        public HttpResponseSubstitutionRouteHandlerTest()
        {
            m_mockContext = MockHttpFactory.GetHttpContext();
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "HttpResponseSubstitutionRouteHandler")]
        public void GetHttpHandler_Returns_HttpResponseSubstitutionRouteHandler()
        {
            // Arrange            
            m_mockContext.Setup(c => c.Request.HttpMethod).Returns("GET");
            var requestContext = new RequestContext(m_mockContext.Object, new RouteData());
            var handler = new HttpResponseSubstitutionRouteHandler();

            // Act
            var result = handler.GetHttpHandler(requestContext);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HttpResponseSubstitutionHandler>(result);
            Assert.Null(((HttpResponseSubstitutionHandler)result).State);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "HttpResponseSubstitutionRouteHandler")]
        public void GetHttpHandler_Returns_MvcHandler()
        {
            // Arrange            
            m_mockContext.Setup(c => c.Request.HttpMethod).Returns("POST");
            var requestContext = new RequestContext(m_mockContext.Object, new RouteData());
            var handler = new HttpResponseSubstitutionRouteHandler();

            // Act
            var result = handler.GetHttpHandler(requestContext);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<MvcHandler>(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "HttpResponseSubstitutionRouteHandler")]
        public void GetHttpHandler_AquireState()
        {
            // Arrange            
            m_mockContext.Setup(c => c.Request.HttpMethod).Returns("GET");
            var requestContext = new RequestContext(m_mockContext.Object, new RouteData());
            var handler = new HttpResponseSubstitutionRouteHandler()
            {
                StateProvider = x => 
                {
                    Assert.Same(requestContext, x);
                    return "x";
                }
            };

            // Act
            var result = handler.GetHttpHandler(requestContext);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HttpResponseSubstitutionHandler>(result);
            Assert.Equal("x", ((HttpResponseSubstitutionHandler)result).State);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "HttpResponseSubstitutionRouteHandler")]
        public void GetHttpHandler_StartEndCallbacks()
        {
            // Arrange            
            m_mockContext.Setup(c => c.Request.HttpMethod).Returns("GET");
            var requestContext = new RequestContext(m_mockContext.Object, new RouteData());
            ParameterizedHttpResponseSubstitutionCallback startCallback = (c, s) => string.Empty;
            ParameterizedHttpResponseSubstitutionCallback endCallback = (c, s) => string.Empty;
            var handler = new HttpResponseSubstitutionRouteHandler()
            {
                StartCallback = startCallback,
                EndCallback = endCallback
            };

            // Act
            var result = handler.GetHttpHandler(requestContext);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HttpResponseSubstitutionHandler>(result);
            var httpHandler = (HttpResponseSubstitutionHandler)result;
            Assert.Same(startCallback, httpHandler.StartCallback);
            Assert.Same(endCallback, httpHandler.EndCallback);
        }
    }
}
