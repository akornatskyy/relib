using System;
using System.Net;
using System.Web;
using System.Web.Caching;

namespace ReusableLibrary.Web
{
    public sealed class DenialModule : IHttpModule
    {
        private const string KeyPrefix = "DenialModule-";        
        private static readonly object g_banned = new object();

        public static void Deny(HttpContext context, DateTime timestamp)
        {
            var response = context.Response;
            var cache = response.Cache;
            cache.SetLastModified(timestamp);
            cache.SetExpires(timestamp.AddMinutes(1));
            cache.SetCacheability(HttpCacheability.Private);
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.End();
        }

        public static void Block(HttpContext context, string ip, DateTime expiresAt)
        {
            var key = KeyPrefix + ip;
            context.Cache.Insert(key, g_banned, null, expiresAt, 
                Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
        }

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication app)
        {
            app.PostMapRequestHandler += new EventHandler(OnEnter);
        }

        #endregion

        private static void OnEnter(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Error != null
                || context.CurrentHandler == null
                || context.CurrentHandler is DefaultHttpHandler)
            {
                return;
            }

            var timestamp = DateTime.UtcNow;
            foreach (var ip in context.Request.UserHosts())
            {
                var key = KeyPrefix + ip;
                var item = context.Cache.Get(key);
                if (item != null)
                {
                    Deny(context, timestamp);
                    return;
                }
            }
        }
    }
}
