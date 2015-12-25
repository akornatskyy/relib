using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class AbstractControllerTest : IDisposable
    {
        private readonly MockAbstractController m_mockAbstractController;
        private readonly Mock<HttpContextBase> m_mockHttpContext;

        public AbstractControllerTest()
        {
            m_mockAbstractController = new MockAbstractController();
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
            ControllerContext controllerContext = new ControllerContext(new RequestContext(m_mockHttpContext.Object, new RouteData()), m_mockAbstractController);
            m_mockAbstractController.ControllerContext = controllerContext;
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public static void Ajax()
        {
            // Arrange
            var controller = new MockAbstractController();

            // Act
            var result = MockAbstractController.Ajax2(controller.RedirectToAction2("Index", "MockAbstract"));

            // Assert
            Assert.IsType<AjaxRedirectToRouteResult>(result);
            Assert.Equal("MockAbstract", result.RouteValues["controller"]);
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void AlternatePartialView_Provider_IsNull()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => m_mockAbstractController.AlternatePartialView2(null));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void AlternatePartialView_ViewName_IsNullOrEmpty()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentNullException>(() => m_mockAbstractController.AlternatePartialView2(null, null));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void AlternatePartialView_Ajax_Request()
        {
            // Arrange
            var viewData = new object();
            m_mockHttpContext.Setup(context => context.Request["X-Requested-With"]).Returns("XMLHttpRequest");

            // Act
            var result = m_mockAbstractController.AlternatePartialView2("ajaxView", viewData) as PartialViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ajaxView", result.ViewName);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void AlternatePartialView_NonAjax_Request()
        {
            // Arrange
            var viewData = new object();

            // Act
            var result = m_mockAbstractController.AlternatePartialView2("ajaxView", viewData) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.ViewName);
            Assert.Equal("ajaxView", result.ViewData.AlternatePartialViewName());
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void HandleUnknownAction_Throws_HttpException_404()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<HttpException>(() => m_mockAbstractController.HandleUnknownAction2("test"));

            // Assert
            Assert.Equal(404, ex.GetHttpCode());
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void HandleAjaxError()
        {
            // Arrange
            m_mockAbstractController.ViewData = new ViewDataDictionary();
            m_mockAbstractController.ViewData.ModelState.AddModelError("key1", "my error");

            // Act
            var result = m_mockAbstractController.HandleAjaxError2() as ContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("my error", result.Content);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "AbstractController")]
        public void HandleAjaxError_But_NoError()
        {
            // Arrange
            m_mockAbstractController.ViewData = new ViewDataDictionary();

            // Act
            var result = m_mockAbstractController.HandleAjaxError2() as ContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Properties.Resources.ErrorUnspecified, result.Content);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockAbstractController.Dispose();
        }

        #endregion
    }
}
