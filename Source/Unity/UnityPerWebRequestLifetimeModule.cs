using System;
using System.Collections.Generic;
using System.Web;

namespace ReusableLibrary.Unity
{
    public sealed class UnityPerWebRequestLifetimeModule : IHttpModule
    {
        private static readonly object Key = new object();

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, e) => RemoveAllInstances();
        }

        #endregion

        internal static IDictionary<UnityPerWebRequestLifetimeManager, object> BackingStore
        {
            get { return GetInstances(HttpContext.Current); }
        }

        private static IDictionary<UnityPerWebRequestLifetimeManager, object> GetInstances(HttpContext httpContext)
        {
            IDictionary<UnityPerWebRequestLifetimeManager, object> instances;
            if (httpContext.Items.Contains(Key))
            {
                instances = (IDictionary<UnityPerWebRequestLifetimeManager, object>)httpContext.Items[Key];
            }
            else
            {
                instances = new Dictionary<UnityPerWebRequestLifetimeManager, object>();
                httpContext.Items.Add(Key, instances);
            }

            return instances;
        }

        private static void RemoveAllInstances()
        {
            var instances = BackingStore;
            if (instances != null && instances.Count > 0)
            {
                foreach (var entry in instances)
                {
                    IDisposable disposable = entry.Value as IDisposable;
                    if (disposable != null)
                    {
                        try
                        {
                            disposable.Dispose();
                        }
                        catch
                        {
                            // TODO:
                            // Must not fail others
                        }
                    }
                }

                instances.Clear();
            }
        }
    }
}
