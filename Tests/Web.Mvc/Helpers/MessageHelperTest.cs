using System.Web.Mvc;
using ReusableLibrary.Web.Mvc.Helpers;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Helpers
{
    public sealed class MessageHelperTest
    {
        private readonly HtmlHelper m_helper;

        public MessageHelperTest()
        {
            m_helper = MockHttpFactory.GetHtmlHelper();
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MessageHelper")]
        public void ShowModelError()
        {
            // Arrange
            m_helper.ViewData.ModelState.AddModelError("__ERROR__", "This is my error message");

            // Act
            var result = m_helper.ShowModelError().ToHtmlString();

            // Assert
            Assert.Equal("<span class=\"error-message\">This is my error message</span>", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MessageHelper")]
        public void ShowModelError_But_There_Is_No_ModelError()
        {
            // Arrange

            // Act
            var result = m_helper.ShowModelError();

            // Assert
            Assert.Equal(MvcHtmlString.Empty, result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MessageHelper")]
        public void ShowMessage()
        {
            // Arrange

            // Act
            var result = m_helper.ShowMessage("My message").ToHtmlString();

            // Assert
            Assert.Equal("<span class=\"info-message\">My message</span>", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MessageHelper")]
        public void ShowMessage_With_Custom_CssClass()
        {
            // Arrange

            // Act
            var result = m_helper.ShowMessage("My message", "info").ToHtmlString();

            // Assert
            Assert.Equal("<span class=\"info\">My message</span>", result);
        }
    }
}
