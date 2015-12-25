using System.Web;

namespace ReusableLibrary.Web
{
    public abstract class HtmlFilterModule : ContentFilterModule
    {
        protected HtmlFilterModule(string name)
            : base(name)
        {
        }

        protected override void TryInstallFilter(HttpApplication app)
        {
            if (app.Response.ContentType == "text/html")
            {
                base.TryInstallFilter(app);
            }
        }
    }
}
