using System.Net;
using ReusableLibrary.Web.Mvc.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public sealed class ForbiddenResult : HttpErrorResult
    {
        public ForbiddenResult()
            : base(HttpStatusCode.Forbidden, GlobalResourceHelper.ErrorHttpAccessDenied())
        {
        }
    }
}
