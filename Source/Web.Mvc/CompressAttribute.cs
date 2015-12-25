using System;
using System.IO.Compression;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace ReusableLibrary.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CompressAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Only apply compression when there is no exception or exception is handled
            if ((filterContext.Exception == null) || ((filterContext.Exception != null) && filterContext.ExceptionHandled))
            {
                CompressResponse(filterContext.HttpContext);
            }

            base.OnResultExecuted(filterContext);
        }

        private static void CompressResponse(HttpContextBase context)
        {
            HttpRequestBase request = context.Request;
            HttpResponseBase response = context.Response;
            string acceptEncoding = (request.Headers["Accept-Encoding"] ?? string.Empty).ToUpperInvariant();
            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
