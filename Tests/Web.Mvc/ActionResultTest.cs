using System.Web;
using System.Web.Mvc;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class ActionResultTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Mvc, "ActionResult")]
        public static void BadRequestResult_ExecuteResult()
        {
            // Arrange
            var result = new BadRequestResult();

            // Act
            var ex = Assert.Throws<HttpException>(() => result.ExecuteResult(new ControllerContext()));

            // Assert
            Assert.Equal(400, ex.GetHttpCode());
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "ActionResult")]
        public static void FileNotFoundResult_ExecuteResult()
        {
            // Arrange
            var result = new FileNotFoundResult();

            // Act
            var ex = Assert.Throws<HttpException>(() => result.ExecuteResult(new ControllerContext()));

            // Assert
            Assert.Equal(404, ex.GetHttpCode());
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "ActionResult")]
        public static void AccessDeniedResult_ExecuteResult()
        {
            // Arrange
            var result = new ForbiddenResult();

            // Act
            var ex = Assert.Throws<HttpException>(() => result.ExecuteResult(new ControllerContext()));

            // Assert
            Assert.Equal(403, ex.GetHttpCode());
        }
    }
}
