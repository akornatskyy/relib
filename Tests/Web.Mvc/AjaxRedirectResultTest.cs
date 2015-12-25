using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Mvc.Helpers;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class AjaxRedirectResultTest
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;
        private readonly ControllerContext m_controllerContext;
        private readonly AjaxRedirectResult m_action;

        public AjaxRedirectResultTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
            m_controllerContext = new ControllerContext(new RequestContext(m_mockHttpContext.Object, new RouteData()), new Mock<AbstractController>().Object);
            m_action = new AjaxRedirectResult("/c/a/i");
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AjaxRedirectResult")]
        public void ExecuteResult_NonAjax()
        {
            // Arrange           
            m_mockHttpContext.Setup(context => context.Response.Redirect("/c/a/i", false)).Verifiable();

            // Act
            m_action.ExecuteResult(m_controllerContext);

            // Assert
            m_mockHttpContext.Verify();
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AjaxRedirectResult")]
        public void ExecuteResult_Ajax()
        {
            // Arrange
            m_mockHttpContext.Setup(context => context.Request["X-Requested-With"]).Returns("XMLHttpRequest");
            m_mockHttpContext.SetupProperty(context => context.Response.StatusCode);
            m_mockHttpContext.Setup(context => context.Response.AddHeader("Location", "/c/a/i")).Verifiable();

            // Act
            m_action.ExecuteResult(m_controllerContext);

            // Assert
            Assert.Equal(ControllerContextHelper.AjaxRedirectStatusCode, m_mockHttpContext.Object.Response.StatusCode);
            m_mockHttpContext.Verify();
        }
    }
}
