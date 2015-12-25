using System.IO.Compression;
using System.Web;

namespace ReusableLibrary.Web
{
    public sealed class CompressModule : HtmlFilterModule
    {
        public CompressModule()
            : base("CompressModule")
        {
        }

        public override void InstallFilter(HttpApplication app)
        {
            var context = app.Context;
            var response = context.Response;
            var request = context.Request;
            
            response.Cache.VaryByHeaders["Accept-Encoding"] = true;
            var acceptEncoding = (request.Headers["Accept-Encoding"] ?? string.Empty).ToUpperInvariant();
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
