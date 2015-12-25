using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;
using IoC = ReusableLibrary.Abstractions.IoC;

namespace ReusableLibrary.Web.Mvc.Tests.Integration
{
    public sealed class ControllerFactoryTest : IDisposable
    {
        private readonly MockControllerFactory m_factory;
        private readonly Mock<IoC::IDependencyResolver> m_resolverMock;

        public ControllerFactoryTest()
        {
            m_resolverMock = new Mock<IoC::IDependencyResolver>(MockBehavior.Strict);
            m_resolverMock.Setup(resolver => resolver.Dispose());
            IoC::DependencyResolver.InitializeWith(m_resolverMock.Object);
            var mockHttpContext = MockHttpFactory.GetHttpContext();
            m_factory = new MockControllerFactory();
            m_factory.RequestContext = new RequestContext(mockHttpContext.Object, new RouteData());
        }

        #region IDisposable Members

        public void Dispose()
        {
            IoC::DependencyResolver.Reset();
            m_resolverMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Integration, "ControllerFactory")]
        public void GetControllerInstance_ControllerType_IsNull()
        {
            // Arrange            

            // Act
            var ex = Assert.Throws<HttpException>(() => m_factory.GetControllerInstance2(null));

            // Assert
            Assert.Equal(404, ex.GetHttpCode());
        }

        [Fact]
        [Trait(Constants.TraitNames.Integration, "ControllerFactory")]
        public void GetControllerInstance()
        {
            // Arrange
            var controllerType = typeof(MockAbstractController);
            var viewDataAccessor = new ViewDataAccessor();
            m_resolverMock.Setup(resolver => resolver.Resolve<IViewDataAccessor>()).Returns(viewDataAccessor);
            m_resolverMock.Setup(resolver => resolver.Resolve<Controller>(controllerType)).Returns(new MockAbstractController());

            // Act
            var controller = m_factory.GetControllerInstance2(controllerType);

            // Assert
            Assert.NotNull(controller);
            Assert.NotNull(viewDataAccessor.ViewData);
            Assert.NotNull(controller.TempDataProvider);
            Assert.IsType<EmptyTempDataProvider>(controller.TempDataProvider);
        }
    }
}
