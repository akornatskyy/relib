using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using ReusableLibrary.Web.Mvc.Integration;
using Xunit;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public sealed class HttpRequestBaseExtensionsTest
    {
        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void UserHosts_ServerVariables_Empty()
        {
            // Arrange
            var variables = new NameValueCollection();
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.ServerVariables)
                .Returns(variables);

            // Act
            var result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void UserHosts_HTTP_X_FORWARDED_FOR()
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("HTTP_X_FORWARDED_FOR", "192.168.10.26, 192.168.10.30, 192.168.10.26");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.ServerVariables)
                .Returns(variables);

            // Act
            var result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("192.168.10.26", result[0]);
            Assert.Equal("192.168.10.30", result[1]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void UserHosts_REMOTE_ADDR()
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("REMOTE_ADDR", "192.168.10.5");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.ServerVariables)
                .Returns(variables);

            // Act
            var result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal("192.168.10.5", result[0]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void UserHosts_HTTP_X_CLUSTER_CLIENT_IP()
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("HTTP_X_CLUSTER_CLIENT_IP", "192.168.10.26, 192.168.10.30");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.ServerVariables)
                .Returns(variables);

            // Act
            var result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("192.168.10.26", result[0]);
            Assert.Equal("192.168.10.30", result[1]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void UserHosts_Order()
        {
            // Arrange
            var variables = new NameValueCollection();
            variables.Add("HTTP_X_FORWARDED_FOR", "192.168.10.1");
            variables.Add("HTTP_X_CLUSTER_CLIENT_IP", "192.168.10.2");
            variables.Add("REMOTE_ADDR", "192.168.10.3");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.ServerVariables)
                .Returns(variables);

            // Act
            var result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal("192.168.10.1", result[0]);

            // Arrange
            variables.Remove("HTTP_X_FORWARDED_FOR");

            // Act
            result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal("192.168.10.2", result[0]);

            // Arrange
            variables.Remove("HTTP_X_CLUSTER_CLIENT_IP");

            // Act
            result = mockHttpRequest.Object.UserHosts();

            // Assert
            Assert.Equal(1, result.Length);
            Assert.Equal("192.168.10.3", result[0]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void CheckParamPath_PathIsNull()
        {
            // Arrange
            var parameters = new NameValueCollection();
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.Params)
                .Returns(parameters);
            var routeValues = new RouteValueDictionary();

            // Act
            var result = mockHttpRequest.Object.CheckParamPath(routeValues, "returnurl");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void CheckParamPath_CurrentCulture_TwoLetterISOLanguageName_Equals_Path_Language()
        {
            // Arrange
            var parameters = new NameValueCollection();
            parameters.Add("returnurl", "/en/account");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.Params)
                .Returns(parameters);
            var routeValues = new RouteValueDictionary();
            SetupCulture("en-us");

            // Act
            var result = mockHttpRequest.Object.CheckParamPath(routeValues, "returnurl");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void CheckParamPath_CurrentCulture_Name_Equals_Path_Language()
        {
            // Arrange
            var parameters = new NameValueCollection();
            parameters.Add("returnurl", "/en-us/account");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.Params)
                .Returns(parameters);
            var routeValues = new RouteValueDictionary();
            SetupCulture("en-us");

            // Act
            var result = mockHttpRequest.Object.CheckParamPath(routeValues, "returnurl");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void CheckParamPath_Path_Language_IsSupported()
        {
            // Arrange
            Localization.Languages = new[] { "en", "zh-cn" };
            var parameters = new NameValueCollection();
            parameters.Add("returnurl", "/zh-cn/account");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.Params)
                .Returns(parameters);
            var routeValues = new RouteValueDictionary();
            SetupCulture("en");

            // Act
            var result = mockHttpRequest.Object.CheckParamPath(routeValues, "returnurl") as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.RouteValues.Count);
            Assert.Equal("zh-cn", result.RouteValues["language"]);
            Assert.Equal("/zh-cn/account", result.RouteValues["returnurl"]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Mvc, "HttpRequestBaseExtensions")]
        public static void CheckParamPath_Path_Language_IsNotSupported()
        {
            // Arrange
            Localization.Languages = new[] { "en", "zh-cn" };
            var parameters = new NameValueCollection();
            parameters.Add("returnurl", "/nl/account");
            var mockHttpRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            mockHttpRequest
                .Setup(request => request.Params)
                .Returns(parameters);
            var routeValues = new RouteValueDictionary();
            SetupCulture("en");

            // Act
            var result = mockHttpRequest.Object.CheckParamPath(routeValues, "returnurl") as RedirectToRouteResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.RouteValues.Count);
            Assert.Equal(Localization.DefaultLanguage, result.RouteValues["language"]);
        }

        private static void SetupCulture(string language)
        {
            var culture = CultureInfo.GetCultureInfo(language);
            var thread = Thread.CurrentThread;
            thread.CurrentUICulture = culture;
            thread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture.Name);
        }
    }
}
