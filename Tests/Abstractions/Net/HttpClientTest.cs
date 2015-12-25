using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using ReusableLibrary.Abstractions.Net;
using Xunit;

namespace ReusableLibrary.Abstractions.Tests.Net
{
    public sealed class HttpClientTest
    {
        private static readonly MockWebResponse g_mockWebResponse = new MockWebResponse("Succeed");
        private static readonly MockWebRequest g_mockWebRequest = new MockWebRequest(g_mockWebResponse);        
        
        private readonly HttpClient m_httpClient;

        public HttpClientTest()
        {
            g_mockWebRequest.Reset();
            WebRequest.RegisterPrefix("test", new MockWebRequestCreate(g_mockWebRequest));
            m_httpClient = new HttpClient();            
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "HttpClient")]
        public void Get()
        {
            // Arrange

            // Act
            var result = m_httpClient.Get(new Uri("test://google.com/search"));

            // Assert
            Assert.Empty(g_mockWebRequest.Uri.Query);
            Assert.Equal("Succeed", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "HttpClient")]
        public void Get_With_Parameters()
        {
            // Arrange
            var parameters = new NameValueCollection();
            parameters.Add("a", "test1");
            parameters.Add("b", "test2");

            // Act
            var result = m_httpClient.Get(new Uri("test://google.com/search"), parameters);

            // Assert
            Assert.Equal("?a=test1&b=test2", g_mockWebRequest.Uri.Query);
            Assert.Equal("Succeed", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "HttpClient")]
        public void Post()
        {
            // Arrange

            // Act
            var result = m_httpClient.Post(new Uri("test://bing.com/search"));

            // Assert
            Assert.Empty(g_mockWebRequest.GetRequestBody());
            Assert.Equal("Succeed", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "HttpClient")]
        public void Post_With_Parameters()
        {
            // Arrange
            var parameters = new NameValueCollection();
            parameters.Add("a", "test1");
            parameters.Add("b", "test2");

            // Act
            var result = m_httpClient.Post(new Uri("test://bing.com/search"), parameters);

            // Assert
            Assert.Equal("a=test1&b=test2", g_mockWebRequest.GetRequestBody());
            Assert.Equal("Succeed", result);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "HttpClient")]
        public void Head()
        {
            // Arrange
            g_mockWebResponse.Headers.Add("Location", "http://localhost");

            // Act
            var result = m_httpClient.Head(new Uri("test://google.com/search"));

            // Assert
            Assert.Empty(g_mockWebRequest.Uri.Query);
            Assert.Equal(1, result.Count);
            Assert.Equal("http://localhost", result["Location"]);
        }

        [Fact]
        [Trait(Constants.TraitNames.Net, "HttpClient")]
        public void Head_With_Parameters()
        {
            // Arrange
            var parameters = new NameValueCollection();
            parameters.Add("a", "test1");
            parameters.Add("b", "test2");
            g_mockWebResponse.Headers.Add("Location", "http://localhost");

            // Act
            var result = m_httpClient.Head(new Uri("test://google.com/search"), parameters);

            // Assert
            Assert.Equal("?a=test1&b=test2", g_mockWebRequest.Uri.Query);
            Assert.Equal(1, result.Count);
            Assert.Equal("http://localhost", result["Location"]);
        }

        #region Mock WebRequest Details

        internal class MockWebRequestCreate : IWebRequestCreate
        {
            private readonly MockWebRequest m_request;

            public MockWebRequestCreate(MockWebRequest request)
            {
                m_request = request;
            }

            #region IWebRequestCreate Members

            public WebRequest Create(Uri uri)
            {
                m_request.Uri = uri;
                return m_request;
            }

            #endregion
        }

        internal class MockWebRequest : WebRequest, IDisposable
        {
            private readonly MockWebResponse m_response;
            private MemoryStream m_stream;            
            private string m_requestBody;

            public MockWebRequest(MockWebResponse response)
            {                
                m_response = response;
            }

            public void Reset()
            {
                m_stream = new MemoryStream();
                m_stream.Position = 0;
                m_response.Reset();
            }

            public override string Method { get; set; }

            public override string ContentType { get; set; }

            public override long ContentLength { get; set; }

            public Uri Uri { get; set; }

            public override Stream GetRequestStream()
            {
                return m_stream;
            }

            public string GetRequestBody()
            {
                return m_requestBody;
            }

            public override WebResponse GetResponse()
            {
                m_requestBody = Encoding.UTF8.GetString(m_stream.ToArray());
                return m_response;
            }

            #region IDisposable Members

            public void Dispose()
            {
                m_stream.Dispose();
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        internal class MockWebResponse : WebResponse
        {
            private readonly string m_response;
            private WebHeaderCollection m_headers;
            private MemoryStream m_stream;

            public MockWebResponse(string response)
            {
                m_response = response;
                Reset();
            }

            public void Reset()
            {
                m_stream = new MemoryStream(Encoding.UTF8.GetBytes(m_response));
                m_headers = new WebHeaderCollection();
            }

            public override WebHeaderCollection Headers
            {
                get
                {
                    return m_headers;
                }
            }

            public override Stream GetResponseStream()
            {
                return m_stream;
            }
        }

        #endregion
    }
}
