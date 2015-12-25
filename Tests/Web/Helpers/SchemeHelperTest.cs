using System.Collections.Specialized;
using ReusableLibrary.Web.Helpers;
using Xunit;
using Xunit.Extensions;

namespace ReusableLibrary.Web.Tests.Helpers
{
    public sealed class SchemeHelperTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Helpers, "SchemeHelper")]
        public static void Scheme_Empty()
        {
            // Arrange
            var variables = new NameValueCollection();

            // Act
            var result = SchemeHelper.Scheme(variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("http", result);
        }

        [Theory]
        [InlineData("HTTP_X_FORWARDED_PROTO", "http")]
        [InlineData("X_FORWARDED_PROTO", "http")]
        [InlineData("HTTPS", "")]
        [InlineData("SERVER_PORT_SECURE", "0")]
        [Trait(Constants.TraitNames.Helpers, "SchemeHelper")]
        public static void Scheme_Http(string header, string value)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add(header, value);

            // Act
            var result = SchemeHelper.Scheme(variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("http", result);
        }

        [Theory]
        [InlineData("HTTP_X_FORWARDED_PROTO", "https")]
        [InlineData("X_FORWARDED_PROTO", "https")]
        [InlineData("HTTPS", "on")]
        [InlineData("SERVER_PORT_SECURE", "1")]
        [Trait(Constants.TraitNames.Helpers, "SchemeHelper")]
        public static void Scheme_Https(string header, string value)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add(header, value);

            // Act
            var result = SchemeHelper.Scheme(variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https", result);
        }

        [Theory]
        [InlineData("HTTP_X_FORWARDED_PROTO", "https")]
        [InlineData("X_FORWARDED_PROTO", "https")]
        [InlineData("HTTPS", "on")]
        [Trait(Constants.TraitNames.Helpers, "SchemeHelper")]
        public static void Scheme_Https_ServerPortSecure_0(string header, string value)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("SERVER_PORT_SECURE", "0");
            variables.Add(header, value);            

            // Act
            var result = SchemeHelper.Scheme(variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https", result);
        }

        [Theory]
        [InlineData("HTTP_X_FORWARDED_PROTO", "http")]
        [InlineData("X_FORWARDED_PROTO", "http")]
        [InlineData("HTTPS", "")]
        [Trait(Constants.TraitNames.Helpers, "SchemeHelper")]
        public static void Scheme_Https_ServerPortSecure_1(string header, string value)
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("SERVER_PORT_SECURE", "1");
            variables.Add(header, value);

            // Act
            var result = SchemeHelper.Scheme(variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https", result);
        }
    }
}
