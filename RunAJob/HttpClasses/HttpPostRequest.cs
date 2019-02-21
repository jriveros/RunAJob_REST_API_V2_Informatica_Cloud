//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;


namespace ICS.Framework.RESTful
{
    public class HttpPostRequest : HttpFormRequestBase, IHttpRequestBase
    {
        public HttpPostRequest(string hostUrl, string postData) : base(hostUrl, postData)
        {

        }

        public HttpPostRequest(string hostUrl, Encoding encoding,string postData) : base(hostUrl, encoding, postData)
        {
        }

        public override HttpVerbs HttpVerbs
        {
            get
            {
                return HttpVerbs.POST;
            }
        }

        
    }
}
