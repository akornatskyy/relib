using System.Web;
using System.Web.Mvc;
using Moq;
using ReusableLibrary.Web.Mvc.Helpers;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Helpers
{
    public sealed class SubstitutionHelperTest
    {
        private static bool g_substitutionAdded;

        private readonly Mock<HttpContextBase> m_mockContext;
        private readonly HtmlHelper m_htmlHelper;

        public SubstitutionHelperTest()
        {
            if (!g_substitutionAdded)
            {
                HttpResponseSubstitutionHandler.Add("name", (c, s) => "result");
                g_substitutionAdded = true;
            }

            m_mockContext = MockHttpFactory.GetHttpContext();
            m_htmlHelper = MockHttpFactory.GetHtmlHelper(m_mockContext.Object);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "SubstitutionHelper")]
        public void RenderSubstitution()
        {
            // Arrange

            // Act
            var result = SubstitutionHelper.RenderSubstitution(m_htmlHelper, "name").ToHtmlString();

            // Assert
            Assert.Equal("result", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Helpers, "SubstitutionHelper")]
        public void RenderSubstitution_Token()
        {
            // Arrange
            m_mockContext.Setup(c => c.Handler).Returns(new HttpResponseSubstitutionHandler(null));

            // Act
            var result = SubstitutionHelper.RenderSubstitution(m_htmlHelper, "name").ToHtmlString();

            // Assert
            Assert.Equal(HttpResponseSubstitutionHandler.Token("name"), result);
        }
    }
}
