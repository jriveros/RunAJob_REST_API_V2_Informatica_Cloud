using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.IO;
using System;



namespace ICS.Framework.RESTful
{
    public interface IHttpRequestBase

    {
        HttpVerbs HttpVerbs { get; }        
        string HostUrl { get; }
        CookieCollection Cookies { get; }
        string postData { get; }
        string ContentType { get; set; }
        Version ProtocolVersion { get; set; }
        string Accept { get; set; }
        Encoding Encoding { get; set; }
        int Timeout { get; set; }
        NameValueCollection QueryStringParams { get; }
        string GetRequestUrl();
        byte[] GetRequestBytes();
        WebHeaderCollection Headers {get;}

     
    }
}