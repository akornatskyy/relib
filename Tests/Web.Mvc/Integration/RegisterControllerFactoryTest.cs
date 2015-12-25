using System.Web.Mvc;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests.Integration
{
    public sealed class RegisterControllerFactoryTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Integration, "RegisterControllerFactory")]
        public static void Execute()
        {
            // Arrange
            var mockControllerFactory = new Mock<IControllerFactory>(MockBehavior.Strict);
            var task = new RegisterControllerFactory(mockControllerFactory.Object);

            // Act
            task.Execute();

            // Assert
            Assert.Equal(mockControllerFactory.Object, ControllerBuilder.Current.GetControllerFactory());
        }
    }
}
