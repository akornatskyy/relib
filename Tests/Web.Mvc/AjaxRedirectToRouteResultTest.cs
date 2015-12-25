using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Mvc.Helpers;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class AjaxRedirectToRouteResultTest : IDisposable
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;
        private readonly ControllerContext m_controllerContext;
        private readonly AjaxRedirectToRouteResult m_action;

        public AjaxRedirectToRouteResultTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
            m_controllerContext = new ControllerContext(new RequestContext(m_mockHttpContext.Object, new RouteData()), new Mock<AbstractController>().Object);
            m_action = new AjaxRedirectToRouteResult(new RouteValueDictionary(new { Controller = "c", Action = "a", Id = "i" }));

            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = string.Empty });
        }

        #region IDisposable Members

        public void Dispose()
        {
            RouteTable.Routes.Clear();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AjaxRedirectToRouteResult")]
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
        [Trait(Constants.TraitNames.Mvc, "AjaxRedirectToRouteResult")]
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
