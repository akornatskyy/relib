using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Moq;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Tracing;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Tracing
{
    public sealed class ExceptionPolicyTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void ExceptionPolicyHandler_HandleNull()
        {
            // Arrange
            var handler = new ExceptionPolicyHandler();

            // Act
            var result = handler.HandleException(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void ExceptionPolicyHandler_Chain_IsNull()
        {
            // Arrange
            var handler = new ExceptionPolicyHandler();
            handler.Chain = null;

            // Act
            var result = handler.HandleException(new InvalidOperationException());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException))]
        [InlineData(typeof(ArgumentOutOfRangeException))]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void ExceptionPolicyHandler(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type, "test", new InvalidOperationException());
            var handlerMock1 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            var handlerMock2 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            var handlerMock3 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            handlerMock1.Setup(h => h.HandleException(ex)).Returns(false);
            handlerMock2.Setup(h => h.HandleException(ex)).Returns(true);
            var handler = new ExceptionPolicyHandler();
            handler.Chain = new[] { handlerMock1.Object, handlerMock2.Object, handlerMock3.Object };

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.True(result);
            handlerMock1.VerifyAll();
            handlerMock2.VerifyAll();
            handlerMock3.VerifyAll();
        }

        [Theory]
        [InlineData(typeof(TargetInvocationException))]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void ExceptionPolicyHandler_Handle_TargetInvocationException(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type, "test", new InvalidOperationException());
            var handlerMock1 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            var handlerMock2 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            var handlerMock3 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            handlerMock1.Setup(h => h.HandleException(ex.InnerException)).Returns(false);
            handlerMock2.Setup(h => h.HandleException(ex.InnerException)).Returns(true);
            var handler = new ExceptionPolicyHandler();
            handler.Chain = new[] { handlerMock1.Object, handlerMock2.Object, handlerMock3.Object };

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.True(result);
            handlerMock1.VerifyAll();
            handlerMock2.VerifyAll();
            handlerMock3.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void ExceptionPolicyHandler_Unhandled()
        {
            // Arrange
            var ex = new InvalidOperationException();
            var handlerMock1 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            var handlerMock2 = new Mock<IExceptionHandler>(MockBehavior.Strict);
            handlerMock1.Setup(h => h.HandleException(ex)).Returns(false);
            handlerMock2.Setup(h => h.HandleException(ex)).Returns(false);
            var handler = new ExceptionPolicyHandler();
            handler.Chain = new[] { handlerMock1.Object, handlerMock2.Object };

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
            handlerMock1.VerifyAll();
            handlerMock2.VerifyAll();
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void EventLogExceptionHandler()
        {
            // Arrange
            var handler = new EventLogExceptionHandler();

            // Act
            var result = handler.HandleException(new InvalidOperationException());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void NullExceptionHandler()
        {
            // Arrange
            var handler = new NullExceptionHandler();

            // Act
            var result = handler.HandleException(new InvalidOperationException());

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(ArgumentNullException))]
        [InlineData(typeof(ArgumentOutOfRangeException))]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void RethrowExceptionHandler(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type);
            var handler = new RethrowExceptionHandler<ArgumentException>();

            // Act
            Assert.Throws(type, () => handler.HandleException(ex));

            // Assert
        }

        [Theory]
        [InlineData(typeof(InvalidOperationException))]
        [InlineData(typeof(InvalidTimeZoneException))]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void RethrowExceptionHandler_NoMatch(Type type)
        {
            // Arrange
            var ex = (Exception)Activator.CreateInstance(type);
            var handler = new RethrowExceptionHandler<ArgumentException>();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void TraceExceptionHandler()
        {
            // Arrange
            var handler = new TraceExceptionHandler();

            // Act
            var result = handler.HandleException(new InvalidOperationException());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void IgnoreExceptionHandler()
        {
            // Arrange
            var handler = new IgnoreExceptionHandler<SystemException>();

            // Act
            var result = handler.HandleException(new FormatException());

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void IgnoreExceptionHandler_NoMatch()
        {
            // Arrange
            var handler = new IgnoreExceptionHandler<ArgumentException>();

            // Act
            var result = handler.HandleException(new InvalidOperationException());

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void IgnoreExceptionHandler_CheckInner()
        {
            // Arrange
            var handler = new IgnoreExceptionHandler<SystemException>() 
            { 
                CheckInner = true
            };

            // Act
            var result = handler.HandleException(new InvalidOperationException("x", new FormatException()));

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void IgnoreExceptionHandler_CheckInner_NoMatch()
        {
            // Arrange
            var handler = new IgnoreExceptionHandler<ArgumentException>()
            {
                CheckInner = true
            };

            // Act
            var result = handler.HandleException(new InvalidOperationException("x", new NotSupportedException()));

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void PerformanceCounterExceptionHandler()
        {
            // Arrange
            var handler = new PerformanceCounterExceptionHandler()
            {
                Counters = new[] 
                { 
                    @"\Memory\Pages/sec",
                    @"\PhysicalDisk(_Total)\Avg. Disk Queue Length",
                    @"\Processor(_Total)\% Processor Time"
                }
            };
            var ex = new InvalidOperationException();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
            var counters = (NameValueCollection)ex.Data["Performance Counters"];
            Assert.Equal(3, counters.Count);
            Assert.True(handler.Counters.All(name => NameValueCollectionHelper.HasKey(counters, name)));
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "ExceptionPolicy")]
        public static void PerformanceCounterExceptionHandler_Counter_Not_Available()
        {
            // Arrange
            var handler = new PerformanceCounterExceptionHandler()
            {
                Counters = new[] 
                { 
                    @"\XXXXX\XXXXXX"
                },
                WatchWindow = 1
            };
            var ex = new InvalidOperationException();

            // Act
            var result = handler.HandleException(ex);

            // Assert
            Assert.False(result);
            var counters = (NameValueCollection)ex.Data["Performance Counters"];
            Assert.Equal(1, counters.Count);
            Assert.Equal("N/A", counters[0]);
        }
    }
}
