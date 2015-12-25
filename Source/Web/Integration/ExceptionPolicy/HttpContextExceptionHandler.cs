using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Tracing;

namespace ReusableLibrary.Web.Integration
{
    public class HttpContextExceptionHandler : IExceptionHandler
    {
        public HttpContextExceptionHandler()
        {
            Exclude = ServerVariables = new string[] { };
        }

        public string[] ServerVariables { get; set; }

        public string[] Exclude { get; set; }

        #region IExceptionHandler Members

        public bool HandleException(Exception ex)
        {
            if (HttpContext.Current == null)
            {
                return false;
            }

            AddServerVariables(ex);
            AddRequestQueryString(ex);
            AddRequestForm(ex);
            AddCookies(ex);

            return false;
        }

        #endregion

        public void AddServerVariables(Exception ex)
        {
            var data = new NameValueCollection();
            var variables = HttpContext.Current.Request.ServerVariables;
            foreach (var key in ServerVariables)
            {
                var value = variables[key];
                if (!String.IsNullOrEmpty(value))
                {
                    data.Add(key, value);
                }
            }

            if (data.Count > 0)
            {
                ex.Data.Add("Http Request Server Variables", data);
            }
        }

        public void AddRequestQueryString(Exception ex)
        {
            var data = Filter(HttpContext.Current.Request.QueryString);
            if (data.Count > 0)
            {
                ex.Data.Add("Http Request Query String", data);
            }
        }

        public void AddRequestForm(Exception ex)
        {
            var data = Filter(HttpContext.Current.Request.Form);
            if (data.Count > 0)
            {
                ex.Data.Add("Http Request Form", data);
            }
        }

        public void AddCookies(Exception ex)
        {
            var data = Filter(HttpContext.Current.Request.Cookies);
            if (data.Count > 0)
            {
                ex.Data.Add("Http Request Cookies", data);
            }
        }

        private NameValueCollection Filter(NameValueCollection collection)
        {
            var result = new NameValueCollection();
            try
            {
                foreach (var key in collection.AllKeys.Where(k => k != null && !k.StartsWith("_", StringComparison.Ordinal)))
                {
                    if (Exclude.Where(k => StringHelper.Contains(key, k, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault() != null)
                    {
                        continue;
                    }

                    try
                    {
                        var value = collection[key];
                        if (!String.IsNullOrEmpty(value))
                        {
                            result.Add(key, value);
                        }
                    }
                    catch (HttpRequestValidationException hrvex)
                    {
                        result.Add(key, hrvex.Message);
                    }
                }
            }
            catch (HttpRequestValidationException hrvex)
            {
                result.Add("HttpRequestValidationException", hrvex.Message);
            }

            return result;
        }

        private NameValueCollection Filter(HttpCookieCollection collection)
        {
            var result = new NameValueCollection();
            try
            {
                foreach (var key in collection.AllKeys.Where(k => k != null && !k.StartsWith("_", StringComparison.Ordinal)))
                {
                    if (Exclude.Where(k => StringHelper.Contains(key, k, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault() != null)
                    {
                        continue;
                    }

                    try
                    {
                        result.Add(key, collection[key].Value);
                    }
                    catch (HttpRequestValidationException hrvex)
                    {
                        result.Add(key, hrvex.Message);
                    }
                }
            }
            catch (HttpRequestValidationException hrvex)
            {
                result.Add("HttpRequestValidationException", hrvex.Message);
            }

            return result;
        }
    }
}
