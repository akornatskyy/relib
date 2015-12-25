using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace ReusableLibrary.Web.Mvc.Tests
{
    public static class MockHttpFactory
    {
        public static readonly string AppPathModifier = string.Empty;

        public static Mock<HttpContextBase> GetHttpContext()
        {
            return GetHttpContext("/", null, null);
        }

        public static Mock<HttpContextBase> GetHttpContext(string appPath, string requestPath, string httpMethod)
        {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp.ToString(), -1);
        }

        public static Mock<HttpContextBase> GetHttpContext(string appPath, string requestPath, string httpMethod, string protocol, int port)
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            if (!String.IsNullOrEmpty(appPath))
            {
                mockHttpContext.SetupGet(c => c.Request.ApplicationPath).Returns(appPath);
            }

            if (!String.IsNullOrEmpty(requestPath))
            {
                mockHttpContext.SetupGet(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri = new Uri(protocol + "://localhost" + ":" + Convert.ToString(port >= 0 ? port : 80, CultureInfo.InvariantCulture));
            mockHttpContext.SetupGet(c => c.Request.Url).Returns(uri);
            mockHttpContext.SetupGet(c => c.Request.PathInfo).Returns(String.Empty);
            if (!String.IsNullOrEmpty(httpMethod))
            {
                mockHttpContext.SetupGet(c => c.Request.HttpMethod).Returns(httpMethod);
            }

            mockHttpContext.SetupGet(c => c.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            return mockHttpContext;
        }

        public static HtmlHelper GetHtmlHelper()
        {
            return GetHtmlHelper(GetHttpContext().Object);
        }

        public static HtmlHelper GetHtmlHelper(HttpContextBase httpContext)
        {
            var rt = new RouteCollection();
            rt.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });
            rt.Add("namedroute", new Route("named/{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });

            var viewContext = GetViewContext(httpContext);
            var viewDataContainer = GetViewDataContainer(viewContext.ViewData);
            HtmlHelper htmlHelper = new HtmlHelper(viewContext, viewDataContainer, rt);
            return htmlHelper;
        }

        private static ViewContext GetViewContext(HttpContextBase httpContext)
        {
            var rd = new RouteData();
            rd.Values.Add("controller", "home");
            rd.Values.Add("action", "oldaction");

            ViewDataDictionary vdd = new ViewDataDictionary();

            var mockViewContext = new Mock<ViewContext>();
            mockViewContext.SetupGet(c => c.HttpContext).Returns(httpContext);
            mockViewContext.SetupGet(c => c.RouteData).Returns(rd);
            mockViewContext.SetupGet(c => c.ViewData).Returns(vdd);
            return mockViewContext.Object;
        }

        private static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
        {
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.SetupGet(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }
    }
}
