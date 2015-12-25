using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using ReusableLibrary.Abstractions.Helpers;

namespace ReusableLibrary.Abstractions.Net
{
    public sealed class HttpClient : IHttpClient
    {
        public HttpClient()
        {
            AllowAutoRedirect = true;
        }

        #region IHttpClient Members

        public string Get(Uri uri)
        {
            return Get(uri, new NameValueCollection());
        }

        public string Get(Uri uri, NameValueCollection @params)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = UriHelper.ToQuery(@params);
            var response = CreateRequest(uriBuilder.Uri).GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public string Post(Uri uri)
        {
            return Post(uri, new NameValueCollection());
        }

        public string Post(Uri uri, NameValueCollection @params)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            var request = CreateRequest(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var value = UriHelper.ToQuery(@params);
            request.ContentLength = value.Length;
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {                
                writer.Write(value);
                writer.Flush();

                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public NameValueCollection Head(Uri uri)
        {
            return Head(uri, new NameValueCollection());
        }

        public NameValueCollection Head(Uri uri, NameValueCollection @params)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = UriHelper.ToQuery(@params);
            var request = CreateRequest(uriBuilder.Uri);
            request.Method = "HEAD";            
            using (var response = request.GetResponse())
            {
                return new NameValueCollection(response.Headers);
            }
        }

        #endregion

        public bool AllowAutoRedirect { get; set; }

        public string UserAgent { get; set; }

        public TimeSpan Timeout { get; set; }

        private WebRequest CreateRequest(Uri uri)
        {
            var request = WebRequest.Create(uri);
            var httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = AllowAutoRedirect;
                httpRequest.KeepAlive = false;
                if (!String.IsNullOrEmpty(UserAgent))
                {
                    httpRequest.UserAgent = UserAgent;
                }
            }

            if (Timeout != TimeSpan.Zero)
            {
                request.Timeout = (int)(Timeout.Ticks / TimeSpan.TicksPerMillisecond);
            }

            return request;
        }
    }
}
