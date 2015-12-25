using System.Net;
using ReusableLibrary.Web.Mvc.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public sealed class FileNotFoundResult : HttpErrorResult
    {
        public FileNotFoundResult()
            : base(HttpStatusCode.NotFound, GlobalResourceHelper.ErrorHttpNotFound())
        {
        }
    }
}
