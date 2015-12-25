using System.Web;

namespace ReusableLibrary.Captcha
{
    public sealed class SimpleCaptchaHttpHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var handler = new SimpleCaptchaHandler(new HttpContextWrapper(context));
            handler.RenderContent();
        }

        #endregion
    }
}
