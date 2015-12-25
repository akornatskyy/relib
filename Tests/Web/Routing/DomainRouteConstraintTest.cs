using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Routing;
using Xunit;

namespace ReusableLibrary.Web.Tests.Routing
{
    public sealed class DomainRouteConstraintTest
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;
        private readonly IRouteConstraint m_constraint;

        public DomainRouteConstraintTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
            m_constraint = new DomainRouteConstraint();
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "DomainRouteConstraint")]
        public void Match_IncomingRequest()
        {
            // Arrange          
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var values = new RouteValueDictionary();
            values.Add(DomainRouteConstraint.Name, "example.org");

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.IncomingRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "DomainRouteConstraint")]
        public void Match_IncomingRequest_Domain_Ignored()
        {
            // Arrange          
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var values = new RouteValueDictionary();
            values.Add(DomainRouteConstraint.Name, DomainRouteConstraint.Ignore);

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.IncomingRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "DomainRouteConstraint")]
        public void Match_UrlGeneration()
        {
            // Arrange          
            var values = new RouteValueDictionary();
            values.Add(DomainRouteConstraint.Name, "example.org");

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.UrlGeneration);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "DomainRouteConstraint")]
        public void Match_UrlGeneration_Domain_Empty()
        {
            // Arrange          
            var values = new RouteValueDictionary();
            values.Add(DomainRouteConstraint.Name, string.Empty);

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.UrlGeneration);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "DomainRouteConstraint")]
        public void Match_IncomingRequest_HasNoMatch()
        {
            // Arrange          
            var headers = new NameValueCollection();
            headers.Add("Host", "example.org");
            m_mockHttpContext.Setup(context => context.Request.Headers).Returns(headers);
            var values = new RouteValueDictionary();
            values.Add(DomainRouteConstraint.Name, "members.example.org");

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.IncomingRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "DomainRouteConstraint")]
        public void Match_UrlGeneration_HasNoMatch()
        {
            // Arrange          
            var values = new RouteValueDictionary();

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.UrlGeneration);

            // Assert
            Assert.False(result);
        }
    }
}
