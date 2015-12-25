using System.Net;
using ReusableLibrary.Web.Mvc.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public sealed class BadRequestResult : HttpErrorResult
    {
        public BadRequestResult()
            : base(HttpStatusCode.BadRequest, GlobalResourceHelper.ErrorHttpBadRequest())
        {
        }

        public BadRequestResult(string message)
            : base(HttpStatusCode.BadRequest, GlobalResourceHelper.ErrorHttpBadRequest()
            + " " + message)
        {
        }
    }
}
