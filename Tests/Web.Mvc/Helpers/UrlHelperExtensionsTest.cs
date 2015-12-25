using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Mvc.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Mvc.Tests.Helpers
{
    public sealed class UrlHelperExtensionsTest
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;

        public UrlHelperExtensionsTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void Domain()
        {
            // Arrange            
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);

            // Act
            var result = UrlHelperExtensions.Domain(m_mockHttpContext.Object, "members.example.org");

            // Assert
            Assert.Equal("members.example.org", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void Domain_Match()
        {
            // Arrange            
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);

            // Act
            var result = UrlHelperExtensions.Domain(m_mockHttpContext.Object, "example.org");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void Domain_NotSpecified()
        {
            // Arrange            

            // Act
            var result = UrlHelperExtensions.Domain(m_mockHttpContext.Object, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void Scheme()
        {
            // Arrange            
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", "http");
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);

            // Act
            var result = UrlHelperExtensions.Scheme(m_mockHttpContext.Object, "https");

            // Assert
            Assert.Equal("https", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void Scheme_Match()
        {
            // Arrange            
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", "https");
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);

            // Act
            var result = UrlHelperExtensions.Scheme(m_mockHttpContext.Object, "https");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void Scheme_NotSpecified()
        {
            // Arrange            

            // Act
            var result = UrlHelperExtensions.Scheme(m_mockHttpContext.Object, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Domain()
        {
            // Arrange   
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", "http");
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var defaults = new RouteValueDictionary();
            defaults.Add("domain", "members.example.org");
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal("http://members.example.org/index.html", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("example.org")]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Domain_Match(string routeDomain)
        {
            // Arrange            
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var defaults = new RouteValueDictionary();
            defaults.Add("domain", routeDomain);
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal("/index.html", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Domain_NotSpecified()
        {
            // Arrange
            var defaults = new RouteValueDictionary();
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal("/index.html", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Scheme()
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", "http");
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var defaults = new RouteValueDictionary();
            defaults.Add("scheme", "https");
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal("https://example.org/index.html", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("https")]       
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Scheme_Match(string routeScheme)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", "https");
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);
            var defaults = new RouteValueDictionary();
            defaults.Add("scheme", routeScheme);
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal("/index.html", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Scheme_NotSpecified()
        {
            // Arrange
            var defaults = new RouteValueDictionary();
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal("/index.html", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRouteUrl_Unspecified_Route()
        {
            // Arrange
            var routeData = new RouteData();
            var routes = new RouteCollection();
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            Assert.Throws<ArgumentException>(() 
                => UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null));

            // Assert
        }

        [Theory]
        [InlineData("http", "http", "http://members.example.org/index.html")]
        [InlineData("http", "https", "https://members.example.org/index.html")]
        [InlineData("http", "", "http://members.example.org/index.html")]
        [InlineData("http", null, "http://members.example.org/index.html")]
        [InlineData("https", "http", "http://members.example.org/index.html")]        
        [InlineData("https", "https", "https://members.example.org/index.html")]
        [InlineData("https", "", "https://members.example.org/index.html")]
        [InlineData("https", null, "https://members.example.org/index.html")]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRoute_Scheme_And_Domain_Change(string requestScheme, string routeScheme, string expected)
        {
            // Arrange            
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", requestScheme);
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var defaults = new RouteValueDictionary();
            defaults.Add("domain", "members.example.org");
            defaults.Add("scheme", routeScheme);
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("http", "https", "example.org")]
        [InlineData("https", "http", "example.org")]
        [InlineData("http", "https", "")]
        [InlineData("http", "https", null)]
        [InlineData("https", "http", "")]
        [InlineData("https", "http", null)]
        [Trait(Constants.TraitNames.Helpers, "UrlHelperExtensions")]
        public void AbsoluteRoute_SchemeChange(string requestScheme, string routeScheme, string routeDomain)
        {
            // Arrange            
            var variables = new NameValueCollection();
            variables.Add("X_FORWARDED_PROTO", requestScheme);
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(variables);
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var defaults = new RouteValueDictionary();
            defaults.Add("domain", routeDomain);
            defaults.Add("scheme", routeScheme);
            var route = new Route("index.html", defaults, new StopRoutingHandler());
            var routeData = new RouteData();
            var routes = new RouteCollection();
            routes.Add("x", route);
            var helper = new UrlHelper(new RequestContext(m_mockHttpContext.Object, routeData), routes);

            // Act
            var result = UrlHelperExtensions.AbsoluteRouteUrl(helper, m_mockHttpContext.Object, "x", null);

            // Assert
            Assert.Equal(routeScheme + "://example.org/index.html", result);
        }
    }
}
