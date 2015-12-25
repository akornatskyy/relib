using System.Web;

namespace ReusableLibrary.Captcha
{
    public sealed class CaptchaHttpHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var handler = new CaptchaHandler(new HttpContextWrapper(context));
            handler.RenderContent();
        }

        #endregion
    }
}
