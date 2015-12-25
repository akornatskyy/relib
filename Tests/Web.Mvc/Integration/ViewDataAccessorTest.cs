using System.Web.Mvc;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Integration
{
    public sealed class ViewDataAccessorTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Integration, "ViewDataAccessor")]
        public static void ViewData()
        {
            // Arrange
            var viewData = new ViewDataDictionary();
            var mockController = new Mock<ControllerBase>(MockBehavior.Strict);
            var controller = mockController.Object;
            controller.ViewData = viewData;
            var viewDataAccessor = new ViewDataAccessor();
            viewDataAccessor.Setup(controller);

            // Act
            var result = viewDataAccessor.ViewData;

            // Assert
            Assert.Equal(viewData, result);
        }
    }
}
