using System;
using System.Security;
using Moq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Abstractions.Services;
using Xunit;
using Xunit.Extensions;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Abstractions.Tests.Services
{
    public sealed class AbstractServiceTest : IDisposable
    {
        private readonly MockAbstractService m_abstractService;
        private readonly Mock<IValidationService> m_validationServiceMock;
        private readonly Mock<IValidationState> m_validationStateMock;

        public AbstractServiceTest()
        {
            m_validationServiceMock = new Mock<IValidationService>(MockBehavior.Strict);
            m_validationStateMock = new Mock<IValidationState>(MockBehavior.Strict);
            m_abstractService = new MockAbstractService();
            m_abstractService.ValidationService = m_validationServiceMock.Object;
            m_abstractService.ValidationState = m_validationStateMock.Object;
        }
        
        #region IDisposable Members

        public void Dispose()
        {
            m_validationServiceMock.VerifyAll();
            m_validationStateMock.VerifyAll();
        }

        #endregion

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Object_DoesNot_Pass_Validation_No_Return_Result()
        {
            // Arrange
            var validatable = new object();
            m_validationServiceMock.Setup((validationService) => validationService.Validate(validatable)).Returns(false);

            // Act
            var res = m_abstractService.WithValid2(validatable, () => "test");

            // Assert
            Assert.Null(res);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Object_Return_Result()
        {
            // Arrange
            var validatable = new object();
            m_validationServiceMock.Setup((validationService) => validationService.Validate(validatable)).Returns(true);

            // Act
            var res = m_abstractService.WithValid2(validatable, () => "test");

            // Assert
            Assert.Equal("test", res);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Object_DoesNot_Pass_Validation_No_Action()
        {
            // Arrange
            var actionCalled = false;
            var validatable = new object();
            m_validationServiceMock.Setup((validationService) => validationService.Validate(validatable)).Returns(false);

            // Act
            var res = m_abstractService.WithValid2(validatable, () => { actionCalled = true; });

            // Assert
            Assert.False(res);
            Assert.False(actionCalled);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Object_Call_Action()
        {
            // Arrange
            var actionCalled = false;
            var validatable = new object();
            m_validationServiceMock.Setup((validationService) => validationService.Validate(validatable)).Returns(true);

            // Act
            var res = m_abstractService.WithValid2(validatable, () => { actionCalled = true; });

            // Assert
            Assert.True(res);
            Assert.True(actionCalled);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_False_No_Return_Result()
        {
            // Arrange

            // Act
            var res = m_abstractService.WithValid2(false, () => "test");

            // Assert
            Assert.Null(res);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_True_Return_Result()
        {
            // Arrange

            // Act
            var res = m_abstractService.WithValid2(true, () => "test");

            // Assert
            Assert.Equal("test", res);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_False_No_Action()
        {
            // Arrange
            var actionCalled = false;

            // Act
            var res = m_abstractService.WithValid2(false, () => { actionCalled = true; });

            // Assert
            Assert.False(res);
            Assert.False(actionCalled);
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_True_Call_Action()
        {
            // Arrange
            var actionCalled = false;

            // Act
            var res = m_abstractService.WithValid2(true, () => { actionCalled = true; });

            // Assert
            Assert.True(res);
            Assert.True(actionCalled);
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(SecurityException))]
        [InlineData(typeof(UnauthorizedAccessException))]
        [InlineData(typeof(RepositoryFailureException))]
        [InlineData(typeof(RepositoryGuardException))]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Handles_Action_Exception(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type, "test");
            m_validationStateMock.Setup((validationState) => validationState.AddError(null, "test"));

            // Act
            Assert.DoesNotThrow(() => m_abstractService.WithValid2(true, () => { throw ex; }));

            // Assert
        }

        [Fact]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Handles_Action_RepositoryGuardAreaException()
        {
            // Arrange
            var ex = new RepositoryGuardAreaException("area", 100, "message");
            m_validationStateMock.Setup((validationState) => validationState.AddError("area", "message"));

            // Act
            Assert.DoesNotThrow(() => m_abstractService.WithValid2(true, () => { throw ex; }));

            // Assert
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(SecurityException))]
        [InlineData(typeof(UnauthorizedAccessException))]
        [InlineData(typeof(RepositoryFailureException))]
        [InlineData(typeof(RepositoryGuardException))]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Handles_Action_Exception_With_ExceptionHandler(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type, "test");
            var exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);
            exceptionHandlerMock.Setup(exceptionHandler => exceptionHandler.HandleException(ex)).Returns(true);
            m_abstractService.ExceptionHandler = exceptionHandlerMock.Object;            
            m_validationStateMock.Setup((validationState) => validationState.AddError(null, "test"));

            // Act
            Assert.DoesNotThrow(() => m_abstractService.WithValid2(true, () => { throw ex; }));

            // Assert
            exceptionHandlerMock.VerifyAll();
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(SecurityException))]
        [InlineData(typeof(UnauthorizedAccessException))]
        [InlineData(typeof(RepositoryFailureException))]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Handles_Return_Result_Exception(Type type)
        {
            // Arrange
            string result = "something";
            var ex = (Exception)Activator.CreateInstance(type, "test");
            m_validationStateMock.Setup((validationState) => validationState.AddError(null, "test"));

            // Act
            Assert.DoesNotThrow(() =>
            {
                result = m_abstractService.WithValid2(true, () =>
                    {
                        if (ex != null)
                        {
                            throw ex;
                        }

                        return "test";
                    });
            });

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(SecurityException))]
        [InlineData(typeof(UnauthorizedAccessException))]
        [InlineData(typeof(RepositoryFailureException))]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Handles_Return_Result_Exception_With_ExceptionHandler(Type type)
        {
            // Arrange
            string result = "something";
            var ex = (Exception)Activator.CreateInstance(type, "test");
            var exceptionHandlerMock = new Mock<IExceptionHandler>(MockBehavior.Strict);
            exceptionHandlerMock.Setup(exceptionHandler => exceptionHandler.HandleException(ex)).Returns(true);
            m_abstractService.ExceptionHandler = exceptionHandlerMock.Object;            
            m_validationStateMock.Setup((validationState) => validationState.AddError(null, "test"));

            // Act
            Assert.DoesNotThrow(() =>
            {
                result = m_abstractService.WithValid2(true, () =>
                {
                    if (ex != null)
                    {
                        throw ex;
                    }

                    return "test";
                });
            });

            // Assert
            Assert.Null(result);
            exceptionHandlerMock.VerifyAll();
        }

        [Theory]
        [InlineData(typeof(SystemException))]
        [InlineData(typeof(ApplicationException))]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Does_Not_Handles_Action_Exception(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type, "test");

            // Act
            Assert.Throws(type, () => m_abstractService.WithValid2(true, () => { throw ex; }));

            // Assert
        }

        [Theory]
        [InlineData(typeof(SystemException))]
        [InlineData(typeof(ApplicationException))]
        [Trait(Constants.TraitNames.Services, "AbstractService")]
        public void WithValid_Does_Not_Handles_Return_Result_Exceptions(Type type)
        {
            // Arrange
            string result = "something";
            var ex = (Exception)Activator.CreateInstance(type, "test");

            // Act
            Assert.Throws(type, () =>
            {
                result = m_abstractService.WithValid2(true, () =>
                {
                    if (ex != null)
                    {
                        throw ex;
                    }

                    return "test";
                });
            });

            // Assert
            Assert.Equal("something", result);
        }
    }
}
