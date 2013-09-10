using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace ICS.Framework.RESTful
{
    public class HttpGetRequest : RequestBase , IHttpRequestBase
    {
        public HttpGetRequest(string hostUrl, string postData) : base(hostUrl, postData)
        {

        }

        public HttpGetRequest(string hostUrl, Encoding encoding, string postData) : base(hostUrl, encoding, postData)
        {
        }

        public override HttpVerbs HttpVerbs
        {
            get
            {
                return HttpVerbs.GET;
            }
        }

        public override byte[] GetRequestBytes()
        {
            return null;
        }
    }
}