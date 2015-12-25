using System;
using System.Diagnostics;
using System.Web;
using System.Globalization;

namespace ReusableLibrary.Web
{
    public sealed class StopwatchModule : IHttpModule
    {
        private const string StopwatchKey = "__Stopwatch__";

        public static string Format(double seconds, DateTime current)
        {
            return string.Format(CultureInfo.InvariantCulture, "<!-- {0:N1} ms ({1:N1} req/sec); {2:s} -->", seconds * 1000F, 1F / seconds, current);
        }

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PostMapRequestHandler += new EventHandler(PostMapRequestHandler);
            app.PostRequestHandlerExecute += new EventHandler(PostRequestHandlerExecute);
        }

        #endregion

        private static void PostMapRequestHandler(object sender, EventArgs e)
        {            
            var context = ((HttpApplication)sender).Context;
            if (context.Error != null
                || context.CurrentHandler == null
                || context.CurrentHandler is DefaultHttpHandler)
            {
                return;
            }

            var stopwatch = new Stopwatch();
            context.Items[StopwatchKey] = stopwatch;
            stopwatch.Start();
        }

        private static void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Error != null
                || context.CurrentHandler == null
                || context.CurrentHandler is DefaultHttpHandler 
                || context.Response.ContentType != "text/html")
            {
                return;                
            }

            var stopwatch = (Stopwatch)context.Items[StopwatchKey];
            stopwatch.Stop();
            var seconds = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
            context.Response.Write(Format(seconds, DateTime.UtcNow));
        }
    }
}
