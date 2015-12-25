using System.IO;
using System.Web;
using ReusableLibrary.Abstractions.IO;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web
{
    public sealed class ShrinkModule : HtmlFilterModule
    {
        public ShrinkModule()
            : base("ShrinkModule")
        {
        }

        public override void InstallFilter(HttpApplication app)
        {
            app.Response.Filter = new ShrinkStream(app.Response.Filter);
        }

        internal sealed class ShrinkStream : DecoratedStream
        {
            public ShrinkStream(Stream inner)
                : base(inner)
            {
            }

            #region Stream Members

            public override void Write(byte[] buffer, int offset, int count)
            {
                var length = ShrinkHelper.Shrink(buffer, offset, count);
                base.Write(buffer, offset, length);
            }

            #endregion
        }
    }
}
