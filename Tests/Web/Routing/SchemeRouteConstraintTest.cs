using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Routing;
using Xunit;

namespace ReusableLibrary.Web.Tests.Routing
{
    public sealed class SchemeRouteConstraintTest
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;
        private readonly IRouteConstraint m_constraint;
        private readonly NameValueCollection m_variables;

        public SchemeRouteConstraintTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
            m_constraint = new SchemeRouteConstraint();
            m_variables = new NameValueCollection();
            m_variables.Add("X_FORWARDED_PROTO", "https");
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "SchemeRouteConstraint")]
        public void Match_IncomingRequest()
        {
            // Arrange          
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(m_variables);
            var values = new RouteValueDictionary();
            values.Add(SchemeRouteConstraint.Name, "https");

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.IncomingRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "SchemeRouteConstraint")]
        public void Match_IncomingRequest_Schema_Ignored()
        {
            // Arrange          
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(m_variables);
            var values = new RouteValueDictionary();
            values.Add(SchemeRouteConstraint.Name, SchemeRouteConstraint.Ignore);

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.IncomingRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "SchemeRouteConstraint")]
        public void Match_UrlGeneration()
        {
            // Arrange          
            var values = new RouteValueDictionary();
            values.Add(SchemeRouteConstraint.Name, "http");

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.UrlGeneration);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "SchemeRouteConstraint")]
        public void Match_UrlGeneration_Schema_Ignored()
        {
            // Arrange          
            var values = new RouteValueDictionary();
            values.Add(SchemeRouteConstraint.Name, SchemeRouteConstraint.Ignore);

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.UrlGeneration);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "SchemeRouteConstraint")]
        public void Match_IncomingRequest_NoMatch()
        {
            // Arrange          
            m_mockHttpContext.Setup(context => context.Request.ServerVariables).Returns(m_variables);
            var values = new RouteValueDictionary();
            values.Add(SchemeRouteConstraint.Name, "http");

            // Act
            var result = m_constraint.Match(m_mockHttpContext.Object, null, null, values, RouteDirection.IncomingRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Routing, "SchemeRouteConstraint")]
        public void Match_UrlGeneration_NoMatch()
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
