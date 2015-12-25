using System;
using System.Diagnostics;
using System.Reflection;
using Moq;
using ReusableLibrary.Abstractions.Tracing;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Abstractions.Tests.Tracing
{
    public sealed class TraceHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Tracing, "TraceHelper")]
        public static void TraceVerbose()
        {
            // Arrange
            var traceInfo = new TraceInfo(new TraceSource("source", SourceLevels.Verbose));

            // Act
            TraceHelper.TraceVerbose(traceInfo, "test");

            // Assert
            Assert.True(traceInfo.IsVerboseEnabled);
            Assert.True(traceInfo.IsInfoEnabled);
            Assert.True(traceInfo.IsWarningEnabled);
            Assert.True(traceInfo.IsErrorEnabled);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "TraceHelper")]
        public static void TraceInfo()
        {
            // Arrange
            var traceInfo = new TraceInfo(new TraceSource("source", SourceLevels.Information));

            // Act
            TraceHelper.TraceInfo(traceInfo, "test");

            // Assert
            Assert.False(traceInfo.IsVerboseEnabled);
            Assert.True(traceInfo.IsInfoEnabled);
            Assert.True(traceInfo.IsWarningEnabled);
            Assert.True(traceInfo.IsErrorEnabled);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "TraceHelper")]
        public static void TraceWarning()
        {
            // Arrange
            var traceInfo = new TraceInfo(new TraceSource("source", SourceLevels.Warning));

            // Act
            TraceHelper.TraceWarning(traceInfo, "test");

            // Assert
            Assert.False(traceInfo.IsVerboseEnabled);
            Assert.False(traceInfo.IsInfoEnabled);
            Assert.True(traceInfo.IsWarningEnabled);
            Assert.True(traceInfo.IsErrorEnabled);
        }

        [Fact]
        [Trait(Constants.TraitNames.Tracing, "TraceHelper")]
        public static void TraceError()
        {
            // Arrange
            var traceInfo = new TraceInfo(new TraceSource("source", SourceLevels.Error));

            // Act
            TraceHelper.TraceError(traceInfo, "test");

            // Assert
            Assert.False(traceInfo.IsVerboseEnabled);
            Assert.False(traceInfo.IsInfoEnabled);
            Assert.False(traceInfo.IsWarningEnabled);
            Assert.True(traceInfo.IsErrorEnabled);
        }
    }
}
