using System.Web;
using Moq;
using ReusableLibrary.Web.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Internals
{
    public sealed class AjaxRedirectHelperTest
    {
        private readonly Mock<HttpContextBase> m_mockHttpContext;

        public AjaxRedirectHelperTest()
        {
            m_mockHttpContext = MockHttpFactory.GetHttpContext();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.TraitNames.Helpers, "AjaxRedirect")]
        public void AjaxRedirect_HttpContextBase_NonAjax(bool endResponse)
        {
            // Arrange           
            m_mockHttpContext.Setup(context => context.Response.Redirect("/c/a/i", endResponse)).Verifiable();

            // Act
            AjaxRedirectHelper.AjaxRedirect(m_mockHttpContext.Object, "/c/a/i", endResponse);

            // Assert
            m_mockHttpContext.Verify();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait(Constants.TraitNames.Helpers, "AjaxRedirect")]
        public void AjaxRedirect_HttpContextBase_Ajax(bool endResponse)
        {
            // Arrange
            m_mockHttpContext.Setup(context => context.Request["X-Requested-With"]).Returns("XMLHttpRequest");
            m_mockHttpContext.SetupProperty(context => context.Response.StatusCode);
            m_mockHttpContext.Setup(context => context.Response.AddHeader("Location", "/c/a/i")).Verifiable();

            // Act
            AjaxRedirectHelper.AjaxRedirect(m_mockHttpContext.Object, "/c/a/i", endResponse);

            // Assert
            Assert.Equal(AjaxRedirectHelper.AjaxRedirectStatusCode, m_mockHttpContext.Object.Response.StatusCode);
            m_mockHttpContext.Verify();
        }
    }
}
