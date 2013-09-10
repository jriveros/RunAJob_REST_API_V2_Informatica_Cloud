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
    public abstract class RequestBase 
    {
        private string _contentType = null;
        private string _accept = null;
        private int _timeOut = 20000;
        private Version _protocolversion;

        public RequestBase(string hostUrl, string postData) : this(hostUrl, Encoding.UTF8, postData)
        {
        }

        public RequestBase(string hostUrl, Encoding encoding, string postData)
        {
            if (string.IsNullOrWhiteSpace(hostUrl))
            {
                throw new ArgumentNullException("hostUrl");
            }

            if (string.IsNullOrEmpty(postData))
            {
                this.postData = "";
            }
            else
            {
                this.postData = postData;
            }

            this.Encoding = encoding ?? Encoding.UTF8;
            this.HostUrl = hostUrl.Trim();
            this.QueryStringParams = new NameValueCollection();
            this.Cookies = new CookieCollection();
            this.Headers = new WebHeaderCollection();
            

        }

        public abstract HttpVerbs HttpVerbs
        {
            get;
        }

        public string HostUrl
        {
            get;
            private set;
        }

        public CookieCollection Cookies
        {
            get;
            private set;
        }

        public string postData
        {
            get;
            private set;
        }

        public string ContentType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._contentType))
                {
                    if (this.HttpVerbs == HttpVerbs.POST
                        || this.HttpVerbs == HttpVerbs.DELETE
                        || this.HttpVerbs == HttpVerbs.GET)
                    {
                        this._contentType = "application/xml";
                    }
                }

                return this._contentType;
            }
            set
            {
                this._contentType = value;
            }
        }

        public Version ProtocolVersion
        {
            get
            {
                return System.Net.HttpVersion.Version10;
            }
            set 
            {
                this._protocolversion = value;
            }
        }

        public string Accept
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._accept))
                {
                    if (this.HttpVerbs == HttpVerbs.POST 
                        || this.HttpVerbs == HttpVerbs.DELETE 
                        || this.HttpVerbs == HttpVerbs.GET)
                    {
                        this._accept = "application/xml";
                    }
                }

                return this._accept;
            }
            set
            {
                this._accept = value;
            }
        }

        public Encoding Encoding
        {
            get;
            set;
        }

        public int Timeout
        {
            get
            {
                return this._timeOut;
            }
            set
            {
                if (value >= 0)
                {
                    this._timeOut = value;
                }
            }
        }

        public NameValueCollection QueryStringParams
        {
            get;
            private set;
        }

        public string GetRequestUrl()
        {
            if (this.QueryStringParams != null && this.QueryStringParams.Count > 0)
            {
                if (!this.HostUrl.EndsWith("?"))
                {
                    this.HostUrl += "?";
                }

                return this.HostUrl + this.GetRequestParams(this.QueryStringParams);
            }

            return this.HostUrl;
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

        public abstract byte[] GetRequestBytes();

        public WebHeaderCollection Headers
        {
            get;
            private set;
        }


                
    }
}

