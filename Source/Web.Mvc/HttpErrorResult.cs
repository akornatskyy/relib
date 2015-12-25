using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Web.Mvc
{
    public abstract class HttpErrorResult : ActionResult
    {
        protected HttpErrorResult(HttpStatusCode status, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            Status = status;
            Message = message;
            ErrorContext = StringHelper.WrapAt(new StackTrace(2, true).ToString(), 2048);
        }

        public HttpStatusCode Status { get; private set; }

        public string Message { get; private set; }

        public string ErrorContext { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var hex = new HttpException((int)Status, Message);
            var data = new NameValueCollection();
            data.Add("Error", String.Concat(Message, Environment.NewLine, ErrorContext));
            hex.Data["Http Error Context"] = data;
            throw hex;
        }
    }
}
