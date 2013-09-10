using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Net;
using System.IO;


namespace ICS.Framework.RESTful
{
    public abstract class HttpFormRequestBase : RequestBase, IHttpFormParams
    {
        public HttpFormRequestBase(string hostUrl, string postData) : this(hostUrl, Encoding.UTF8, postData)
        {
        }

        public HttpFormRequestBase(string hostUrl, Encoding encoding, string postData) : base(hostUrl, encoding,postData)
        {
        }

        public NameValueCollection FormParams
        {
            get;
            private set;
        }

        public override byte[] GetRequestBytes()
        {
            string requestBytes = GetRequestParams(this.FormParams);

            if (string.IsNullOrWhiteSpace(requestBytes))
            {
                return null;
            }

            return this.Encoding.GetBytes(requestBytes);
        }

        private string GetRequestParams(NameValueCollection collection)
        {
            if (collection != null && collection.Count > 0)
            {
                var builder = new StringBuilder();

                foreach (var key in collection.AllKeys)
                {
                    builder.Append(HttpUtility.UrlEncode(key, this.Encoding));
                    builder.Append("=");
                    builder.Append(HttpUtility.UrlEncode(collection[key], this.Encoding));
                    builder.Append("&");
                }

                return builder.Remove(builder.Length - 1, 1).ToString();
            }

            return string.Empty;
        }
    }
}
