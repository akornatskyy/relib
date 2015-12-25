using System;
using System.Web;
using System.Web.Mvc;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Mvc.Tests.Integration
{
    public sealed class ModelStateValidationAdapterTest : IDisposable
    {
        private readonly Mock<IViewDataAccessor> m_mockViewDataAccessor;
        private readonly ModelStateValidationAdapter m_adapter;

        public ModelStateValidationAdapterTest()
        {
            m_mockViewDataAccessor = new Mock<IViewDataAccessor>(MockBehavior.Strict);
            m_adapter = new ModelStateValidationAdapter(m_mockViewDataAccessor.Object);
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_mockViewDataAccessor.VerifyAll();
        }

        #endregion

        [Theory]
        [InlineData(null, "some error")]
        [InlineData("key", "some error")]
        [Trait(Constants.TraitNames.Integration, "ModelStateValidationAdapter")]
        public void AddError(string errorKey, string errorMessage)
        {
            // Arrange
            var viewData = new ViewDataDictionary();
            m_mockViewDataAccessor.Setup(viewDataAccessor => viewDataAccessor.ViewData).Returns(viewData);

            // Act
            m_adapter.AddError(errorKey, errorMessage);

            // Assert
            Assert.Equal(1, viewData.ModelState.Count);
            ModelState modelState = viewData.ModelState[errorKey ?? "__ERROR__"];
            Assert.Equal(1, modelState.Errors.Count);
            Assert.Equal(errorMessage, modelState.Errors[0].ErrorMessage);
        }
    }
}
