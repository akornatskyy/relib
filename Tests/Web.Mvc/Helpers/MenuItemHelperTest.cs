using System.Web.Mvc;
using ReusableLibrary.Web.Mvc.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Mvc.Tests.Helpers
{
    public sealed class MenuItemHelperTest
    {
        private readonly HtmlHelper m_helper;

        public MenuItemHelperTest()
        {
            m_helper = MockHttpFactory.GetHtmlHelper();
            m_helper.ViewContext.RouteData.Values["controller"] = "Home";
            m_helper.ViewContext.RouteData.Values["action"] = "Index";
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MenuItemHelper")]
        public void MenuItem_Inactive()
        {
            // Arrange

            // Act
            var result = m_helper.MenuItem("About Us", "About", "Home").ToHtmlString();

            // Assert
            Assert.Equal(@"<li><a href=""/Home/About"">About Us</a></li>", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MenuItemHelper")]
        public void MenuItem_Active()
        {
            // Arrange

            // Act
            var result = m_helper.MenuItem("Hello", "Index", "Home").ToHtmlString();

            // Assert
            Assert.Equal(@"<li class=""active""><a href=""/Home/Index"">Hello</a></li>", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "MenuItemHelper")]
        public void MenuItem_Active_With_Custom_CssClass()
        {
            // Arrange

            // Act
            var result = m_helper.MenuItem("Hello", "Index", "Home", "selected").ToHtmlString();

            // Assert
            Assert.Equal(@"<li class=""selected""><a href=""/Home/Index"">Hello</a></li>", result);
        }

        [Theory]
        [InlineData(true, "Index", "Home")]
        [InlineData(true, "Index", null)]
        [InlineData(false, null, null)]
        [InlineData(false, "Abount", "Home")]
        [Trait(Constants.TraitNames.Helpers, "MenuItemHelper")]
        public void IsCurrentAction(bool expected, string action, string controller)
        {
            // Arrange

            // Act
            var result = m_helper.IsCurrentAction(action, controller);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
