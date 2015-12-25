using System;
using System.Collections.Specialized;

namespace ReusableLibrary.Abstractions.Net
{
    public interface IHttpClient
    {       
        string Get(Uri uri);

        string Get(Uri uri, NameValueCollection @params);

        string Post(Uri uri);

        string Post(Uri uri, NameValueCollection @params);

        NameValueCollection Head(Uri uri);

        NameValueCollection Head(Uri uri, NameValueCollection @params);
    }
}
